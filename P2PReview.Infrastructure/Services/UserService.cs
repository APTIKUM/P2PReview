using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using P2PReview.Application.Interfaces;
using P2PReview.Application.User.DTOs;
using P2PReview.Domain.Entities;
using System.Security.Claims;

namespace P2PReview.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly AuthenticationStateProvider _authStateProvider;

        public UserService(UserManager<User> userManager,
            AuthenticationStateProvider authenticationState)
        {
            _userManager = userManager;
            _authStateProvider = authenticationState;
        }

        public async Task<Guid?> GetAuthUserId()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();

            var user = authState.User;

            var currentUserIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Guid.TryParse(currentUserIdClaim, out var id) ? id : null;
        }

        public async Task<UserProfileDto?> GetUserProfileAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return null;
            }

            var authUserId = await GetAuthUserId();

            return new UserProfileDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                QualityScore = user.QualityScore,
                IsOwnProfile = userId == authUserId
            };
        }
    }
}
