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
        Task<bool> UserOwnsPostAsync(string postId, string userId);
        // Task<List<Like>> GetLikesAsync(Guid postId);
        // Task<bool> IsLikedAsync(Guid postId);
    }
}