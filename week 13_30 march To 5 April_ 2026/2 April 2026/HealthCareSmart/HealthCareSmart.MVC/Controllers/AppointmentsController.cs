using HealthCareSmart.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace HealthCareSmart.MVC.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly ApiService _apiService;

        public AppointmentsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            var client = _apiService.GetAuthenticatedClient();
            var response = await client.GetAsync("api/appointments");

            List<Dictionary<string, object>>? appointments = null;
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                appointments = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            ViewBag.Appointments = appointments ?? new List<Dictionary<string, object>>();
            ViewBag.JwtToken = token;
            return View();
        }

        private readonly Dictionary<string, string> _fieldCategories = new(StringComparer.OrdinalIgnoreCase)
        {
            ["X-Ray"] = "Radiology",
            ["Heart Issue"] = "Cardiology",
            ["Ear or Eye Issues"] = "ENT,Ophthalmology",
            ["Full Body Checkup"] = "General Medicine",
            ["Skin Care"] = "Dermatology",
            ["Child Health"] = "Pediatrics",
            ["Women's Health"] = "Gynecology",
            ["Brain & Nerves"] = "Neurology"
        };

        public async Task<IActionResult> Create(int? doctorId, string? specialization)
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            var client = _apiService.GetAuthenticatedClient();
            ViewBag.FieldCategories = _fieldCategories;
            ViewBag.SelectedField = specialization;

            if (doctorId != null)
            {
                var response = await client.GetAsync($"api/doctors/{doctorId}");

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Error = "Selected doctor could not be loaded. Please choose another doctor.";
                    return View();
                }

                var json = await response.Content.ReadAsStringAsync();
                var doctor = JsonSerializer.Deserialize<Dictionary<string, object>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                ViewBag.SelectedDoctor = doctor;
                ViewBag.DoctorId = doctorId;
                return View();
            }

            if (!string.IsNullOrWhiteSpace(specialization))
            {
                var mapped = _fieldCategories.TryGetValue(specialization, out var mappedValue)
                    ? mappedValue
                    : specialization?.Trim();
                var searchValue = mapped ?? string.Empty;

                var requestUrl = string.IsNullOrWhiteSpace(searchValue) || searchValue == "*"
                    ? "api/doctors"
                    : $"api/doctors/search?specialization={Uri.EscapeDataString(searchValue)}";

                var response = await client.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    ViewBag.Doctors = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new List<Dictionary<string, object>>();
                }
                else
                {
                    ViewBag.Error = "Unable to load doctors for the selected field.";
                    ViewBag.Doctors = new List<Dictionary<string, object>>();
                }
            }
            else
            {
                ViewBag.Doctors = new List<Dictionary<string, object>>();
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DateTime appointmentDate, int doctorId, string notes)
        {
            var patientId = HttpContext.Session.GetInt32("PatientId");
            var role = HttpContext.Session.GetString("UserRole");

            if (patientId == null || patientId == 0 || role != "Patient")
                return RedirectToAction("Login", "Account");

            var client = _apiService.GetAuthenticatedClient();
            var payload = JsonSerializer.Serialize(new { appointmentDate, doctorId, patientId = patientId.Value, notes });
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/appointments", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            var errorBody = await response.Content.ReadAsStringAsync();
            ViewBag.Error = $"Failed to create appointment ({(int)response.StatusCode}): {errorBody}";
            var doctorResponse = await client.GetAsync($"api/doctors/{doctorId}");
            if (doctorResponse.IsSuccessStatusCode)
            {
                var doctorJson = await doctorResponse.Content.ReadAsStringAsync();
                ViewBag.SelectedDoctor = JsonSerializer.Deserialize<Dictionary<string, object>>(doctorJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            ViewBag.DoctorId = doctorId;
            return View();
        }

        // GET: Appointments/Prescription/{id}
        public async Task<IActionResult> Prescription(int id)
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            var client = _apiService.GetAuthenticatedClient();
            var response = await client.GetAsync($"api/appointments/{id}/prescription");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var prescription = JsonSerializer.Deserialize<Dictionary<string, object>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(prescription);
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                var errorJson = await response.Content.ReadAsStringAsync();
                var error = JsonSerializer.Deserialize<Dictionary<string, object>>(errorJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                ViewBag.Error = error?["message"]?.ToString() ?? "Prescription not found";
            }
            else
            {
                ViewBag.Error = $"Failed to load prescription (Status: {(int)response.StatusCode})";
            }

            return View();
        }
    }
}