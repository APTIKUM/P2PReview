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
        private readonly INotificationsService _notificationsService;

        public ReviewRequestService(IUserService userService, AppDbContext context, INotificationsService notificationsService)
        {
            _userService = userService;
            _context = context;
            _notificationsService = notificationsService;
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
                ReviewersCount = request.ReviewersCount,
                Tags = request.Tags.ToArray()
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

            if (request.IsAutoAssignReviewer)
            {

                var reviewResponse = await AssignReviewer(reviewRequest);

                if (reviewResponse == null)
                {
                    await _notificationsService.CreateNotificationAsync(new()
                    {
                        Title = "Ревьюер для вашего запроса не найдет!",
                        UserId = authId,
                        Url = $"review/request/{reviewRequest.Id}"
                    });
                }
                else
                {
                    await _notificationsService.CreateNotificationAsync(new()
                    {
                        Title = "Ревьюер назначен!",
                        UserId = authId,
                        Url = $"review/request/{reviewRequest.Id}"
                    });

                    await _notificationsService.CreateNotificationAsync(new()
                    {
                        Title = "На вас назначено новое ревью!",
                        UserId = reviewResponse.UserId,
                        Url = $"review/response/{reviewResponse.Id}"
                    });
                }

            }

            return Guid.Parse(reviewRequest.Id);
        }


        private async Task<ReviewResponse?> AssignReviewer(ReviewRequest reviewRequest)
        {
            var tagSet = reviewRequest.Tags;

            var users = await _context.Users
                .Where(u => u.IsAutoGetReview
                    && u.Id != reviewRequest.UserId)
                .ToListAsync();

            var candidate = users
                .Where(u => u.Tags != null && u.Tags.Length > 0)
                .Select(u => new
                {
                    User = u,
                    MatchCount = u.Tags.Count(t => tagSet.Contains(t))
                })
                .OrderByDescending(x => x.MatchCount)
                .FirstOrDefault();

            if (candidate == null || candidate.MatchCount == 0)
            {
                return null;
            }

            var reviewResponse = new ReviewResponse
            {
                Id = Guid.NewGuid().ToString(),
                UserId = candidate.User.Id,
                ReviewRequestId = reviewRequest.Id,
                CreatedAt = DateTime.UtcNow,
            };

            await _context.ReviewResponses.AddAsync(reviewResponse);
            await _context.SaveChangesAsync();

            return reviewResponse;
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

            return await GetUserReviewRequestAsync(authId);
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

        public async Task<ICollection<ReviewRequestDto>?> GetUserReviewRequestAsync(string userId)
        {
            return await _context.ReviewRequests
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new ReviewRequestDto(x))
                .ToListAsync();
        }
    }
}
