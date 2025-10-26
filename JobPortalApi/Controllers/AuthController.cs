using JobPortalApi.Data;
using JobPortalApi.DTOs;
using JobPortalApi.Models;
using JobPortalApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortalApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AppDbContext db, JwtService jwt) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (await db.Users.AnyAsync(u => u.Email == dto.Email))
            return Conflict("Email already registered.");

        var role = dto.Role.Equals("Recruiter", StringComparison.OrdinalIgnoreCase) ? UserRole.Recruiter : UserRole.User;

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            Role = role,
            PasswordHash = PasswordHasher.Hash(dto.Password)
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();

        return Ok(new { message = "Registered successfully." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await db.Users.SingleOrDefaultAsync(u => u.Email == dto.Email);
        if (user is null) return Unauthorized("Invalid credentials.");

        var hash = PasswordHasher.Hash(dto.Password);
        if (!string.Equals(hash, user.PasswordHash, StringComparison.Ordinal))
            return Unauthorized("Invalid credentials.");

        var token = jwt.CreateToken(user);
        return Ok(new
        {
            token,
            user = new { user.Id, user.FullName, user.Email, role = user.Role.ToString() }
        });
    }
}
