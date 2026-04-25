using Microsoft.AspNetCore.Identity;
using P2PReview.Application.Interfaces;
using P2PReview.Application.User.DTOs;
using P2PReview.Application.User.Queries;
using P2PReview.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2PReview.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserProfileDto?> GetUserProfileAsync(GetUserProfileQuery query)
        {
            var user = await _userManager.FindByIdAsync(query.UserId.ToString());

            if (user == null)
            {
                return null;
            }

            return new UserProfileDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                QualityScore = user.QualityScore,
                IsOwnProfile = query.UserId == query.UserId
            };
        }
    }
}
