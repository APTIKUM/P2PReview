using P2PReview.Domain.Enums;

namespace P2PReview.Domain.Entities
{
    public class ReviewResponse
    {
        public string Id { get; set; }
        
        public string UserId { get; set; }
        public User User { get; set; }

        public string ReviewRequestId { get; set; }
        public ReviewRequest ReviewRequest { get; set; }

        public ReviewResponseStatus Status { get; set; }

        public int? CodeRating { get; set; }
        public string? Summary { get; set; }

        public int? ReviewRating { get; set; }
        public string? ReviewFeedback { get; set; }

        public ICollection<ReviewResponseComment> Comments { get; set; } = new List<ReviewResponseComment>();
    }
}
