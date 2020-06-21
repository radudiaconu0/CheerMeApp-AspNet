using AutoMapper;
using CheerMeApp.Contracts.V1.Requests.Queries;
using CheerMeApp.Models;

namespace CheerMeApp.Mappers
{
    public class RequestMapper : Profile
    {
        public RequestMapper()
        {
            CreateMap<PaginationQuery, PaginationFilter>();
        }
    }
}