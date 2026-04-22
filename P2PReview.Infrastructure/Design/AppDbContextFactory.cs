using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using P2PReview.Domain;

namespace P2PReview.Infrastructure.Design;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseSqlite("Data Source=app.db");

        return new AppDbContext(optionsBuilder.Options);
    }
}