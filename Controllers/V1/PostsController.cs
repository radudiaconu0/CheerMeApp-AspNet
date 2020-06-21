using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CheerMeApp.Contracts.V1.Requests;
using CheerMeApp.Contracts.V1.Responses;
using CheerMeApp.Extensions;
using CheerMeApp.Models;
using CheerMeApp.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CheerMeApp.Contracts.V1;
using CheerMeApp.Contracts.V1.Requests.Queries;
using CheerMeApp.Data;
using CheerMeApp.Helpers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;

namespace CheerMeApp.Controllers.V1
{
    [EnableCors("MyPolicy")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        private readonly ILikeService _likeService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IUriService _uriService;

        public PostsController(IPostService postService, IMapper mapper, UserManager<User> userManager,
            IUriService uriService, ILikeService likeService)
        {
            _postService = postService;
            _mapper = mapper;
            _userManager = userManager;
            _uriService = uriService;
            _likeService = likeService;
        }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery paginationQuery)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationQuery);
            var posts = await _postService.GetPostsAsync(paginationFilter);
            var postResponse = _mapper.Map<List<PostResponse>>(posts);
            foreach (var post in postResponse)
            {
                post.LikesCount = _postService.GetLikesCount(post.Id);
                post.Liked = await _postService.IsLiked(HttpContext.GetUserId(), post.Id);
            }

            if (paginationQuery == null || paginationQuery.PageNumber < 1 || paginationQuery.PageSize < 1)
            {
                return Ok(new PagedResponse<PostResponse>(postResponse));
            }

            var paginationResponse =
                PaginationHelpers.CreatePaginatedResponse(_uriService, paginationFilter, postResponse);
            return Ok(paginationResponse);
        }

        [HttpGet(ApiRoutes.Posts.Get)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] Guid postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);
            if (post == null)
            {
                return NotFound();
            }

            var postResponse = _mapper.Map<PostResponse>(post);
            postResponse.LikesCount = _postService.GetLikesCount(post.Id);
            postResponse.Liked = await _postService.IsLiked(HttpContext.GetUserId(), postId);
            return Ok(new Response<PostResponse>(postResponse));
        }

        [HttpPut(ApiRoutes.Posts.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] Guid postId, [FromBody] UpdatePostRequest request)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());
            if (!userOwnsPost)
            {
                return BadRequest(new {error = "You do not own this post"});
            }

            var post = await _postService.GetPostByIdAsync(postId);
            post.PostText = request.PostText;
            post.UpdatedAt = DateTime.UtcNow;
            var updated = await _postService.UpdatePostAsync(post);

            if (!updated) return NotFound();
            var response = _mapper.Map<PostResponse>(post);
            return Ok(new Response<PostResponse>(response));
        }

        [HttpDelete(ApiRoutes.Posts.Delete)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid postId)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());
            if (!userOwnsPost)
            {
                return BadRequest(new {error = "You do not own this post"});
            }

            var deleted = await _postService.DeletePostAsync(postId);
            if (deleted)
                return NoContent();

            return NotFound();
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest postRequest)
        {
            var newPostId = Guid.NewGuid();
            var post = new Post {Id = newPostId, PostText = postRequest.PostText, UserId = HttpContext.GetUserId()};
            await _postService.CreatePostAsync(post);
            var locationUrl = _uriService.GetPostById(post.Id.ToString());
            post.User = await _userManager.FindByIdAsync(post.UserId);
            return Created(locationUrl, new Response<PostResponse>(_mapper.Map<PostResponse>(post)));
        }

        [HttpPost(ApiRoutes.Posts.LikePost)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> LikePost([FromRoute] Guid postId)
        {
            if (await _likeService.LikedByUserAsync(HttpContext.GetUserId(), postId, nameof(Post)))
            {
                return BadRequest();
            }

            var like = new Like
                {LikerId = HttpContext.GetUserId(), LikableId = postId.ToString(), LikableType = nameof(Post)};
            var liked = await _postService.LikePost(like);
            if (liked)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost(ApiRoutes.Posts.UnLikePost)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> UnLikePost([FromRoute] Guid postId)
        {
            var liked = await _likeService.LikedByUserAsync(HttpContext.GetUserId(), postId, nameof(Post));
            if (!liked) return BadRequest();
            var like = new Like
                {LikerId = HttpContext.GetUserId(), LikableId = postId.ToString(), LikableType = nameof(Post)};
            var unLiked = await _postService.UnLikePost(like);
            if (unLiked)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet(ApiRoutes.Posts.GetLikes)]
        public async Task<IActionResult> GetLikes([FromRoute] Guid postId)
        {
            var likes = await _postService.GetLikes(postId);
            var likesResponse = _mapper.Map<List<LikeResponse>>(likes);
            return Ok(new Response<List<LikeResponse>>(likesResponse));
        }
    }
}