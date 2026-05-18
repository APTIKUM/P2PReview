using Microsoft.EntityFrameworkCore;
using P2PReview.Application.Files;
using P2PReview.Application.Interfaces;
using P2PReview.Application.ReviewRequests;
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
                Deadline = request.Deadline.HasValue ? request.Deadline.Value.Date.AddHours(23).AddMinutes(59) : null,
                AllowEducationalUse = request.AllowEducationalUse,
                TechStack = request.TechStack,
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

        public async Task<bool> DeleteReviewRequestAsync(string id)
        {
            var authId = await _userService.GetAuthUserId();

            if (authId == null)
            {
                return false;
            }

            var deletedCount = await _context.ReviewRequests
                .Where(r => r.Id == id && r.UserId == authId)
                .ExecuteDeleteAsync();

            return deletedCount > 0;
        }

        public async Task<ICollection<ReviewRequestDto>?> GetActiveReviewRequestAsync(int limit)
        {
            return await _context.ReviewRequests
                .Where(x => x.Deadline == null || x.Deadline >= DateTime.UtcNow.Date)
                .OrderByDescending(x => x.CreatedAt)
                .Take(limit)
                .Select(x => new ReviewRequestDto(x))
                .ToListAsync();
        }

        public async Task<ICollection<ReviewRequestDto>?> GetAllReviewRequestAsync()
        {
            return await _context.ReviewRequests
                .Select(x => new ReviewRequestDto(x))
                .ToListAsync();
        }

        public async Task<ICollection<ReviewRequestDto>?> GetAuthUserReviewRequestAsync()
        {
            var authId = await _userService.GetAuthUserId();

            if (authId == null)
            {
                return null;
            }

            return await _context.ReviewRequests
                .Where(x => x.UserId == authId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new ReviewRequestDto(x))
                .ToListAsync();
        }

        public async Task<ReviewRequestDto?> GetReviewRequestAsync(string id)
        {

            var authId = await _userService.GetAuthUserId();

            var reviewRequest = await _context.ReviewRequests
                .Include(r => r.Files)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reviewRequest == null)
            {
                return null;
            }

            return new ReviewRequestDto(reviewRequest)
            {
                IsOwn = authId == reviewRequest.UserId,

                Files = [.. reviewRequest.Files.Select(f => new ReadedCodeFileDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Content = f.Content
                })]
            };
        }
    }
}
