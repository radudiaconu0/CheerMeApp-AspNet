using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CheerMeApp.Contracts.V1;
using CheerMeApp.Contracts.V1.Responses;
using CheerMeApp.Extensions;
using CheerMeApp.Models;
using CheerMeApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CheerMeApp.Controllers.V1
{
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ILikeService _likeService;
        private readonly IMapper _mapper;

        public LikeController(IPostService postService, ILikeService likeService, IMapper mapper)
        {
            _postService = postService;
            _likeService = likeService;
            _mapper = mapper;
        }

        [HttpPost(ApiRoutes.Posts.LikePost)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> LikePost([FromRoute] Guid postId)
        {
            var likedByUser =
                await _likeService.LikedByUserAsync(HttpContext.GetUserId(), postId.ToString(), nameof(Post));
            if (likedByUser)
                return BadRequest();

            var liked = await _postService.LikePostAsync(postId);
            if (liked)
                return Ok();
            return NotFound();
        }

        [HttpPost(ApiRoutes.Posts.UnLikePost)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UnLikePost([FromRoute] Guid postId)
        {
            var liked = await _likeService.LikedByUserAsync(HttpContext.GetUserId(), postId.ToString(), nameof(Post));
            if (!liked) return BadRequest();
            var unLiked = await _postService.UnLikePostAsync(postId);
            if (unLiked)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpGet(ApiRoutes.Posts.GetLikes)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLikes([FromRoute] Guid postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);
            if (post == null) return NotFound();
            var likes = await _postService.GetLikesAsync(postId);
            var likesResponse = _mapper.Map<List<LikeResponse>>(likes);
            return Ok(new Response<List<LikeResponse>>(likesResponse));
        }
    }
}