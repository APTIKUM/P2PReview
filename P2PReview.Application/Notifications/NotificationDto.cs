using P2PReview.Domain.Entities;

namespace P2PReview.Application.Notifications
{
    public class NotificationDto
    {
        public string Id {  get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public bool IsNew { get; set; }
        public DateTime CreatedAt { get; set; }

        public NotificationDto()
        {

        }

        public NotificationDto(Notification notification)
        {
            Id = notification.Id;
            Title = notification.Title;
            Url = notification.Url;
            IsNew = notification.IsNew;
            CreatedAt = notification.CreatedAt;
        }
    }
}
