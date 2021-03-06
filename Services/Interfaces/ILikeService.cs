﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CheerMeApp.Contracts.V1.Responses;
using CheerMeApp.Models;

namespace CheerMeApp.Services.Interfaces
{
    public interface ILikeService
    {
        Task<bool> LikedByUserAsync(string userId, string entityId, string entityType);
        Task<List<Like>> GetLikesAsync(string entityId, string entityType);
        Task<Like> GetLike(string userId, string entityId, string entityType);
        Task<bool> LikeAsync(Guid entityId, string entityType);
        Task<bool> UnLikeAsync(Guid entityId, string entityType);
        Task<List<Like>> GetLikesAsync(Guid entityId, string entityType);
        Task<bool> IsLikedAsync(Guid entityId, string entityType);
    }
}