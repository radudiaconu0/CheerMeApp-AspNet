using System.Linq;
using AutoMapper;
using CheerMeApp.Contracts.V1.Responses;
using CheerMeApp.Extensions;
using CheerMeApp.Models;
using CheerMeApp.Services;
using Microsoft.AspNetCore.Http;

namespace CheerMeApp.Mappers
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            CreateMap<Post, PostResponse>()
                .ForMember(response => response.LikesCount, opt => opt.MapFrom(post => post.Likes.Count))
                .ForMember(response => response.CommentsCount, opt => opt.MapFrom(post => post.Comments.Count))
                .ReverseMap();
            CreateMap<User, UserResponse>().ReverseMap();
            CreateMap<Like, LikeResponse>().ReverseMap();
            CreateMap<Comment, CommentResponse>()
                .ForMember(response => response.LikesCount, opt => opt.MapFrom(comment => comment.Likes.Count))
                .ForMember(response => response.RepliesCount, opt => opt.MapFrom(comment => comment.Replies.Count))
                .ReverseMap();
        }
    }
}