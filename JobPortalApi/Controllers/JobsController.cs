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
public class JobsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(string? q = null)
    {
        var query = db.Jobs
            .Include(j => j.Recruiter)
            .OrderByDescending(j => j.PostedAt)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            query = query.Where(j => j.Title.Contains(q) || j.Company.Contains(q) || j.Location.Contains(q));
        }

        var data = await query.Select(j => new
        {
            j.Id,
            j.Title,
            j.Company,
            j.Location,
            j.SalaryMin,
            j.SalaryMax,
            j.PostedAt,
            recruiter = new { j.RecruiterId, j.Recruiter.FullName }
        }).ToListAsync();

        return Ok(data);
    }

    [Authorize(Roles = "Recruiter")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateJobDto dto)
    {
        var recruiterId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var job = new Job
        {
            Title = dto.Title,
            Company = dto.Company,
            Location = dto.Location,
            Description = dto.Description,
            SalaryMin = dto.SalaryMin,
            SalaryMax = dto.SalaryMax,
            RecruiterId = recruiterId
        };
        db.Jobs.Add(job);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = job.Id }, job);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var job = await db.Jobs.Include(j => j.Recruiter).FirstOrDefaultAsync(j => j.Id == id);
        if (job is null) return NotFound();
        return Ok(job);
    }

    [Authorize(Roles = "Recruiter")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CreateJobDto dto)
    {
        var job = await db.Jobs.FindAsync(id);
        if (job is null) return NotFound();

        var recruiterId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (job.RecruiterId != recruiterId) return Forbid();

        job.Title = dto.Title;
        job.Company = dto.Company;
        job.Location = dto.Location;
        job.Description = dto.Description;
        job.SalaryMin = dto.SalaryMin;
        job.SalaryMax = dto.SalaryMax;

        await db.SaveChangesAsync();
        return NoContent();
    }

    [Authorize(Roles = "Recruiter")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var job = await db.Jobs.FindAsync(id);
        if (job is null) return NotFound();

        var recruiterId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (job.RecruiterId != recruiterId) return Forbid();

        db.Jobs.Remove(job);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
