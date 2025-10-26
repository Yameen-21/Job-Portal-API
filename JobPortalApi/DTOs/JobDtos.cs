namespace JobPortalApi.DTOs;

public class CreateJobDto
{
    public string Title { get; set; } = default!;
    public string Company { get; set; } = default!;
    public string Location { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal SalaryMin { get; set; }
    public decimal SalaryMax { get; set; }
}

public class ApplyJobDto
{
    public int JobId { get; set; }
    public string CoverLetter { get; set; } = "";
}
