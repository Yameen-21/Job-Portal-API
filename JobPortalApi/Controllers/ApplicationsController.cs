using JobPortalApi.Data;
using JobPortalApi.DTOs;
using JobPortalApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JobPortalApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicationsController(AppDbContext db) : ControllerBase
{
    [Authorize(Roles = "User")]
    [HttpPost]
    public async Task<IActionResult> Apply(ApplyJobDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var jobExists = await db.Jobs.AnyAsync(j => j.Id == dto.JobId);
        if (!jobExists) return NotFound("Job not found.");

        var already = await db.Applications.AnyAsync(a => a.JobId == dto.JobId && a.UserId == userId);
        if (already) return Conflict("You already applied to this job.");

        var appEntity = new Application
        {
            JobId = dto.JobId,
            UserId = userId,
            CoverLetter = dto.CoverLetter
        };
        db.Applications.Add(appEntity);
        await db.SaveChangesAsync();

        return Ok(new { message = "Application submitted.", appEntity.Id });
    }

    [Authorize(Roles = "User")]
    [HttpGet("my")]
    public async Task<IActionResult> MyApplications()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var list = await db.Applications
            .Include(a => a.Job)
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.AppliedAt)
            .Select(a => new
            {
                a.Id,
                a.Status,
                a.AppliedAt,
                job = new { a.Job.Id, a.Job.Title, a.Job.Company, a.Job.Location }
            })
            .ToListAsync();

        return Ok(list);
    }

    [Authorize(Roles = "Recruiter")]
    [HttpGet("job/{jobId:int}")]
    public async Task<IActionResult> ApplicationsForJob(int jobId)
    {
        var recruiterId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var owns = await db.Jobs.AnyAsync(j => j.Id == jobId && j.RecruiterId == recruiterId);
        if (!owns) return Forbid();

        var list = await db.Applications
            .Include(a => a.User)
            .Where(a => a.JobId == jobId)
            .OrderByDescending(a => a.AppliedAt)
            .Select(a => new
            {
                a.Id,
                a.Status,
                a.AppliedAt,
                user = new { a.UserId, a.User.FullName, a.User.Email }
            })
            .ToListAsync();

        return Ok(list);
    }

    [Authorize(Roles = "Recruiter")]
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromQuery] ApplicationStatus status)
    {
        var appData = await db.Applications.Include(a => a.Job).FirstOrDefaultAsync(a => a.Id == id);
        if (appData is null) return NotFound();

        var recruiterId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (appData.Job.RecruiterId != recruiterId) return Forbid();

        appData.Status = status;
        await db.SaveChangesAsync();
        return Ok(new { message = "Status updated." });
    }
}
