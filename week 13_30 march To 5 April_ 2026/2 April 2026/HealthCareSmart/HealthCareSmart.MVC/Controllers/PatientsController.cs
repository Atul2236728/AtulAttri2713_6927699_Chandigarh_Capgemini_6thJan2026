using HealthCareSmart.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HealthCareSmart.MVC.Controllers
{
    public class PatientsController : Controller
    {
        private readonly ApiService _apiService;

        public PatientsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            var patientId = HttpContext.Session.GetInt32("PatientId");
            if (patientId == null || patientId == 0)
                return RedirectToAction("Login", "Account");

            var client = _apiService.GetAuthenticatedClient();
            var response = await client.GetAsync("api/appointments");

            var appointments = new List<Dictionary<string, object>>();
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var allAppointments = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new List<Dictionary<string, object>>();

                appointments = allAppointments.Where(a =>
                    a.TryGetValue("patientId", out var pidValue)
                    && pidValue != null
                    && int.TryParse(pidValue.ToString() ?? string.Empty, out var pid)
                    && pid == patientId.Value
                ).ToList();
            }

            ViewBag.Appointments = appointments;
            return View();
        }
    }
}