using AutoMapper;
using HospitalAPI.Data;
using HospitalAPI.DTOs;
using HospitalAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly HospitalDbContext _db;
    private readonly IMapper _mapper;
    private readonly ILogger<AdminController> _logger;

    public AdminController(HospitalDbContext db, IMapper mapper, ILogger<AdminController> logger)
    {
        _db = db;
        _mapper = mapper;
        _logger = logger;
    }

    // =============================================
    // DASHBOARD
    // =============================================
    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard()
    {
        var today = DateTime.Today;

        var dto = new AdminDashboardDto
        {
            TotalPatients = await _db.Users.CountAsync(u => u.Role == "Patient" && u.IsActive),
            TotalDoctors = await _db.Doctors.CountAsync(d => d.IsAvailable),
            TotalAppointments = await _db.Appointments.CountAsync(),
            TodayAppointments = await _db.Appointments.CountAsync(a => a.AppointmentDate.Date == today),
            TotalRevenue = await _db.Bills.Where(b => b.PaymentStatus == "Paid").SumAsync(b => b.TotalAmount),
            PendingBills = await _db.Bills.CountAsync(b => b.PaymentStatus == "Unpaid"),
        };

        var recentAppts = await _db.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor).ThenInclude(d => d.User)
            .Include(a => a.Doctor).ThenInclude(d => d.Department)
            .Include(a => a.Prescription)
            .Include(a => a.Bill)
            .OrderByDescending(a => a.CreatedAt)
            .Take(10)
            .ToListAsync();

        dto.RecentAppointments = _mapper.Map<List<AppointmentDto>>(recentAppts);

        return Ok(dto);
    }

    // =============================================
    // USERS (all)
    // =============================================
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers([FromQuery] string? role)
    {
        var query = _db.Users.AsQueryable();
        if (!string.IsNullOrEmpty(role))
            query = query.Where(u => u.Role == role);

        var users = await query.OrderBy(u => u.FullName).ToListAsync();
        return Ok(_mapper.Map<List<UserDto>>(users));
    }

    [HttpPut("users/{id}/toggle")]
    public async Task<IActionResult> ToggleUser(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return NotFound();
        user.IsActive = !user.IsActive;
        await _db.SaveChangesAsync();
        return Ok(new { message = $"User {(user.IsActive ? "activated" : "deactivated")}." });
    }

    [HttpDelete("users/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return NotFound();
        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return Ok(new { message = "User deleted." });
    }

    // =============================================
    // DEPARTMENTS
    // =============================================
    [HttpGet("departments")]
    [AllowAnonymous]
    public async Task<IActionResult> GetDepartments()
    {
        var depts = await _db.Departments
            .Include(d => d.Doctors)
            .Where(d => d.IsActive)
            .ToListAsync();
        return Ok(_mapper.Map<List<DepartmentDto>>(depts));
    }

    [HttpPost("departments")]
    public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentDto dto)
    {
        var dept = _mapper.Map<Department>(dto);
        _db.Departments.Add(dept);
        await _db.SaveChangesAsync();
        return Ok(_mapper.Map<DepartmentDto>(dept));
    }

    [HttpPut("departments/{id}")]
    public async Task<IActionResult> UpdateDepartment(int id, [FromBody] CreateDepartmentDto dto)
    {
        var dept = await _db.Departments.FindAsync(id);
        if (dept == null) return NotFound();
        dept.DepartmentName = dto.DepartmentName;
        dept.Description = dto.Description;
        dept.IconClass = dto.IconClass;
        await _db.SaveChangesAsync();
        return Ok(_mapper.Map<DepartmentDto>(dept));
    }

    [HttpDelete("departments/{id}")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        var dept = await _db.Departments.FindAsync(id);
        if (dept == null) return NotFound();
        dept.IsActive = false;
        await _db.SaveChangesAsync();
        return Ok(new { message = "Department deactivated." });
    }

    // =============================================
    // DOCTORS (Admin manages)
    // =============================================
    [HttpPost("doctors")]
    public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorDto dto)
    {
        if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
            return BadRequest(new { message = "Email already exists." });

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            Phone = dto.Phone,
            Role = "Doctor",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var doctor = new Doctor
        {
            UserId = user.UserId,
            DepartmentId = dto.DepartmentId,
            Specialization = dto.Specialization,
            ExperienceYears = dto.ExperienceYears,
            Availability = dto.Availability,
            ConsultationFee = dto.ConsultationFee,
            Qualification = dto.Qualification
        };
        _db.Doctors.Add(doctor);
        await _db.SaveChangesAsync();

        var result = await _db.Doctors
            .Include(d => d.User)
            .Include(d => d.Department)
            .FirstAsync(d => d.DoctorId == doctor.DoctorId);

        return Ok(_mapper.Map<DoctorDto>(result));
    }

    [HttpDelete("doctors/{id}")]
    public async Task<IActionResult> DeleteDoctor(int id)
    {
        var doctor = await _db.Doctors.Include(d => d.User).FirstOrDefaultAsync(d => d.DoctorId == id);
        if (doctor == null) return NotFound();
        doctor.User.IsActive = false;
        await _db.SaveChangesAsync();
        return Ok(new { message = "Doctor deactivated." });
    }

    // =============================================
    // BILLS (admin sees all)
    // =============================================
    [HttpGet("bills")]
    public async Task<IActionResult> GetAllBills()
    {
        var bills = await _db.Bills
            .Include(b => b.Appointment).ThenInclude(a => a.Patient)
            .Include(b => b.Appointment).ThenInclude(a => a.Doctor).ThenInclude(d => d.User)
            .OrderByDescending(b => b.GeneratedAt)
            .ToListAsync();
        return Ok(_mapper.Map<List<BillDto>>(bills));
    }
}
