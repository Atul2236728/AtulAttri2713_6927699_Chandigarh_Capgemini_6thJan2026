using HospitalWeb.Models;
using HospitalWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HospitalWeb.Controllers;

// =============================================
// HOME CONTROLLER
// =============================================
public class HomeController : Controller
{
    private readonly ApiService _api;

    public HomeController(ApiService api) => _api = api;

    public async Task<IActionResult> Index()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            return role switch
            {
                "Admin" => RedirectToAction("Index", "Admin"),
                "Doctor" => RedirectToAction("Index", "Doctor"),
                _ => RedirectToAction("Index", "Patient")
            };
        }

        var departments = await _api.GetAsync<List<DepartmentDto>>("api/admin/departments") ?? new();
        var doctors = await _api.GetAsync<List<DoctorDto>>("api/doctors") ?? new();

        ViewBag.Departments = departments;
        ViewBag.FeaturedDoctors = doctors.Take(6).ToList();
        return View();
    }

    public IActionResult Error() => View();
}

// =============================================
// ADMIN CONTROLLER
// =============================================
[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApiService _api;

    public AdminController(ApiService api) => _api = api;

    public async Task<IActionResult> Index()
    {
        var dashboard = await _api.GetAsync<AdminDashboardDto>("api/admin/dashboard") ?? new();
        return View(dashboard);
    }

    // DOCTORS
    public async Task<IActionResult> Doctors()
    {
        var doctors = await _api.GetAsync<List<DoctorDto>>("api/doctors") ?? new();
        return View(doctors);
    }

    [HttpGet]
    public async Task<IActionResult> CreateDoctor()
    {
        var departments = await _api.GetAsync<List<DepartmentDto>>("api/admin/departments") ?? new();
        return View(new CreateDoctorViewModel { Departments = departments });
    }

    [HttpPost]
    public async Task<IActionResult> CreateDoctor(CreateDoctorViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Departments = await _api.GetAsync<List<DepartmentDto>>("api/admin/departments") ?? new();
            return View(model);
        }

        var (success, _, error) = await _api.PostAsync("api/admin/doctors", new
        {
            fullName = model.FullName,
            email = model.Email,
            password = model.Password,
            phone = model.Phone,
            departmentId = model.DepartmentId,
            specialization = model.Specialization,
            experienceYears = model.ExperienceYears,
            availability = model.Availability,
            consultationFee = model.ConsultationFee,
            qualification = model.Qualification
        });

        if (!success)
        {
            ModelState.AddModelError("", error ?? "Failed to create doctor.");
            model.Departments = await _api.GetAsync<List<DepartmentDto>>("api/admin/departments") ?? new();
            return View(model);
        }

        TempData["Success"] = "Doctor created successfully!";
        return RedirectToAction(nameof(Doctors));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteDoctor(int id)
    {
        await _api.DeleteAsync($"api/admin/doctors/{id}");
        TempData["Success"] = "Doctor deactivated.";
        return RedirectToAction(nameof(Doctors));
    }

    // PATIENTS
    public async Task<IActionResult> Patients()
    {
        var patients = await _api.GetAsync<List<PatientProfileDto>>("api/patients") ?? new();
        return View(patients);
    }

    // DEPARTMENTS
    public async Task<IActionResult> Departments()
    {
        var departments = await _api.GetAsync<List<DepartmentDto>>("api/admin/departments") ?? new();
        return View(departments);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDepartment(string departmentName, string? description, string iconClass)
    {
        await _api.PostAsync("api/admin/departments", new
        {
            departmentName,
            description,
            iconClass = iconClass ?? "fa-stethoscope"
        });
        TempData["Success"] = "Department created.";
        return RedirectToAction(nameof(Departments));
    }

    // BILLS
    public async Task<IActionResult> Bills()
    {
        var bills = await _api.GetAsync<List<BillDto>>("api/admin/bills") ?? new();
        return View(bills);
    }

    [HttpPost]
    public async Task<IActionResult> UpdatePayment(int billId, string paymentStatus, string? paymentMethod)
    {
        await _api.PatchAsync($"api/bills/{billId}/payment", new
        {
            paymentStatus,
            paymentMethod
        });
        TempData["Success"] = "Payment status updated.";
        return RedirectToAction(nameof(Bills));
    }

    // USERS
    public async Task<IActionResult> Users()
    {
        var users = await _api.GetAsync<List<UserDto>>("api/admin/users") ?? new();
        return View(users);
    }

    [HttpPost]
    public async Task<IActionResult> ToggleUser(int id)
    {
        await _api.PutAsync($"api/admin/users/{id}/toggle", new { });
        return RedirectToAction(nameof(Users));
    }

    // MEDICINES
    public async Task<IActionResult> Medicines()
    {
        var meds = await _api.GetAsync<List<MedicineDto>>("api/medicines") ?? new();
        return View(meds);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMedicine(string medicineName, decimal unitPrice, string unit)
    {
        await _api.PostAsync("api/medicines", new { medicineName, unitPrice, unit });
        TempData["Success"] = "Medicine added.";
        return RedirectToAction(nameof(Medicines));
    }
}

// =============================================
// DOCTOR CONTROLLER
// =============================================
[Authorize(Roles = "Doctor")]
public class DoctorController : Controller
{
    private readonly ApiService _api;

    public DoctorController(ApiService api) => _api = api;

    private int GetDoctorId() => HttpContext.Session.GetInt32("DoctorId") ?? 0;
    private int GetUserId() => int.Parse(User.FindFirst("UserId")?.Value ?? "0");

    public async Task<IActionResult> Index()
    {
        var doctorId = GetDoctorId();
        var dashboard = await _api.GetAsync<DoctorDashboardDto>($"api/doctors/{doctorId}/dashboard") ?? new();
        return View(dashboard);
    }

    public async Task<IActionResult> Appointments(string? status)
    {
        var doctorId = GetDoctorId();
        var url = $"api/doctors/{doctorId}/appointments";
        if (!string.IsNullOrEmpty(status)) url += $"?status={status}";
        var appointments = await _api.GetAsync<List<AppointmentDto>>(url) ?? new();
        ViewBag.StatusFilter = status;
        return View(appointments);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateStatus(int appointmentId, string status, string? notes)
    {
        await _api.PatchAsync($"api/appointments/{appointmentId}/status", new { status, notes });
        TempData["Success"] = "Appointment status updated.";
        return RedirectToAction(nameof(Appointments));
    }

    [HttpGet]
    public async Task<IActionResult> WritePrescription(int appointmentId)
    {
        var appt = await _api.GetAsync<AppointmentDto>($"api/appointments/{appointmentId}");
        if (appt == null) return NotFound();

        var medicines = await _api.GetAsync<List<MedicineDto>>("api/medicines") ?? new();

        var vm = new CreatePrescriptionViewModel
        {
            AppointmentId = appointmentId,
            PatientName = appt.PatientName,
            AllMedicines = medicines
        };

        // Pre-load if editing
        if (appt.HasPrescription)
        {
            var existing = await _api.GetAsync<PrescriptionDto>($"api/prescriptions/appointment/{appointmentId}");
            if (existing != null)
            {
                vm.Diagnosis = existing.Diagnosis;
                vm.Medicines = existing.Medicines;
                vm.Notes = existing.Notes;
                vm.FollowUpDate = existing.FollowUpDate;
            }
        }

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> WritePrescription(CreatePrescriptionViewModel model)
    {
        var medicines = model.SelectedMedicines
            .Where(m => m.MedicineId > 0)
            .Select(m => new { m.MedicineId, m.Quantity, m.Dosage, m.Duration })
            .ToList();

        var existing = await _api.GetAsync<PrescriptionDto>(
            $"api/prescriptions/appointment/{model.AppointmentId}");

        bool success;
        string? error;

        if (existing != null)
        {
            (success, error) = await _api.PutAsync($"api/prescriptions/{existing.PrescriptionId}", new
            {
                appointmentId = model.AppointmentId,
                diagnosis = model.Diagnosis,
                medicines = model.Medicines,
                notes = model.Notes,
                followUpDate = model.FollowUpDate,
                prescriptionMedicines = medicines
            });
        }
        else
        {
            string? json;
            (success, json, error) = await _api.PostAsync("api/prescriptions", new
            {
                appointmentId = model.AppointmentId,
                diagnosis = model.Diagnosis,
                medicines = model.Medicines,
                notes = model.Notes,
                followUpDate = model.FollowUpDate,
                prescriptionMedicines = medicines
            });
        }

        if (!success)
        {
            TempData["Error"] = error ?? "Failed to save prescription.";
            return RedirectToAction(nameof(WritePrescription), new { appointmentId = model.AppointmentId });
        }

        TempData["Success"] = "Prescription saved and bill generated!";
        return RedirectToAction(nameof(Appointments));
    }

    public async Task<IActionResult> ViewPrescription(int appointmentId)
    {
        var p = await _api.GetAsync<PrescriptionDto>($"api/prescriptions/appointment/{appointmentId}");
        if (p == null) { TempData["Error"] = "No prescription found."; return RedirectToAction(nameof(Appointments)); }
        return View(p);
    }

    public async Task<IActionResult> Profile()
    {
        var doctorId = GetDoctorId();
        var doctor = await _api.GetAsync<DoctorDto>($"api/doctors/{doctorId}");
        return View(doctor);
    }
}

// =============================================
// PATIENT CONTROLLER
// =============================================
[Authorize(Roles = "Patient")]
public class PatientController : Controller
{
    private readonly ApiService _api;

    public PatientController(ApiService api) => _api = api;

    private int GetUserId() => int.Parse(User.FindFirst("UserId")?.Value ?? "0");

    public async Task<IActionResult> Index()
    {
        var userId = GetUserId();
        var appointments = await _api.GetAsync<List<AppointmentDto>>(
            $"api/appointments/patient/{userId}") ?? new();

        var upcoming = appointments.Where(a => a.Status != "Cancelled" && a.Status != "Completed")
                                   .OrderBy(a => a.AppointmentDate).ToList();
        var past = appointments.Where(a => a.Status == "Completed").OrderByDescending(a => a.AppointmentDate).ToList();

        ViewBag.UpcomingAppointments = upcoming;
        ViewBag.PastAppointments = past;
        ViewBag.TotalAppointments = appointments.Count;
        return View();
    }

    public async Task<IActionResult> Doctors(int? departmentId, string? search)
    {
        var departments = await _api.GetAsync<List<DepartmentDto>>("api/admin/departments") ?? new();
        var url = "api/doctors?";
        if (departmentId.HasValue) url += $"departmentId={departmentId}&";
        if (!string.IsNullOrEmpty(search)) url += $"search={search}";

        var doctors = await _api.GetAsync<List<DoctorDto>>(url) ?? new();

        ViewBag.Departments = departments;
        ViewBag.SelectedDept = departmentId;
        ViewBag.Search = search;
        return View(doctors);
    }

    [HttpGet]
    public async Task<IActionResult> BookAppointment(int doctorId)
    {
        var doctor = await _api.GetAsync<DoctorDto>($"api/doctors/{doctorId}");
        if (doctor == null) return NotFound();

        return View(new BookAppointmentViewModel
        {
            DoctorId = doctorId,
            DoctorName = doctor.FullName,
            DepartmentName = doctor.DepartmentName,
            ConsultationFee = doctor.ConsultationFee,
            Availability = doctor.Availability,
            AppointmentDate = DateTime.Today.AddDays(1)
        });
    }

    [HttpPost]
    public async Task<IActionResult> BookAppointment(BookAppointmentViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var (success, _, error) = await _api.PostAsync("api/appointments", new
        {
            doctorId = model.DoctorId,
            appointmentDate = model.AppointmentDate,
            timeSlot = model.TimeSlot,
            reason = model.Reason
        });

        if (!success)
        {
            ModelState.AddModelError("", error ?? "Booking failed.");
            return View(model);
        }

        TempData["Success"] = "Appointment booked successfully!";
        return RedirectToAction(nameof(Appointments));
    }

    public async Task<IActionResult> Appointments()
    {
        var userId = GetUserId();
        var appointments = await _api.GetAsync<List<AppointmentDto>>(
            $"api/appointments/patient/{userId}") ?? new();
        return View(appointments);
    }

    [HttpPost]
    public async Task<IActionResult> CancelAppointment(int id)
    {
        await _api.DeleteAsync($"api/appointments/{id}");
        TempData["Success"] = "Appointment cancelled.";
        return RedirectToAction(nameof(Appointments));
    }

    public async Task<IActionResult> Prescriptions()
    {
        var userId = GetUserId();
        var prescriptions = await _api.GetAsync<List<PrescriptionDto>>(
            $"api/prescriptions/patient/{userId}") ?? new();
        return View(prescriptions);
    }

    public async Task<IActionResult> PrescriptionDetail(int id)
    {
        var p = await _api.GetAsync<PrescriptionDto>($"api/prescriptions/{id}");
        if (p == null) return NotFound();
        return View(p);
    }

    public async Task<IActionResult> Bills()
    {
        var userId = GetUserId();
        var bills = await _api.GetAsync<List<BillDto>>($"api/bills/patient/{userId}") ?? new();
        return View(bills);
    }

    public async Task<IActionResult> BillDetail(int appointmentId)
    {
        var bill = await _api.GetAsync<BillDto>($"api/bills/appointment/{appointmentId}");
        if (bill == null) return NotFound();
        return View(bill);
    }

    public async Task<IActionResult> Profile()
    {
        var userId = GetUserId();
        var profile = await _api.GetAsync<PatientProfileDto>($"api/patients/{userId}");
        return View(profile);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProfile(PatientProfileDto model)
    {
        var userId = GetUserId();
        await _api.PutAsync($"api/patients/{userId}", new
        {
            dateOfBirth = model.DateOfBirth,
            gender = model.Gender,
            bloodGroup = model.BloodGroup,
            address = model.Address,
            emergencyContact = model.EmergencyContact,
            medicalHistory = model.MedicalHistory
        });
        TempData["Success"] = "Profile updated.";
        return RedirectToAction(nameof(Profile));
    }
}
