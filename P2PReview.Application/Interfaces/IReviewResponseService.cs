using P2PReview.Application.ReviewResponses;

namespace P2PReview.Application.Interfaces
{
    public interface IReviewResponseService
    {
        public Task<string?> CreateReviewResponseAsync(CreateReviewResponseDto createDto);
        public Task<ReviewResponseDto?> GetReviewResponseAsync(string id);
        public Task<ReviewResponseDto?> UpdateReviewResponseAsync(string id,
            UpdateReviewResponseDto updateDto);

        public Task<bool> DeleteReviewResponseAsync(string id);
    }
}
