namespace JobPortalApi.Models;

public enum UserRole { User = 0, Recruiter = 1 }

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public UserRole Role { get; set; } = UserRole.User;

    public ICollection<Job>? Jobs { get; set; }           // Recruiter’s jobs
    public ICollection<Application>? Applications { get; set; } // User’s applications
}
