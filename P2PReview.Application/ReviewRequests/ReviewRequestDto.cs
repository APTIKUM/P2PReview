using P2PReview.Application.Files;

namespace P2PReview.Application.ReviewRequests
{
    public class ReviewRequestDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Difficulty { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? Deadline { get; set; }
        public string? TechStack { get; set; }
        public int ReviewersCount { get; set; }
        public bool IsOwnReview { get; set; } = false;

        public ICollection<ReadedCodeFileDto>? Files { get; set; }
    }
}
