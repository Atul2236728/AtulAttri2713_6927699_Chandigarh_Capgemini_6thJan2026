using HealthCareSmart.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.Json;

namespace HealthCareSmart.MVC.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly ApiService _apiService;

        private readonly List<KeyValuePair<string, string>> _specialtyCategories = new()
        {
            new("X-Ray", "Radiology"),
            new("Heart Issue", "Cardiology"),
            new("Ear or Eye Issues", "ENT,Ophthalmology"),
            new("Full Body Checkup", "General Medicine"),
            new("Skin Care", "Dermatology"),
            new("Child Health", "Pediatrics"),
            new("Women's Health", "Gynecology"),
            new("Brain & Nerves", "Neurology")
        };

        public DoctorsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index(string? specialization)
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            var searchValue = specialization?.Trim();
            var mapped = _specialtyCategories.FirstOrDefault(c => string.Equals(c.Key, searchValue, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(mapped.Key))
            {
                searchValue = mapped.Value;
            }

            var client = _apiService.GetAuthenticatedClient();
            var requestUrl = string.IsNullOrWhiteSpace(searchValue) || searchValue == "*"
                ? "api/doctors"
                : $"api/doctors/search?specialization={Uri.EscapeDataString(searchValue)}";

            var response = await client.GetAsync(requestUrl);

            List<Dictionary<string, object>>? doctors = null;
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                doctors = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else
            {
                ViewBag.Error = "Unable to load doctors for that field. Showing all available doctors instead.";
                var fallback = await client.GetAsync("api/doctors");
                if (fallback.IsSuccessStatusCode)
                {
                    var fallbackJson = await fallback.Content.ReadAsStringAsync();
                    doctors = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(fallbackJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }

            var specialties = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (doctors != null)
            {
                foreach (var doctor in doctors)
                {
                    if (doctor["specializations"] is JsonElement specs)
                    {
                        foreach (var item in specs.EnumerateArray())
                        {
                            var name = item.GetString();
                            if (!string.IsNullOrWhiteSpace(name))
                                specialties.Add(name);
                        }
                    }
                }
            }

            ViewBag.Doctors = doctors ?? new List<Dictionary<string, object>>();
            ViewBag.Specializations = specialties.OrderBy(x => x).ToList();
            ViewBag.ActiveSpecialization = specialization;
            ViewBag.SpecialtyCategories = _specialtyCategories;
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            var client = _apiService.GetAuthenticatedClient();
            var response = await client.GetAsync($"api/doctors/{id}");

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            var json = await response.Content.ReadAsStringAsync();
            var doctor = JsonSerializer.Deserialize<Dictionary<string, object>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (doctor == null)
                return RedirectToAction("Index");

            ViewBag.Doctor = doctor;
            return View();
        }
    }
}