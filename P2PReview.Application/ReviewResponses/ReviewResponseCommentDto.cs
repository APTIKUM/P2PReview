using P2PReview.Domain.Entities;
using P2PReview.Domain.Enums;

namespace P2PReview.Application.ReviewResponses
{
    public class ReviewResponseCommentDto
    {
        public string Id { get; set; }
        public string ReviewResponseId { get; set; }
        public string FileId { get; set; }
        public ReviewResponseCommentType Type { get; set; }
        public int Line { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public ReviewResponseCommentDto()
        {

        }

        public ReviewResponseCommentDto(ReviewResponseComment comment)
        {
            Id = comment.Id;
            FileId = comment.FileId;
            Line = comment.Line;
            Type = comment.Type;
            Content = comment.Content;
            CreatedAt = comment.CreatedAt;
            ReviewResponseId = comment.ReviewResponseId;
        }
    }
}
