using P2PReview.Application.Interfaces;
using P2PReview.Application.Notifications;
using P2PReview.Application.Users;

namespace P2PReview.Web.State;

public class UserContext
{
    private readonly IUserService _userService;
    private readonly INotificationsService _notificationsService;

    public UserProfileDto? Profile { get; private set; }
    public ICollection<NotificationDto> Notifications { get; private set; }

    public event Action? OnChange;

    public UserContext(IUserService userService, INotificationsService notificationsService)
    {
        _userService = userService;
        _notificationsService = notificationsService;
    }

    public async Task InitializeAsync()
    {
        if (Profile != null) return;

        Profile = await _userService.GetAuthProfileAsync();
        Notifications = await _notificationsService.GetAuthNewNotificationsAsync();

        NotifyStateChanged();
    }

    public async Task RefreshAsync()
    {
        Profile = await _userService.GetAuthProfileAsync();
        NotifyStateChanged();
    }

    public void Clear()
    {
        Profile = null;
        NotifyStateChanged();
    }

    private void NotifyStateChanged()
    {
        OnChange?.Invoke();
    }
}