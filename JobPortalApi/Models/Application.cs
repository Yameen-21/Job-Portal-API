namespace JobPortalApi.Models;

public enum ApplicationStatus { Submitted, Reviewed, Accepted, Rejected }

public class Application
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public Job Job { get; set; } = default!;

    public int UserId { get; set; }
    public User User { get; set; } = default!;

    public string CoverLetter { get; set; } = "";
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Submitted;
}
