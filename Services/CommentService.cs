using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheerMeApp.Data;
using CheerMeApp.Models;
using CheerMeApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CheerMeApp.Services
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IPostService _postService;

        public CommentService(ApplicationDbContext dbContext, IPostService postService)
        {
            _dbContext = dbContext;
            _postService = postService;
        }

        public async Task<bool> CreateCommentAsync(Comment comment)
        {
            comment.CreatedAt = DateTime.UtcNow;
            comment.UpdatedAt = comment.CreatedAt;
            await _dbContext.Comments.AddAsync(comment);
            var created = await _dbContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> UserOwnsCommentAsync(string commentId, string userId)
        {
            var comment = await _dbContext.Comments.AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id.ToString() == commentId);
            return comment != null && comment.UserId == userId;
        }

        public async Task<bool> DeleteCommentAsync(Guid commentId)
        {
            var comment = await GetCommentByIdAsync(commentId);
            if (comment == null)
                return false;
            _dbContext.Comments.Remove(comment);
            var deleted = await _dbContext.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<bool> UpdateCommentAsync(Guid commentId)
        {
            var comment = await GetCommentByIdAsync(commentId);
            if (comment == null)
                return false;
            _dbContext.Comments.Update(comment);
            var updated = await _dbContext.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<List<Comment>> GetCommentsByPostAsync(Guid postId, PaginationFilter paginationFilter = null)
        {
            if (paginationFilter == null)
            {
                return await _dbContext.Comments.Include(comment => comment.User)
                    .Where(comment => comment.PostId == postId).ToListAsync();
            }

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await _dbContext.Comments
                .Include(comment => comment.User)
                .Where(comment => comment.PostId == postId)
                .Skip(skip)
                .Take(paginationFilter.PageSize)
                .ToListAsync();
        }

        public async Task<bool> CreateReply(Guid commentId, Comment comment, PaginationFilter paginationFilter)
        {
            comment.CreatedAt = DateTime.UtcNow;
            comment.UpdatedAt = comment.CreatedAt;
            comment.ParentId = commentId;
            await _dbContext.Comments.AddAsync(comment);
            var created = await _dbContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<Comment> GetCommentByIdAsync(Guid commentId)
        {
            return await _dbContext.Comments.Include(comment => comment.User)
                .SingleOrDefaultAsync(comment => comment.Id == commentId);
        }
    }
}