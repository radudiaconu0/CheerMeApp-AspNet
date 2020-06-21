using System.Collections.Generic;
using System.Linq;
using CheerMeApp.Contracts.V1.Requests.Queries;
using CheerMeApp.Contracts.V1.Responses;
using CheerMeApp.Models;
using CheerMeApp.Services.Interfaces;

namespace CheerMeApp.Helpers
{
    public static class PaginationHelpers
    {
        public static PagedResponse<T> CreatePaginatedResponse<T>(IUriService uriService,
            PaginationFilter paginationFilter, List<T> postResponses)
        {
            var nextPage = paginationFilter.PageNumber >= 1
                ? uriService.GetAllPostUri(new PaginationQuery(paginationFilter.PageNumber + 1,
                    paginationFilter.PageSize)).ToString()
                : null;
            var previousPage = paginationFilter.PageNumber - 1 >= 1
                ? uriService.GetAllPostUri(new PaginationQuery(paginationFilter.PageNumber - 1,
                    paginationFilter.PageSize)).ToString()
                : null;
            return new PagedResponse<T>
            {
                Data = postResponses,
                PageNumber = paginationFilter.PageNumber >= 1 ? paginationFilter.PageNumber : (int?) null,
                PageSize = paginationFilter.PageSize >= 1 ? paginationFilter.PageSize : (int?) null,
                NextPage = postResponses.Any() ? nextPage : null,
                PreviousPage = previousPage,
            };
        }
    }
}