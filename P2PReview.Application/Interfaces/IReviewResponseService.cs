using P2PReview.Application.ReviewResponses;

namespace P2PReview.Application.Interfaces
{
    public interface IReviewResponseService
    {
        public Task<string?> CreateResponseAsync(CreateReviewResponseDto createDto);
        public Task<ReviewResponseDto?> GetResponseAsync(string id);
        public Task<ReviewResponseDto?> UpdateResponseAsync(string id,
            UpdateReviewResponseDto updateDto);

        public Task<bool> DeleteResponseAsync(string id);
        public Task<ICollection<ReviewResponseDto>?> GetResponsesByRequestIdAsync(string requestId);

        public Task<ICollection<ReviewResponseDto>?> GetUserResponsesAsync(string userId);
    }
}
