using Microsoft.EntityFrameworkCore;
using P2PReview.Application.Interfaces;
using P2PReview.Application.ReviewRequests;
using P2PReview.Application.ReviewResponses;
using P2PReview.Domain.Entities;

namespace P2PReview.Infrastructure.Services
{
    public class ReviewResponseService : IReviewResponseService
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;
        private readonly IReviewRequestService _reviewRequestService;

        public ReviewResponseService(IUserService userService,
            AppDbContext context,
            IReviewRequestService reviewRequestService)
        {
            _userService = userService;
            _context = context;
            _reviewRequestService = reviewRequestService;
        }

        public async Task<string?> CreateResponseAsync(CreateReviewResponseDto createDto)
        {
            var authId = await _userService.GetAuthUserId();

            if (authId == null)
            {
                return null;
            }

            if (_context.ReviewRequests
                .FirstOrDefault(r => r.Id == createDto.ReviewRequestId) == null)
            {
                return null;
            }

            var reviewResponse = new ReviewResponse()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = authId,
                ReviewRequestId = createDto.ReviewRequestId,
                CreatedAt = DateTime.UtcNow,
            };

            await _context.ReviewResponses.AddAsync(reviewResponse);

            await _context.SaveChangesAsync();

            return reviewResponse.Id;
        }

        public async Task<bool> DeleteResponseAsync(string id)
        {
            var authId = await _userService.GetAuthUserId();

            if (authId == null)
            {
                return false;
            }

            var deletedCount = await _context.ReviewResponses
                .Where(r => r.Id == id && r.UserId == authId)
                .ExecuteDeleteAsync();

            return deletedCount > 0;
        }

        public async Task<ReviewResponseDto?> GetResponseAsync(string id)
        {
            var authId = await _userService.GetAuthUserId();

            var reviewResponse = await _context.ReviewResponses
                .Include(r => r.Comments)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reviewResponse == null)
            {
                return null;
            }


            return new ReviewResponseDto(reviewResponse)
            {
                IsOwn = authId == reviewResponse.UserId
            };

        }

        public async Task<ICollection<ReviewResponseDto>?> GetResponsesByRequestIdAsync(string requestId)
        {
            return await _context.ReviewResponses
                .Where(x => x.ReviewRequestId == requestId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new ReviewResponseDto(x))
                .ToListAsync();
        }

        public async Task<ICollection<ReviewResponseDto>?> GetUserResponsesAsync(string userId)
        {
            return await _context.ReviewResponses
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new ReviewResponseDto(x))
                .ToListAsync();
        }

        public async Task<ReviewResponseDto?> UpdateResponseAsync(string id, UpdateReviewResponseDto updateDto)
        {
            var authId = await _userService.GetAuthUserId();

            if (authId == null)
            {
                return null;
            }

            var reviewResponse = await _context.ReviewResponses
                .Include(x => x.Comments)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (reviewResponse == null)
            {
                return null;
            }

            var reviewRequest = await _reviewRequestService.GetReviewRequestAsync(reviewResponse.ReviewRequestId);

            if (reviewResponse.UserId != authId && reviewRequest?.UserId != authId)
            {
                return null;
            }
            
            // update fields
            reviewResponse.Status = updateDto.Status ?? reviewResponse.Status;

            reviewResponse.CodeRating = updateDto.CodeRating ?? reviewResponse.CodeRating;
            reviewResponse.Summary = updateDto.Summary ?? reviewResponse.Summary;

            reviewResponse.ReviewRating = updateDto.ReviewRating ?? reviewResponse.ReviewRating;
            reviewResponse.ReviewFeedback = updateDto.ReviewFeedback ?? reviewResponse.ReviewFeedback;

            if (updateDto.Comments != null)
            {
                var dtoCommentIds = updateDto.Comments
                    .Where(x => !string.IsNullOrWhiteSpace(x.Id))
                    .Select(x => x.Id)
                    .ToHashSet();

                var commentsToRemove = reviewResponse.Comments
                    .Where(x => !dtoCommentIds.Contains(x.Id))
                    .ToList();

                foreach (var comment in commentsToRemove)
                {
                    reviewResponse.Comments.Remove(comment);
                }

                foreach (var commentDto in updateDto.Comments)
                {
                    var existingComment = reviewResponse.Comments
                        .FirstOrDefault(x => x.Id == commentDto.Id);

                    if (existingComment != null)
                    {
                        existingComment.Line = commentDto.Line;
                        existingComment.Type = commentDto.Type;
                        existingComment.Content = commentDto.Content;
                    }
                    else
                    {
                        reviewResponse.Comments.Add(new ReviewResponseComment
                        {
                            Id = Guid.NewGuid().ToString(),
                            FileId = commentDto.FileId,
                            ReviewResponseId = reviewResponse.Id,
                            Line = commentDto.Line,
                            Type = commentDto.Type,
                            Content = commentDto.Content,
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();

            return new ReviewResponseDto(reviewResponse);
        }
    }
}
