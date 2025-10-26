using JobPortalApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace JobPortalApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<Application> Applications => Set<Application>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        b.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        b.Entity<Job>()
            .Property(j => j.SalaryMin)
            .HasColumnType("decimal(18,2)");

        b.Entity<Job>()
            .Property(j => j.SalaryMax)
            .HasColumnType("decimal(18,2)");

        b.Entity<Application>()
            .HasIndex(a => new { a.JobId, a.UserId })
            .IsUnique();

        // Seed: one recruiter and one user
        var recruiter = new User
        {
            Id = 1,
            FullName = "Recruiter Admin",
            Email = "recruiter@example.com",
            Role = UserRole.Recruiter,
            PasswordHash = Sha256("recruiter123")
        };
        var user = new User
        {
            Id = 2,
            FullName = "Demo User",
            Email = "user@example.com",
            Role = UserRole.User,
            PasswordHash = Sha256("user123")
        };
        b.Entity<User>().HasData(recruiter, user);

        // Seed a couple of jobs
        b.Entity<Job>().HasData(
            new Job
            {
                Id = 1,
                Title = "Junior .NET Developer",
                Company = "TechVerse",
                Location = "Karachi, PK",
                Description = "Build REST APIs with ASP.NET Core, EF Core, SQL Server.",
                SalaryMin = 60000,
                SalaryMax = 120000,
                PostedAt = DateTime.UtcNow,
                RecruiterId = 1
            },
            new Job
            {
                Id = 2,
                Title = "Frontend Developer (React)",
                Company = "CloudEdge",
                Location = "Remote",
                Description = "SPA development, API integration, unit testing.",
                SalaryMin = 70000,
                SalaryMax = 140000,
                PostedAt = DateTime.UtcNow,
                RecruiterId = 1
            }
        );

        static string Sha256(string input)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }
    }
}
