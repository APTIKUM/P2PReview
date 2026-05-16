using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using P2PReview.Application.Auth.Commands;
using P2PReview.Application.Files;
using P2PReview.Application.Interfaces;
using P2PReview.Application.ReviewRequests;
using P2PReview.Application.Users;
using P2PReview.Domain.Entities;

namespace P2PReview.Infrastructure.Services
{
    public class ReviewRequestService : IReviewRequestService
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;

        public ReviewRequestService(IUserService userService, AppDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        public async Task<Guid?> CreateReviewRequestAsync(CreateReviewRequestDto request,
            ICollection<ReadedCodeFileDto> files)
        {
            var authId = await _userService.GetAuthUserId();

            if (authId == null)
            {
                return null;
            }

            var reviewRequest = new ReviewRequest
            {
                Id = Guid.NewGuid().ToString(),
                UserId = authId.ToString(),
                Name = request.Name,
                Description = request.Description,
                Difficulty = request.Difficulty,
                CreatedAt = DateTime.UtcNow,
                Deadline = request.Deadline,
                AllowEducationalUse = request.AllowEducationalUse,
                TechStack = null, //TODO
                ReviewersCount = request.ReviewersCount
            };

            await _context.ReviewRequests.AddAsync(reviewRequest);


            var reviewRequestFiles = files.Select(file => new ReviewRequestFile
            {
                Id = Guid.NewGuid().ToString(),
                Name = file.Name,
                Content = file.Content,
                ReviewRequestId = reviewRequest.Id
            }).ToList();

            await _context.ReviewRequestFiles.AddRangeAsync(reviewRequestFiles);


            await _context.SaveChangesAsync();

            return Guid.Parse(reviewRequest.Id);
        }



        public async Task<ReviewRequestDto?> GetReviewRequestAsync(string id)
        {

            var authId = await _userService.GetAuthUserId();

            if (authId == null)
            {
                return null;
            }


            var reviewRequest = await _context.ReviewRequests
                .Include(r => r.Files)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reviewRequest == null)
            {
                return null;
            }

            return new ReviewRequestDto
            {
                Id = reviewRequest.Id,
                Name = reviewRequest.Name,
                Description = reviewRequest.Description,
                Difficulty = reviewRequest.Difficulty,
                CreatedAt = reviewRequest.CreatedAt,
                Deadline = reviewRequest.Deadline,
                ReviewersCount = reviewRequest.ReviewersCount,

                IsOwnReview = authId.ToString() == reviewRequest.UserId,

                Files = reviewRequest.Files.Select(f => new ReadedCodeFileDto
                {
                    Name = f.Name,
                    Content = f.Content
                }).ToList()
            };
        }
    }
}
