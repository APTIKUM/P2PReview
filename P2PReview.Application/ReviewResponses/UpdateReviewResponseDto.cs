using P2PReview.Domain.Enums;

namespace P2PReview.Application.ReviewResponses
{
    public class UpdateReviewResponseDto
    {
        public ReviewResponseStatus? Status { get; set; }

        public int? CodeRating { get; set; }
        public string? Summary { get; set; }

        public int? ReviewRating { get; set; }
        public string? ReviewFeedback { get; set; }

        public ICollection<ReviewResponseCommentDto>? Comments { get; set; }
    }
}
