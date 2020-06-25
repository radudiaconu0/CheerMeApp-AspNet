using System;
using CheerMeApp.Contracts.V1.Requests.Queries;

namespace CheerMeApp.Services.Interfaces
{
    public interface IUriService
    {
        Uri GetPostById(string postId);

        Uri GetAllPostUri(PaginationQuery paginationQuery = null);
        Uri GetCommentById(string commentId);

        Uri GetAllCommentsUri(PaginationQuery paginationQuery = null);
    }
}