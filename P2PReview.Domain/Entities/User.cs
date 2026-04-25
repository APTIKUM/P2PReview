using Microsoft.AspNetCore.Identity;

namespace P2PReview.Domain.Entities
{
    public class User : IdentityUser
    {
        public int QualityScore { get; set; }
    }
}
