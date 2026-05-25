using Microsoft.EntityFrameworkCore;
using P2PReview.Application.Interfaces;
using P2PReview.Application.Notifications;
using P2PReview.Domain.Entities;

namespace P2PReview.Infrastructure.Services
{
    public class NotificationsService : INotificationsService
    {

        private readonly AppDbContext _context;
        private readonly IUserService _userService;

        public NotificationsService(IUserService userService, AppDbContext context)
        {
            _userService = userService;
            _context = context;
        }


        public async Task<bool> CreateNotificationAsync(CreateNotificationDto createDto)
        {
            var authId = await _userService.GetAuthUserId();

            if (authId == null)
            {
                return false;
            }

            var notification = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UserId = createDto.UserId,
                Title = createDto.Title,
                Url = createDto.Url,
                IsNew = true
            };

            await _context.Notifications.AddAsync(notification);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ICollection<NotificationDto>?> GetAuthNewNotificationsAsync()
        {
            var authId = await _userService.GetAuthUserId();

            if (authId == null)
            {
                return null;
            }

            return await _context.Notifications
                .Where(x => x.IsNew && x.UserId == authId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new NotificationDto(x))
                .ToListAsync();
        }

        public async Task<bool> ViewNotificationAsync(string id)
        {
            var authId = await _userService.GetAuthUserId();

            if (authId == null)
            {
                return false;
            }

            var notification = await _context.Notifications
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == authId);

            if (notification == null)
            {
                return false;
            }

            notification.IsNew = false;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
