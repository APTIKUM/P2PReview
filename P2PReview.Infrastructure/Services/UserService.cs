using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using P2PReview.Application.Interfaces;
using P2PReview.Application.Users;
using P2PReview.Domain.Entities;
using System.Security.Claims;

namespace P2PReview.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly AuthenticationStateProvider _authStateProvider;

        public UserService(UserManager<User> userManager,
            AuthenticationStateProvider authenticationState,
            AppDbContext context)
        {
            _userManager = userManager;
            _authStateProvider = authenticationState;
            _context = context;
        }

        public async Task<UserProfileDto?> GetAuthProfileAsync()
        {
            var authId = await GetAuthUserId();

            if (authId == null)
            {
                return null;
            }

            return await GetUserProfileAsync(authId);
        }

        public async Task<string?> GetAuthUserId()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();

            var user = authState.User;

            var currentUserIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return currentUserIdClaim;
        }

        public async Task<(IList<UserProfileDto> leaders, int authPlace)> GetLeaderBoardAsync()
        {
            var leader = await _context.Users
                .OrderByDescending(u => u.QualityScore)
                .ThenBy(u => u.Id)
                .Take(5)
                .Select(u => new UserProfileDto(u))
                .ToListAsync();


            var authUser = await GetAuthProfileAsync();

            if (authUser == null)
            {
                return (leader, 0);
            }

            var authPlace = await _context.Users.CountAsync(u =>
                    u.QualityScore > authUser.QualityScore 
                    || (u.QualityScore == authUser.QualityScore && u.Id.CompareTo(authUser.Id) < 0)) + 1;



            return (leader, authPlace);
        }

        public async Task<UserProfileDto?> GetUserProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return null;
            }

            var authUserId = await GetAuthUserId();

            return new UserProfileDto(user)
            {
                IsOwnProfile = userId == authUserId
            };
        }

        public async Task<UserProfileDto> UpdateUserProfileAsync(string userId, UpdateUserProfileDto updateUserProfileDto)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.UserName = updateUserProfileDto.UserName ?? user.UserName;
            user.AvatarId = updateUserProfileDto.AvatarId ?? user.AvatarId;
            user.GitHubUrl = updateUserProfileDto.GitHubUrl ?? user.GitHubUrl;
            user.GitLabUrl = updateUserProfileDto.GitLabUrl ?? user.GitLabUrl;

            await _userManager.UpdateAsync(user);

            return new UserProfileDto(user);
        }

        public async Task<bool> UpdateUserStatsAsync(string userId, int? reviewGrade, int requestDifficulty, bool isAccepted)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            var score = CalculateQualityScoreDelta(reviewGrade, requestDifficulty, isAccepted);

            user.QualityScore = Math.Clamp(user.QualityScore + score, 0 , int.MaxValue);

            if (isAccepted) 
            {
                if (requestDifficulty == 0)
                {
                    user.ReviewsEasy++;
                }
                else if (requestDifficulty == 1)
                {
                    user.ReviewsNormal++;
                }
                else if (requestDifficulty == 2)
                {
                    user.ReviewsHard++;
                }
            }

            await _userManager.UpdateAsync(user); 

            return true;
        }

        private int CalculateQualityScoreDelta(int? reviewGrade, int requestDifficulty, bool isAccepted)
        {
            var baseScore = requestDifficulty switch
            {
                0 => 10,
                1 => 20,
                2 => 30,
                _ => 10
            };

            var multiplier = reviewGrade switch
            {
                1 => 0.5,
                2 => 0.7,
                3 => 1.0,
                4 => 1.2,
                5 => 1.5,
                _ => 0.5
            };

            var score = (int)Math.Round(baseScore * multiplier);

            if (!isAccepted)
            {
                var penaltyMultiplier = reviewGrade switch
                {
                    5 => 0.0,
                    4 => 0.0,
                    3 => 0.3,
                    2 => 0.6,
                    1 => 1.0,
                    _ => 0.0
                };

                score = -(int)Math.Round(baseScore * penaltyMultiplier);
            }

            return score;
        }
    }
}
