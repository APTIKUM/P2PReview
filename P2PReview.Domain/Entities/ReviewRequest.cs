namespace P2PReview.Domain.Entities
{
    public class ReviewRequest
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Difficulty { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? Deadline { get; set; }
        public bool AllowEducationalUse { get; set; } = false;
        public string? TechStack { get; set; }
        public int ReviewersCount { get; set; }

        // Files
        public ICollection<ReviewRequestFile> Files { get; set; } = new List<ReviewRequestFile>();
        public ICollection<ReviewResponse> ReviewResponses { get; set; } = new List<ReviewResponse>();
    }
}
