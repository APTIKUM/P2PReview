using P2PReview.Application.Notifications;

namespace P2PReview.Application.Interfaces
{
    public interface INotificationsService
    {
        public Task<ICollection<NotificationDto>?> GetAuthNewNotificationsAsync();

        public Task<bool> ViewNotificationAsync(string id);
        public Task<bool> CreateNotificationAsync(CreateNotificationDto createDto);

    }
}
