using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CheerMeApp.Contracts.V1.Responses;
using CheerMeApp.Data;
using CheerMeApp.Extensions;
using CheerMeApp.Models;
using CheerMeApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CheerMeApp.Services
{
    public class LikeService : ILikeService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;

        public LikeService(ApplicationDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor, IPostService postService, ICommentService commentService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _postService = postService;
            _commentService = commentService;
        }

        public async Task<bool> LikedByUserAsync(string userId, string entityId, string entityType)
        {
            Like like = null;
            if (entityType == nameof(Post))
            {
                like = await _dbContext.Likes.AsNoTracking().SingleOrDefaultAsync(x =>
                    x.UserId == userId && x.PostId == new Guid(entityId));
            }
            else
            {
                like = await _dbContext.Likes.AsNoTracking().SingleOrDefaultAsync(x =>
                    x.UserId == userId && x.CommentId == new Guid(entityId));
            }

            return like != null;
        }


        public async Task<List<Like>> GetLikesAsync(string entityId, string entityType)
        {
            List<Like> likes;
            if (entityType == nameof(Post))
            {
                likes = await _dbContext.Likes.Include(like => like.Liker).Where(like =>
                    like.PostId == new Guid(entityId)).ToListAsync();
            }
            else
            {
                likes = await _dbContext.Likes.Include(like => like.Liker).Where(like =>
                    like.CommentId == new Guid(entityId)).ToListAsync();
            }

            return likes;
        }

        public async Task<Like> GetLike(string userId, string entityId, string entityType)
        {
            Like like;
            if (entityType == nameof(Post))
            {
                like = await _dbContext.Likes.SingleOrDefaultAsync(x =>
                    x.UserId == userId && x.PostId == new Guid(entityId));
            }
            else
            {
                like = await _dbContext.Likes.SingleOrDefaultAsync(x =>
                    x.UserId == userId && x.CommentId == new Guid(entityId));
            }

            return like;
        }

        public async Task<bool> LikeAsync(Guid entityId, string entityType)
        {
            Like like;
            if (entityType == nameof(Post))
            {
                var post = await _postService.GetPostByIdAsync(entityId);
                if (post == null)
                {
                    return false;
                }

                like = new Like
                {
                    UserId = _httpContextAccessor.HttpContext.GetUserId(),
                    PostId = entityId,
                    CommentId = null,
                    CreatedAt = DateTime.UtcNow,
                };
            }
            else
            {
                var comment = await _commentService.GetCommentByIdAsync(entityId);
                if (comment == null)
                {
                    return false;
                }

                like = new Like
                {
                    UserId = _httpContextAccessor.HttpContext.GetUserId(),
                    PostId = null,
                    CommentId = entityId,
                    CreatedAt = DateTime.UtcNow,
                };
            }


            like.UpdatedAt = like.CreatedAt;
            await _dbContext.Likes.AddAsync(like);
            var liked = await _dbContext.SaveChangesAsync();
            return liked > 0;
        }

        public async Task<bool> UnLikeAsync(Guid entityId, string entityType)
        {
            Like like = null;
            if (entityType == nameof(Post))
            {
                var post = await _postService.GetPostByIdAsync(entityId);
                if (post == null)
                {
                    return false;
                }

                like = await GetLike(_httpContextAccessor.HttpContext.GetUserId(), entityId.ToString(),
                    nameof(Post));
                if (like == null)
                    return false;
                _dbContext.Likes.Remove(like);
            }
            else
            {
                var comment = await _commentService.GetCommentByIdAsync(entityId);
                if (comment == null)
                {
                    return false;
                }

                like = await GetLike(_httpContextAccessor.HttpContext.GetUserId(), entityId.ToString(),
                    nameof(Comment));
                if (like == null)
                    return false;
                _dbContext.Likes.Remove(like);
            }


            var deleted = await _dbContext.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<List<Like>> GetLikesAsync(Guid entityId, string entityType)
        {
            List<Like> likes;
            if (entityType == nameof(Post))
            {
                likes = await _dbContext.Likes.Where(like => like.PostId == entityId).ToListAsync();
            }
            else
            {
                likes = await _dbContext.Likes.Where(like => like.CommentId == entityId).ToListAsync();
            }

            return likes;
        }

        public async Task<bool> IsLikedAsync(Guid entityId, string entityType)
        {
            var userId = _httpContextAccessor.HttpContext.GetUserId();
            var isLiked = await LikedByUserAsync(userId, entityId.ToString(), nameof(Post));
            return isLiked;
        }
    }
}