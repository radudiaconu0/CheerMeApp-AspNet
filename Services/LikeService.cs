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

        public async Task<bool> LikedByUserAsync(string userId, Guid likableId, string likableType)
        {
            var like = await _dbContext.Likers.AsNoTracking().SingleOrDefaultAsync(x =>
                x.LikerId == userId && x.LikableId == likableId.ToString() && x.LikableType == likableType);
            return like != null;
        }

        public int GetLikeCount(Guid likableId, string likableType)
        {
            return _dbContext.Likers.Count(like =>
                like.LikableId == likableId.ToString() && like.LikableType == likableType);
        }

        public async Task<List<Like>> GetLikesAsync(Guid likableId, string likableType)
        {
            var likes = await _dbContext.Likers.Include(like => like.Liker).Where(like =>
                like.LikableId == likableId.ToString() && like.LikableType == likableType).ToListAsync();

            return likes;
        }
        
        public async Task<Like> GetLike(string userId, string likableId, string likableType)
        {
            var like = await _dbContext.Likers.SingleOrDefaultAsync(x =>
                x.LikerId == userId && x.LikableId == likableId && x.LikableType == likableType);
            return like;
        }
        
    }
}