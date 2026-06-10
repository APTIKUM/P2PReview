namespace P2PReview.Application.ReviewRequests
{
    public class CreateReviewRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Difficulty { get; set; }
        public DateTime? Deadline { get; set; } = null;
        public bool AllowEducationalUse { get; set; } = false;
        public int ReviewersCount { get; set; } = 2;
        public string? TechStack { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public bool IsAutoAssignReviewer { get; set; }
    }
}
