using HealthCareSmart.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HealthCareSmart.MVC.Controllers
{
    public class BillingsController : Controller
    {
        private readonly ApiService _apiService;

        public BillingsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            var client = _apiService.GetAuthenticatedClient();
            var response = await client.GetAsync("api/billings");

            List<Dictionary<string, object>>? billings = null;
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                billings = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            ViewBag.Billings = billings ?? new List<Dictionary<string, object>>();
            ViewBag.JwtToken = token;
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            var client = _apiService.GetAuthenticatedClient();
            var response = await client.GetAsync($"api/billings/{id}");

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            var json = await response.Content.ReadAsStringAsync();
            var billing = JsonSerializer.Deserialize<Dictionary<string, object>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            ViewBag.Billing = billing ?? new Dictionary<string, object>();
            return View();
        }
    }
}
