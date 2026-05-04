namespace P2PReview.Application.Users.DTOs
{
    public class UpdateUserProfileDto
    {
        public string? UserName { get; set; } = null;
        public string? AvatarId { get; set; } = null;
        public string? GitHubUrl { get; set; } = null;
        public string? GitLabUrl { get; set; } = null;
    }
}
