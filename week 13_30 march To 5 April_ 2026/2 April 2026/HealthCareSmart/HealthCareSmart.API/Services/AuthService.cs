using HealthCareSmart.API.Data;
using HealthCareSmart.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthCareSmart.API.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<LoginResult?> LoginAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;

            if (user.PasswordHash != password) return null;

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == user.Id);
            return new LoginResult
            {
                Token = GenerateToken(user),
                UserId = user.Id,
                Role = user.Role,
                PatientId = patient?.Id
            };
        }

        public async Task<RegisterResult> RegisterPatientAsync(string fullName, string email, string password, string phoneNumber, DateTime dateOfBirth, string address)
        {
            if (await _context.Users.AnyAsync(u => u.Email == email))
            {
                return new RegisterResult
                {
                    Success = false,
                    ErrorMessage = "Email is already registered."
                };
            }

            var user = new User
            {
                FullName = fullName,
                Email = email,
                PasswordHash = password,
                Role = "Patient",
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var patient = new Patient
            {
                PhoneNumber = phoneNumber,
                DateOfBirth = dateOfBirth,
                Address = address,
                UserId = user.Id
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return new RegisterResult
            {
                Success = true,
                Token = GenerateToken(user),
                UserId = user.Id,
                PatientId = patient.Id,
                Role = "Patient"
            };
        }

        private string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, user.FullName)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginResult
    {
        public string Token { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int? PatientId { get; set; }
        public string Role { get; set; } = string.Empty;
    }

    public class RegisterResult
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public int UserId { get; set; }
        public int PatientId { get; set; }
        public string Role { get; set; } = string.Empty;
        public string? ErrorMessage { get; set; }
    }
}