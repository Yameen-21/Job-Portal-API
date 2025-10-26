using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobPortalApi.Data;

namespace JobPortalApi.Controllers;

public class WebController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        var jobs = await db.Jobs
            .Include(j => j.Recruiter)
            .OrderByDescending(j => j.PostedAt)
            .ToListAsync();
        return View(jobs);
    }

    public async Task<IActionResult> Details(int id)
    {
        var job = await db.Jobs.Include(j => j.Recruiter).FirstOrDefaultAsync(j => j.Id == id);
        if (job == null) return NotFound();
        return View(job);
    }
}
