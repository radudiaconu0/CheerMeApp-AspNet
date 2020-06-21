using System;
using CheerMeApp.Contracts.V1.Requests.Queries;

namespace CheerMeApp.Services.Interfaces
{
    public interface IUriService
    {
        Uri GetPostById(string postId);
        
        Uri GetAllPostUri(PaginationQuery paginationQuery = null);
    }
}