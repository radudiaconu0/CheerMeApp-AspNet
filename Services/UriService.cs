using System;
using CheerMeApp.Contracts.V1;
using CheerMeApp.Contracts.V1.Requests.Queries;
using CheerMeApp.Services.Interfaces;
using Microsoft.AspNetCore.WebUtilities;

namespace CheerMeApp.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;

        public UriService(string baseUri)
        {
            _baseUri = baseUri + "api/v1/posts";
        }

        public Uri GetPostById(string postId)
        {
            return new Uri(_baseUri + ApiRoutes.Posts.Get.Replace("{postId}", postId));
        }

        public Uri GetAllPostUri(PaginationQuery paginationQuery = null)
        {
            var uri = new Uri(_baseUri);
            if (paginationQuery == null)
            {
                return uri;
            }

            var modifiedUri =
                QueryHelpers.AddQueryString(_baseUri, "pageNumber",
                    paginationQuery.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", paginationQuery.PageSize.ToString());
            return new Uri(modifiedUri);
        }
    }
}