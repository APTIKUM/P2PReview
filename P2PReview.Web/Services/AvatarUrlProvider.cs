namespace P2PReview.Web.Services
{
    public class AvatarUrlProvider
    {
        public const string DefaultAvatar = "/uploads/avatars/default-avatar.png";
        public string GetAvatarUrl(string? avatarId)
        {
            if (string.IsNullOrWhiteSpace(avatarId))
            {
                return DefaultAvatar;
            }

            return $"/uploads/avatars/{avatarId}";
        }
    }
}
