using HealthCareSmart.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HealthCareSmart.MVC.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApiService _apiService;

        public DashboardController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            var client = _apiService.GetAuthenticatedClient();

            // Fetch appointments
            var apptResponse = await client.GetAsync("api/appointments");
            List<Dictionary<string, object>>? appointments = null;
            if (apptResponse.IsSuccessStatusCode)
            {
                var json = await apptResponse.Content.ReadAsStringAsync();
                appointments = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            // Fetch doctors
            var doctorResponse = await client.GetAsync("api/doctors");
            int doctorCount = 0;
            if (doctorResponse.IsSuccessStatusCode)
            {
                var json = await doctorResponse.Content.ReadAsStringAsync();
                var doctors = JsonSerializer.Deserialize<List<object>>(json);
                doctorCount = doctors?.Count ?? 0;
            }

            ViewBag.Appointments = appointments ?? new List<Dictionary<string, object>>();
            ViewBag.AppointmentCount = appointments?.Count ?? 0;
            ViewBag.DoctorCount = doctorCount;
            ViewBag.PatientCount = 0;
            ViewBag.PendingCount = appointments?.Count(a =>
                a.ContainsKey("status") && a["status"]?.ToString() == "Pending") ?? 0;

            return View();
        }
    }
}