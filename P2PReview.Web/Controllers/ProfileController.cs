using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using P2PReview.Application.Interfaces;
using P2PReview.Application.User.Queries;
using P2PReview.Domain.Entities;
using System.Security.Claims;

namespace P2PReview.Web.Controllers
{

    [ApiController]
    [Route("api/profile")]
    public class ProfileController : ControllerBase
    {
        private readonly IUserService _userService;

        public ProfileController(
            IUserService userService)
        {
            _userService = userService;
        }

        private Guid? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (Guid.TryParse(userIdClaim, out var userId))
                return userId;

            return null;
        }

        //[Authorize]
        [HttpGet("{userId:Guid}")]
        public async Task<IActionResult> GetProfile(Guid userId)
        {
            var currentUserId = GetCurrentUserId();

            var query = new GetUserProfileQuery(userId, currentUserId);

            var result = await _userService.GetUserProfileAsync(query);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);            
        }

        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> GetMyProfile()
        {
            var currentUserId = GetCurrentUserId();

            return await GetProfile((Guid)currentUserId);
        }
    }
}
