using P2PReview.Domain.Entities;

namespace P2PReview.Application.Users.DTOs
{
    public class UserProfileDto
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string AvatarId { get; set; } = null!;
        public int QualityScore { get; set; }
        public bool IsOwnProfile { get; set; } = false;

        public DateTime CreatedAt { get; set; }
        public int ReviewsGiven { get; set; }
        public int ReviewsEasy { get; set; }
        public int ReviewsNormal { get; set; }
        public int ReviewsHard { get; set; }


        public UserProfileDto(User user)
        {
            Id = user.Id;
            UserName = user.UserName;
            AvatarId = user.AvatarId;
            QualityScore = user.QualityScore;
            CreatedAt = user.CreatedAt;
            ReviewsHard = user.ReviewsHard;
            ReviewsEasy = user.ReviewsEasy;
            ReviewsNormal = user.ReviewsNormal;
            ReviewsGiven = user.ReviewsGiven;
        }

        public UserProfileDto()
        {

        }

    }
}
