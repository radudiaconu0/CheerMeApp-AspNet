using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CheerMeApp.Contracts.V1;
using CheerMeApp.Contracts.V1.Requests;
using CheerMeApp.Contracts.V1.Requests.Queries;
using CheerMeApp.Contracts.V1.Responses;
using CheerMeApp.Extensions;
using CheerMeApp.Helpers;
using CheerMeApp.Models;
using CheerMeApp.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CheerMeApp.Controllers.V1
{
    [ApiController]
    [EnableCors("CorsPolicy")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class CommentsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ILikeService _likeService;
        private readonly IUriService _uriService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ICommentService _commentService;

        public CommentsController(IPostService postService, IUriService uriService, UserManager<User> userManager,
            IMapper mapper, ICommentService commentService, ILikeService likeService)
        {
            _postService = postService;
            _uriService = uriService;
            _userManager = userManager;
            _mapper = mapper;
            _commentService = commentService;
            _likeService = likeService;
        }

        [HttpPost(ApiRoutes.Posts.CreateComment)]
        public async Task<IActionResult> Create([FromBody] CreateCommentRequest commentRequest, [FromRoute] Guid postId)
        {
            var comment = new Comment
            {
                CommentText = commentRequest.CommentText,
                PostId = postId,
                UserId = HttpContext.GetUserId()
            };
            await _commentService.CreateCommentAsync(comment);
            var locationUrl = _uriService.GetCommentById(comment.Id.ToString());
            comment.User = await _userManager.FindByIdAsync(comment.UserId);
            return Created(locationUrl, new Response<CommentResponse>(_mapper.Map<CommentResponse>(comment)));
        }

        [HttpPost(ApiRoutes.Comments.ReplyComment)]
        public async Task<IActionResult> CreateReply([FromBody] CreateCommentRequest commentRequest,
            [FromRoute] Guid commentId)
        {
            var comment = await _commentService.GetCommentByIdAsync(commentId);
            if (comment == null)
            {
                return NotFound();
            }

            var reply = new Comment
            {
                CommentText = commentRequest.CommentText,
                ParentId = commentId,
                PostId = comment.PostId,
                UserId = HttpContext.GetUserId()
            };
            await _commentService.CreateCommentAsync(reply);
            var locationUrl = _uriService.GetCommentById(comment.Id.ToString());
            reply.User = await _userManager.FindByIdAsync(comment.UserId);
            return Created(locationUrl, new Response<CommentResponse>(_mapper.Map<CommentResponse>(reply)));
        }

        [HttpGet(ApiRoutes.Comments.GetCommentReplies)]
        public async Task<IActionResult> GetRepliesAsync([FromRoute] Guid commentId,
            [FromQuery] PaginationQuery paginationQuery)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationQuery);
            var comment = await _commentService.GetCommentByIdAsync(commentId);
            if (comment == null)
            {
                return NotFound();
            }

            var replies = await _commentService.GetRepliesByCommentAsync(commentId, paginationFilter);
            var response = _mapper.Map<List<CommentResponse>>(replies);
            foreach (var reply in response)
                reply.Liked =
                    await _likeService.LikedByUserAsync(HttpContext.GetUserId(), comment.Id.ToString(),
                        nameof(Comment));

            var paginationResponse =
                PaginationHelpers.CreatePaginatedResponse(_uriService, paginationFilter, response);
            return Ok(paginationResponse);
        }


        [HttpDelete(ApiRoutes.Comments.DeleteComment)]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid commentId)
        {
            var userOwnsComment =
                await _commentService.UserOwnsCommentAsync(commentId.ToString(), HttpContext.GetUserId());
            if (!userOwnsComment)
            {
                return BadRequest(new {error = "You do not own this comment"});
            }

            var deleted = await _commentService.DeleteCommentAsync(commentId);
            if (deleted)
                return NoContent();

            return NotFound();
        }

        [HttpPut(ApiRoutes.Comments.UpdateComment)]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid commentId,
            [FromBody] UpdateCommentRequest request)
        {
            var userOwnsPost =
                await _commentService.UserOwnsCommentAsync(commentId.ToString(), HttpContext.GetUserId());
            if (!userOwnsPost)
            {
                return BadRequest(new {error = "You do not own this comment"});
            }

            var comment = await _commentService.GetCommentByIdAsync(commentId);
            if (comment == null)
            {
                return NotFound();
            }

            comment.CommentText = request.CommentText;
            comment.UpdatedAt = DateTime.UtcNow;
            var updated = await _commentService.UpdateCommentAsync(commentId);

            if (!updated) return NotFound();
            var response = _mapper.Map<CommentResponse>(comment);
            return Ok(new Response<CommentResponse>(response));
        }

        [HttpGet(ApiRoutes.Posts.GetCommentsByPost)]
        public async Task<IActionResult> GetPostCommentsAsync([FromRoute] Guid postId,
            [FromQuery] PaginationQuery paginationQuery)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationQuery);
            var post = await _postService.GetPostByIdAsync(postId);
            if (post == null)
            {
                return NotFound();
            }

            var comments = await _commentService.GetCommentsByPostAsync(postId, paginationFilter);


            var response = _mapper.Map<List<CommentResponse>>(comments);
            foreach (var comment in response)
                comment.Liked =
                    await _likeService.LikedByUserAsync(HttpContext.GetUserId(), comment.Id.ToString(),
                        nameof(Comment));
            var paginationResponse =
                PaginationHelpers.CreatePaginatedResponse(_uriService, paginationFilter, response);
            return Ok(paginationResponse);
        }
    }
}