using System;
using System.Threading.Tasks;
using AutoMapper;
using CheerMeApp.Data;
using CheerMeApp.Extensions;
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
    public class FollowersController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IUriService _uriService;

        public FollowersController(ApplicationDbContext dbContext, IMapper mapper, UserManager<User> userManager,
            IUriService uriService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
            _uriService = uriService;
        }

        [HttpPost("follow/{userId}")]
        public async Task<IActionResult> FollowUser(string followingId)
        {
            var authUser = await _userManager.FindByIdAsync(HttpContext.GetUserId());
            var followingUser = await _userManager.FindByIdAsync(followingId);
            if (followingUser == null)
            {
                return NotFound();
            }

            var follower = new Follower
            {
                FollowerId = authUser.Id,
                FollowableId = followingId,
                CreatedAt = DateTime.UtcNow,
                Accepted = true,
                UpdatedAt = null
            };
            await _dbContext.Followers.AddAsync(follower);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}