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
        }
    }
}
