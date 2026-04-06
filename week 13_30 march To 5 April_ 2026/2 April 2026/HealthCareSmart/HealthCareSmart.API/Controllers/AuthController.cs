using HealthCareSmart.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareSmart.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation("Login attempt for: {Email} as {Role}", request.Email, request.Role);

            var result = await _authService.LoginAsync(request.Email, request.Password);
            if (result == null)
            {
                _logger.LogWarning("Failed login for: {Email}", request.Email);
                return Unauthorized(new { message = "Invalid email or password" });
            }

            if (!string.Equals(result.Role, request.Role, StringComparison.OrdinalIgnoreCase))
            {
                var mismatchMessage = request.Role switch
                {
                    "Doctor" => "You are not doctor",
                    "Admin" => "You are not admin",
                    "Patient" => "You are not patient",
                    _ => "Invalid role selected"
                };
                _logger.LogWarning("Role mismatch for {Email}: selected {SelectedRole}, actual {ActualRole}", request.Email, request.Role, result.Role);
                return BadRequest(new { message = mismatchMessage });
            }

            return Ok(new
            {
                token = result.Token,
                userId = result.UserId,
                role = result.Role,
                patientId = result.PatientId ?? 0
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            _logger.LogInformation("Registration attempt for: {Email}", request.Email);

            var result = await _authService.RegisterPatientAsync(
                request.FullName,
                request.Email,
                request.Password,
                request.PhoneNumber,
                request.DateOfBirth,
                request.Address);

            if (!result.Success)
            {
                _logger.LogWarning("Failed registration for: {Email}. Reason: {Reason}", request.Email, result.ErrorMessage);
                return Conflict(new { message = result.ErrorMessage });
            }

            return Ok(new
            {
                token = result.Token,
                userId = result.UserId,
                role = result.Role,
                patientId = result.PatientId
            });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "Patient";
    }

    public class RegisterRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; } = string.Empty;
    }
}