using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CheerMeApp.Data;
using CheerMeApp.Extensions;
using CheerMeApp.Models;
using CheerMeApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CheerMeApp.Services
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostService(ApplicationDbContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Post>> GetPostsAsync(PaginationFilter paginationFilter = null)
        {
            if (paginationFilter == null)
            {
                return await _dbContext.Posts.Include(post => post.User)
                    .Include(post => post.Likes)
                    .Include(post => post.Comments)
                    .ToListAsync();
            }

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await _dbContext.Posts.Include(post => post.User)
                .Include(post => post.User)
                .Include(post => post.Comments)
                .Include(post => post.Likes)
                .OrderByDescending(post => post.CreatedAt)
                .Skip(skip)
                .Take(paginationFilter.PageSize)
                .ToListAsync();
        }

        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            return await _dbContext.Posts.Include(post => post.User)
                .Include(post => post.Likes)
                .Include(post => post.Comments)
                .SingleOrDefaultAsync(post => post.Id == postId);
        }

        public async Task<bool> UpdatePostAsync(Post postToUpdate)
        {
            postToUpdate.UpdatedAt = DateTime.UtcNow;
            _dbContext.Posts.Update(postToUpdate);
            var updated = await _dbContext.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<bool> DeletePostAsync(Guid postId)
        {
            var post = await GetPostByIdAsync(postId);
            if (post == null)
                return false;
            _dbContext.Posts.Remove(post);
            var deleted = await _dbContext.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<bool> CreatePostAsync(Post post)
        {
            post.CreatedAt = DateTime.UtcNow;
            post.UpdatedAt = post.CreatedAt;
            await _dbContext.Posts.AddAsync(post);
            var created = await _dbContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> UserOwnsPostAsync(string postId, string userId)
        {
            var post = await _dbContext.Posts.AsNoTracking().SingleOrDefaultAsync(x => x.Id.ToString() == postId);
            return post != null && post.UserId == userId;
        }

    }
}