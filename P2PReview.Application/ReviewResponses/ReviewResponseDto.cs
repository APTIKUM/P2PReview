using P2PReview.Domain.Entities;
using P2PReview.Domain.Enums;

namespace P2PReview.Application.ReviewResponses
{
    public class ReviewResponseDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }

        public string ReviewRequestId { get; set; }
        public ReviewRequest ReviewRequest { get; set; }

        public ReviewResponseStatus Status { get; set; }

        public int? CodeRating { get; set; }
        public string? Summary { get; set; }

        public int? ReviewRating { get; set; }
        public string? ReviewFeedback { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool IsOwn { get; set; }

        public List<ReviewResponseCommentDto> Comments { get; set; } = new List<ReviewResponseCommentDto>();

        public ReviewResponseDto()
        {

        }

        public ReviewResponseDto(ReviewResponse request)
        {
            Id = request.Id;
            UserId = request.UserId;
            ReviewRequestId = request.ReviewRequestId;
            Status = request.Status;
            CodeRating = request.CodeRating;
            Summary = request.Summary;
            ReviewRating = request.ReviewRating;
            ReviewFeedback = request.ReviewFeedback;
            CreatedAt = request.CreatedAt;

            Comments = request.Comments.Select(x => new ReviewResponseCommentDto(x))
                .ToList();
        }
    }
}
