using P2PReview.Application.Files;
using P2PReview.Domain.Entities;

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

        // да-да, надо вынести в отедльную dto для карточек , но мне лень, отвалите 🙄
        public string DeadlineLabel
        {
            get
            {
                if (Deadline == null)
                    return "Без дедлайна";

                var diff = Deadline.Value - DateTime.UtcNow;

                if (diff.TotalSeconds <= 0)
                    return "Просрочено";

                if (diff.TotalDays >= 30)
                    return $"{(int)(diff.TotalDays / 30)} мес.";

                if (diff.TotalDays >= 7)
                    return $"{(int)(diff.TotalDays / 7)} нед.";

                if (diff.TotalDays >= 1)
                    return $"{(int)diff.TotalDays} дн.";

                if (diff.TotalHours >= 1)
                    return $"{(int)diff.TotalHours} ч.";

                return $"{Math.Max(1, (int)diff.TotalMinutes)} мин.";
            }
        }

        public ICollection<ReadedCodeFileDto>? Files { get; set; }

        public ReviewRequestDto(ReviewRequest request)
        {
            Id = request.Id;
            UserId = request.UserId;
            Name = request.Name;
            Description = request.Description;
            Difficulty = request.Difficulty;
            CreatedAt = request.CreatedAt;
            Deadline = request.Deadline;
            TechStack = request.TechStack;
            ReviewersCount = request.ReviewersCount;
        }

        public ReviewRequestDto() 
        {
        }
    }
}
