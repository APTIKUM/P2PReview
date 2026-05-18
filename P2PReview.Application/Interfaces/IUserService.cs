using P2PReview.Application.Users;

namespace P2PReview.Application.Interfaces
{
    public interface IUserService
    {
        public Task<UserProfileDto?> GetUserProfileAsync(string userId);
        public Task<UserProfileDto?> GetAuthProfileAsync();
        public Task<string?> GetAuthUserId();

        public Task<UserProfileDto> UpdateUserProfileAsync(string userId, UpdateUserProfileDto updateUserProfileDto);

        public Task<(IList<UserProfileDto> leaders, int authPlace)> GetLeaderBoardAsync();
    }
}
