using P2PReview.Application.Files;
using P2PReview.Application.ReviewRequests;

namespace P2PReview.Application.Interfaces
{
    public interface IReviewRequestService
    {
        public Task<Guid?> CreateReviewRequestAsync(CreateReviewRequestDto request, ICollection<ReadedCodeFileDto> files);
        public Task<ReviewRequestDto?> GetReviewRequestAsync(string id);
        public Task<ICollection<ReviewRequestDto>?> GetAllReviewRequestAsync();
        public Task<ICollection<ReviewRequestDto>?> GetActiveReviewRequestAsync(int limit);

        public Task<ICollection<ReviewRequestDto>?> GetAuthUserReviewRequestAsync();
        public Task<bool> DeleteReviewRequestAsync(string id);
    }
}