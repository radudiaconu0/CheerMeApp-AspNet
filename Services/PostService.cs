using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheerMeApp.Data;
using CheerMeApp.Models;
using CheerMeApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CheerMeApp.Services
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILikeService _likeService;

        public PostService(ApplicationDbContext dbContext, ILikeService likeService)
        {
            _dbContext = dbContext;
            _likeService = likeService;
        }

        public async Task<List<Post>> GetPostsAsync(PaginationFilter paginationFilter = null)
        {
            if (paginationFilter == null)
            {
                return await _dbContext.Posts.Include(post => post.User).ToListAsync();
            }

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await _dbContext.Posts.Include(post => post.User).Skip(skip).Take(paginationFilter.PageSize)
                .ToListAsync();
        }

        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            return await _dbContext.Posts.Include(post => post.User).SingleOrDefaultAsync(post => post.Id == postId);
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
            await _dbContext.AddAsync(post);
            var created = await _dbContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> UserOwnsPostAsync(Guid postId, string userId)
        {
            var post = await _dbContext.Posts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == postId);
            return post != null && post.UserId == userId;
        }

        public async Task<bool> LikePost(Like like)
        {
            var post = await GetPostByIdAsync(new Guid(like.LikableId));
            if (post == null)
            {
                return false;
            }

            await _dbContext.Likers.AddAsync(like);
            var liked = await _dbContext.SaveChangesAsync();
            return liked > 0;
        }

        public async Task<bool> UnLikePost(Like likeToDelete)
        {
            var post = await GetPostByIdAsync(new Guid(likeToDelete.LikableId));
            if (post == null)
            {
                return false;
            }

            var like = await _likeService.GetLike(likeToDelete.LikerId, likeToDelete.LikableId,
                likeToDelete.LikableType);
            if (like == null)
                return false;
            _dbContext.Likers.Remove(like);
            var deleted = await _dbContext.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<List<Like>> GetLikes(Guid postId)
        {
            var likes = await _likeService.GetLikesAsync(postId, nameof(Post));
            return likes;
        }

        public int GetLikesCount(Guid postId)
        {
            var likesCount = _likeService.GetLikeCount(postId, nameof(Post));
            return likesCount;
        }

        public async Task<bool> IsLiked(string userId, Guid postId)
        {
            var isLiked = await _likeService.LikedByUserAsync(userId, postId, nameof(Post));
            return isLiked;
        }
    }
}