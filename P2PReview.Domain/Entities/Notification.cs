namespace P2PReview.Domain.Entities
{
    public class Notification
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string Title { get; set; }

        public string? Url { get; set; }

        public bool IsNew { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
