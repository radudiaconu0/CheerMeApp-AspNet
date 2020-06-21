using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CheerMeApp.Contracts.V1.Responses;
using CheerMeApp.Models;

namespace CheerMeApp.Services.Interfaces
{
    public interface ILikeService
    {
        Task<bool> LikedByUserAsync(string userId, Guid likableId, string likableType);
        int GetLikeCount(Guid likableId, string likableType);
        Task<List<Like>> GetLikesAsync(Guid likableId, string likableType);
        Task<Like> GetLike(string userId, string likableId, string likableType);
    }
}