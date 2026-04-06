using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace HealthCareSmart.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IHttpClientFactory httpClientFactory,
                                 ILogger<AccountController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, string role = "Patient")
        {
            role = string.IsNullOrWhiteSpace(role) ? "Patient" : role;
            var client = _httpClientFactory.CreateClient("API");

            var payload = JsonSerializer.Serialize(new { email, password, role });
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/auth/login", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = "Invalid email or password";
                try
                {
                    using var errorDoc = JsonDocument.Parse(responseBody);
                    if (errorDoc.RootElement.TryGetProperty("message", out var messageElement))
                    {
                        errorMessage = messageElement.GetString() ?? errorMessage;
                    }
                }
                catch
                {
                    // ignore parse errors
                }

                _logger.LogWarning("Failed login attempt for: {Email} as {Role}. Error: {Error}", email, role, errorMessage);
                ViewData["LoginErrorMessage"] = errorMessage;
                ViewData["SelectedLoginRole"] = role;
                return View();
            }

            var json = responseBody;
            using var document = JsonDocument.Parse(json);
            var root = document.RootElement;
            var token = root.GetProperty("token").GetString();
            var userId = root.GetProperty("userId").GetInt32();
            var userRole = root.GetProperty("role").GetString() ?? string.Empty;
            var patientId = root.TryGetProperty("patientId", out var patientIdElement) && patientIdElement.ValueKind == JsonValueKind.Number
                ? patientIdElement.GetInt32()
                : 0;

            HttpContext.Session.SetString("JwtToken", token ?? string.Empty);
            HttpContext.Session.SetString("UserRole", userRole);
            HttpContext.Session.SetInt32("UserId", userId);
            HttpContext.Session.SetInt32("PatientId", patientId);
            _logger.LogInformation("Successful login for: {Email} as {Role}", email, userRole);

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(string fullName, string email, string password, string phoneNumber, DateTime dateOfBirth, string address)
        {
            var client = _httpClientFactory.CreateClient("API");

            var payload = JsonSerializer.Serialize(new
            {
                fullName,
                email,
                password,
                phoneNumber,
                dateOfBirth,
                address
            });
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/auth/register", content);
            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Registration failed for: {Email}. Response: {Response}", email, responseBody);
                ModelState.AddModelError("", "Registration failed. The email may already be in use.");
                return View();
            }

            var json = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(json);
            var root = document.RootElement;
            var token = root.GetProperty("token").GetString();
            var userId = root.GetProperty("userId").GetInt32();
            var role = root.GetProperty("role").GetString() ?? string.Empty;
            var patientId = root.TryGetProperty("patientId", out var patientIdElement) && patientIdElement.ValueKind == JsonValueKind.Number
                ? patientIdElement.GetInt32()
                : 0;

            HttpContext.Session.SetString("JwtToken", token ?? string.Empty);
            HttpContext.Session.SetString("UserRole", role);
            HttpContext.Session.SetInt32("UserId", userId);
            HttpContext.Session.SetInt32("PatientId", patientId);

            _logger.LogInformation("Registered and logged in new patient: {Email}", email);
            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}