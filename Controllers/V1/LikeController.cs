using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CheerMeApp.Contracts.V1;
using CheerMeApp.Contracts.V1.Responses;
using CheerMeApp.Extensions;
using CheerMeApp.Models;
using CheerMeApp.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CheerMeApp.Controllers.V1
{
    [ApiController]
    [EnableCors("CorsPolicy")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class LikeController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ILikeService _likeService;
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        public LikeController(IPostService postService, ILikeService likeService, IMapper mapper, ICommentService commentService)
        {
            _postService = postService;
            _likeService = likeService;
            _mapper = mapper;
            _commentService = commentService;
        }

        [HttpPost(ApiRoutes.Posts.LikePost)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> LikePost([FromRoute] Guid postId)
        {
            var likedByUser =
                await _likeService.LikedByUserAsync(HttpContext.GetUserId(), postId.ToString(), nameof(Post));
            if (likedByUser)
                return BadRequest();

            var liked = await _likeService.LikeAsync(postId, nameof(Post));
            if (liked)
                return Ok();
            return NotFound();
        }

        [HttpDelete(ApiRoutes.Posts.UnLikePost)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UnLikePost([FromRoute] Guid postId)
        {
            var liked = await _likeService.LikedByUserAsync(HttpContext.GetUserId(), postId.ToString(), nameof(Post));
            if (!liked) return BadRequest();
            var unLiked = await _likeService.UnLikeAsync(postId, nameof(Post));
            if (unLiked)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpGet(ApiRoutes.Posts.GetLikes)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLikesPostAsync([FromRoute] Guid postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);
            if (post == null) return NotFound();
            var likes = await _likeService.GetLikesAsync(postId, nameof(Post));
            var likesResponse = _mapper.Map<List<LikeResponse>>(likes);
            return Ok(new Response<List<LikeResponse>>(likesResponse));
        }
        [HttpPost(ApiRoutes.Comments.LikeComment)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> LikeComment([FromRoute] Guid commentId)
        {
            var likedByUser =
                await _likeService.LikedByUserAsync(HttpContext.GetUserId(), commentId.ToString(), nameof(Comment));
            if (likedByUser)
                return BadRequest();

            var liked = await _likeService.LikeAsync(commentId, nameof(Comment));
            if (liked)
                return Ok();
            return NotFound();
        }

        [HttpDelete(ApiRoutes.Comments.UnLikeComment)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UnLikeComment([FromRoute] Guid commentId)
        {
            var liked = await _likeService.LikedByUserAsync(HttpContext.GetUserId(), commentId.ToString(), nameof(Comment));
            if (!liked) return BadRequest();
            var unLiked = await _likeService.UnLikeAsync(commentId, nameof(Post));
            if (unLiked)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpGet(ApiRoutes.Comments.GetCommentLikes)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLikes([FromRoute] Guid commentId)
        {
            var post = await _commentService.GetCommentByIdAsync(commentId);
            if (post == null) return NotFound();
            var likes = await _likeService.GetLikesAsync(commentId, nameof(Comment));
            var likesResponse = _mapper.Map<List<LikeResponse>>(likes);
            return Ok(new Response<List<LikeResponse>>(likesResponse));
        }
    }
}