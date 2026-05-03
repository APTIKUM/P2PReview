using P2PReview.Application.Users.DTOs;

namespace P2PReview.Application.Interfaces
{
    public interface IUserService
    {
        public Task<UserProfileDto?> GetUserProfileAsync(Guid userId);
        public Task<UserProfileDto?> GetAuthProfileAsync();
        public Task<Guid?> GetAuthUserId();

        public Task<UserProfileDto> UpdateUserProfileAsync(Guid userId, UpdateUserProfileDto updateUserProfileDto);
    }
}
