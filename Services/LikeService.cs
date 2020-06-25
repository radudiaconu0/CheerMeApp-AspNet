using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CheerMeApp.Contracts.V1.Responses;
using CheerMeApp.Data;
using CheerMeApp.Models;
using CheerMeApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CheerMeApp.Services
{
    public class LikeService : ILikeService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public LikeService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
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
            List<Like> likes = null;
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
            Like like = null;
            if (entityType == nameof(Post))
            {
                like = await _dbContext.Likes.SingleOrDefaultAsync(x =>
                    x.UserId == userId && x.PostId == new Guid(entityId));
            }
            return like;
        }
    }
}