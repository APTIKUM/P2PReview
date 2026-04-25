using P2PReview.Application.User.DTOs;
using P2PReview.Application.User.Queries;

namespace P2PReview.Application.Interfaces
{
    public interface IUserService
    {
        public Task<UserProfileDto?> GetUserProfileAsync(GetUserProfileQuery query);
    }
}
