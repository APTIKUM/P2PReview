using P2PReview.Application.Files;
using P2PReview.Application.ReviewRequests;

namespace P2PReview.Application.Interfaces
{
    public interface IReviewRequestService
    {
        public Task<Guid?> CreateReviewRequestAsync(CreateReviewRequestDto request, ICollection<ReadedCodeFileDto> files);
        public Task<ReviewRequestDto?> GetReviewRequestAsync(string id);
    }
}