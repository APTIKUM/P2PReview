namespace P2PReview.Application.User.DTOs
{
    public class UserProfileDto
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string AvatarUrl { get; set; } = null!;
        public int QualityScore { get; set; }
        public bool IsOwnProfile { get; set; } 

        public DateTime CreatedAt { get; set; }
        public int ReviewsGiven { get; set; }
        public int ReviewsEasy { get; set; }
        public int ReviewsNormal { get; set; }
        public int ReviewsHard { get; set; }

    }
}
