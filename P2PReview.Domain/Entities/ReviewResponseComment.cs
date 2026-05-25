using P2PReview.Domain.Enums;

namespace P2PReview.Domain.Entities
{
    public class ReviewResponseComment
    {
        public string Id { get; set; }
        public string ReviewResponseId { get; set; }
        public string FileId { get; set; }
        public ReviewResponse ReviewResponse { get; set; }
        public ReviewResponseCommentType Type { get; set; }
        public string Content { get; set; }
        public int Line { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
