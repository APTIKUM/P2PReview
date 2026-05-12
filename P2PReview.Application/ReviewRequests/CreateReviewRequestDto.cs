using System.ComponentModel.DataAnnotations;

namespace P2PReview.Application.ReviewRequests
{
    public class CreateReviewRequestDto
    {
        [Required(ErrorMessage = "Название обязательно")]
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Difficulty { get; set; }
        public DateTime? Deadline { get; set; } = null;
        public bool AllowEducationalUse { get; set; } = false;
        public int ReviewersCount { get; set; } = 2;
    }
}
