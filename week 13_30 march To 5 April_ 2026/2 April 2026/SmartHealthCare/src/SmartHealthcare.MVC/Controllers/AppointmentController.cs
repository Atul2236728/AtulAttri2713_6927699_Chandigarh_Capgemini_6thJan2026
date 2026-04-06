using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // IMPORTANT
using SmartHealthcare.MVC.Services;
using SmartHealthcare.Models.DTOs;

namespace SmartHealthcare.MVC.Controllers;

public class AppointmentController : Controller
{
    private readonly IApiService _apiService;
    private readonly ILogger<AppointmentController> _logger;

    public AppointmentController(IApiService apiService, ILogger<AppointmentController> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var token = HttpContext.Session.GetString("Token");
        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Account");
        }

        var role = HttpContext.Session.GetString("Role");
        PagedResult<AppointmentDTO>? result = null;

        if (role == "Patient")
        {
            result = await _apiService.GetAsync<PagedResult<AppointmentDTO>>("appointments/my-appointments?pageNumber=1&pageSize=50", token);
        }
        else if (role == "Doctor")
        {
            result = await _apiService.GetAsync<PagedResult<AppointmentDTO>>("appointments/doctor-appointments?pageNumber=1&pageSize=50", token);
        }
        else if (role == "Admin")
        {
            result = await _apiService.GetAsync<PagedResult<AppointmentDTO>>("appointments?pageNumber=1&pageSize=50", token);
        }

        ViewBag.Role = role;
        return View(result?.Items ?? Enumerable.Empty<AppointmentDTO>());
    }

    /* ========================= */
    /* 🔥 CREATE (GET) */
    /* ========================= */
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var token = HttpContext.Session.GetString("Token");
        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Account");
        }

        await LoadDoctors(token);

        return View();
    }

    /* ========================= */
    /* 🔥 CREATE (POST) */
    /* ========================= */
    [HttpPost]
    public async Task<IActionResult> Create(CreateAppointmentDTO dto)
    {
        var token = HttpContext.Session.GetString("Token");
        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Account");
        }

        if (!ModelState.IsValid)
        {
            await LoadDoctors(token);
            return View(dto);
        }

        var result = await _apiService.PostAsync<AppointmentDTO>("appointments", dto, token);

        if (result == null)
        {
            ModelState.AddModelError("", "Failed to create appointment.");
            await LoadDoctors(token);
            return View(dto);
        }

        _logger.LogInformation("Appointment booked successfully");
        TempData["Success"] = "Appointment booked successfully!";
        return RedirectToAction(nameof(Index));
    }

    /* ========================= */
    /* 🔥 HELPER METHOD */
    /* ========================= */
    private async Task LoadDoctors(string token)
    {
        // The doctors API returns a paged result, not a raw list.
        var doctorsPage = await _apiService.GetAsync<PagedResult<DoctorDTO>>("doctors?pageNumber=1&pageSize=100", token);
        var doctors = doctorsPage?.Items ?? Enumerable.Empty<DoctorDTO>();

        ViewBag.Doctors = doctors.Select(d => new SelectListItem
        {
            Value = d.Id.ToString(),
            Text = $"Dr. {d.FullName}"
        }).ToList();
    }

    public async Task<IActionResult> Details(int id)
    {
        var token = HttpContext.Session.GetString("Token");
        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Account");
        }

        var appointment = await _apiService.GetAsync<AppointmentDTO>($"appointments/{id}", token);
        if (appointment == null)
        {
            return NotFound();
        }

        ViewBag.Role = HttpContext.Session.GetString("Role");
        ViewBag.Prescription = await _apiService.GetAsync<PrescriptionDTO>($"prescriptions/appointment/{id}", token);

        return View(appointment);
    }
}