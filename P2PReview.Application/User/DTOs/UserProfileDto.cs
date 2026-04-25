using System;
using System.Collections.Generic;
using System.Text;

namespace P2PReview.Application.User.DTOs
{
    public class UserProfileDto
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string AvatarUrl { get; set; } = null!;

        public int QualityScore { get; set; }

        public bool IsOwnProfile { get; set; }
    }
}
