using HospitalAPI.Data;
using HospitalAPI.DTOs;
using HospitalAPI.Helpers;
using HospitalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly HospitalDbContext _db;
    private readonly JwtHelper _jwt;
    private readonly ILogger<AuthController> _logger;

    public AuthController(HospitalDbContext db, JwtHelper jwt, ILogger<AuthController> logger)
    {
        _db = db;
        _jwt = jwt;
        _logger = logger;
    }

    /// <summary>Login for all roles</summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _db.Users
            .Include(u => u.Doctor)
            .FirstOrDefaultAsync(u => u.Email == dto.Email && u.IsActive);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Unauthorized(new { message = "Invalid email or password." });

        var token = _jwt.GenerateToken(user, user.Doctor?.DoctorId);

        _logger.LogInformation("User {Email} logged in as {Role}", user.Email, user.Role);

        return Ok(new AuthResponseDto
        {
            Token = token,
            Role = user.Role,
            UserId = user.UserId,
            FullName = user.FullName,
            Email = user.Email,
            DoctorId = user.Doctor?.DoctorId
        });
    }

    /// <summary>Patient self-registration</summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
            return BadRequest(new { message = "Email already registered." });

        // Only allow Patient self-registration
        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            Phone = dto.Phone,
            Role = "Patient",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        // Create empty patient profile
        _db.PatientProfiles.Add(new PatientProfile { UserId = user.UserId });
        await _db.SaveChangesAsync();

        var token = _jwt.GenerateToken(user);

        return Ok(new AuthResponseDto
        {
            Token = token,
            Role = user.Role,
            UserId = user.UserId,
            FullName = user.FullName,
            Email = user.Email
        });
    }
}
