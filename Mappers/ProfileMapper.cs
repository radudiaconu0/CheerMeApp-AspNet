using AutoMapper;
using CheerMeApp.Contracts.V1.Responses;
using CheerMeApp.Models;

namespace CheerMeApp.Mappers
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            CreateMap<Post, PostResponse>().ReverseMap();
            CreateMap<User, UserResponse>().ReverseMap();
            CreateMap<Like, LikeResponse>().ReverseMap();
        }
    }
}