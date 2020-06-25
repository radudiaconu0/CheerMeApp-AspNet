using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CheerMeApp.Models;

namespace CheerMeApp.Services.Interfaces
{
    public interface ICommentService
    {
        Task<bool> CreateCommentAsync(Comment comment);
        Task<bool> UserOwnsCommentAsync(string commentId, string userId);
        Task<bool> DeleteCommentAsync(Guid commentId);
        Task<Comment> GetCommentByIdAsync(Guid commentId);
        Task<bool> UpdateCommentAsync(Guid commentId);
        Task<List<Comment>> GetCommentsByPostAsync(Guid postId, PaginationFilter paginationFilter);
        Task<bool> CreateReply(Guid commentId, Comment comment, PaginationFilter paginationFilter);
    }
}