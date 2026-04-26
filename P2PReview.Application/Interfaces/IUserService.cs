using P2PReview.Application.User.DTOs;

namespace P2PReview.Application.Interfaces
{
    public interface IUserService
    {
        public Task<UserProfileDto?> GetUserProfileAsync(Guid userId);

        public Task<Guid?> GetAuthUserId();
    }
}
