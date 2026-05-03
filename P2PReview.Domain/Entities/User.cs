using Microsoft.AspNetCore.Identity;

namespace P2PReview.Domain.Entities
{
    public class User : IdentityUser
    {
        public int QualityScore { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ReviewsGiven {  get; set; }
        public int ReviewsEasy {  get; set; }
        public int ReviewsNormal {  get; set; }
        public int ReviewsHard {  get; set; }

        public string? AvatarId { get; set; }

    }
}
