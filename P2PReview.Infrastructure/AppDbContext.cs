using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using P2PReview.Domain.Entities;

namespace P2PReview.Infrastructure
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<ReviewRequest> ReviewRequests { get; set; }
        public DbSet<ReviewRequestFile> ReviewRequestFiles { get; set; }
        public DbSet<ReviewResponse> ReviewResponses { get; set; }
        public DbSet<ReviewResponseComment> ReviewResponseComments { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ReviewRequest>()
                .HasOne(x => x.User)
                .WithMany(x => x.ReviewRequests)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ReviewRequestFile>()
                .HasOne(x => x.ReviewRequest)
                .WithMany(x => x.Files)
                .HasForeignKey(x => x.ReviewRequestId);

            builder.Entity<ReviewResponse>()
                .HasOne(x => x.User)
                .WithMany(u => u.ReviewResponses)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ReviewResponse>()
                .HasOne(x => x.ReviewRequest)
                .WithMany(r => r.ReviewResponses)
                .HasForeignKey(x => x.ReviewRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ReviewResponseComment>()
                .HasOne(x => x.ReviewResponse)
                .WithMany(r => r.Comments)
                .HasForeignKey(x => x.ReviewResponseId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
