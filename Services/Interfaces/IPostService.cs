using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CheerMeApp.Contracts.V1.Responses;
using CheerMeApp.Models;

namespace CheerMeApp.Services.Interfaces
{
    public interface IPostService
    {
        Task<List<Post>> GetPostsAsync(PaginationFilter paginationFilter);
        Task<Post> GetPostByIdAsync(Guid postId);
        Task<bool> UpdatePostAsync(Post postToUpdate);
        Task<bool> DeletePostAsync(Guid postId);
        Task<bool> CreatePostAsync(Post post);
        Task<bool> UserOwnsPostAsync(Guid postId, string userId);
        Task<bool> LikePost(Like like);
        Task<bool> UnLikePost(Like likeToDelete);
        Task<List<Like>> GetLikes(Guid postId);
        int GetLikesCount(Guid postId);
        Task<bool> IsLiked(string userId, Guid postId);
    }
}