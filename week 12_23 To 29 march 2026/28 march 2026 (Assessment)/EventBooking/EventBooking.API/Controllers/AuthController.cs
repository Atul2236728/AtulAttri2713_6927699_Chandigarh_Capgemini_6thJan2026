using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EventBooking.API.Models;
using System.Linq;
using EventBooking.API.Data;
using Microsoft.AspNetCore.Authorization;

namespace EventBooking.API.Controllers
{
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
private readonly AppDbContext _context;


    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    // ✅ REGISTER
    [HttpPost("register")]
    public IActionResult Register([FromBody] LoginModel model)
    {
        var existingUser = _context.Users
            .FirstOrDefault(u => u.Username == model.Username);

        if (existingUser != null)
        {
            return BadRequest(new { message = "User already exists" });
        }

        // 🔐 HASH PASSWORD
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

        var user = new User
        {
            Username = model.Username,
            Password = hashedPassword,
            Role = model.Role ?? "user"
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return Ok(new { message = "User registered successfully" });
    }

    // ✅ LOGIN
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        var user = _context.Users
            .FirstOrDefault(u => u.Username == model.Username && u.Role == model.Role);

        if (user == null)
            return Unauthorized(new { message = "Invalid credentials" });

        bool isValid = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);

        if (!isValid)
            return Unauthorized(new { message = "Invalid credentials" });

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("super_secret_key_12345678901234567890"));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds
        );

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token)
        });
    }

    // 🔥 ADMIN ONLY - GET USERS
    [Authorize(Roles = "admin")]
    [HttpGet("users")]
    public IActionResult GetAllUsers()
    {
        var users = _context.Users
            .Select(u => new
            {
                u.Id,
                u.Username,
                u.Role
            })
            .ToList();

        return Ok(users);
    }
}


}
