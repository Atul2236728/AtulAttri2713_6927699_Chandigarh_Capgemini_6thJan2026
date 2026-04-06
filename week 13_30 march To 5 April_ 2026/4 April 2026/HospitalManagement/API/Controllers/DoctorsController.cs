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
public class DoctorsController : ControllerBase
{
    private readonly HospitalDbContext _db;
    private readonly IMapper _mapper;

    public DoctorsController(HospitalDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    /// <summary>Get all doctors - filterable by department</summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] int? departmentId, [FromQuery] string? search)
    {
        var query = _db.Doctors
            .Include(d => d.User)
            .Include(d => d.Department)
            .Where(d => d.User.IsActive)
            .AsQueryable();

        if (departmentId.HasValue)
            query = query.Where(d => d.DepartmentId == departmentId);

        if (!string.IsNullOrEmpty(search))
            query = query.Where(d =>
                d.User.FullName.Contains(search) ||
                d.Specialization.Contains(search) ||
                d.Department.DepartmentName.Contains(search));

        var doctors = await query.OrderBy(d => d.User.FullName).ToListAsync();
        return Ok(_mapper.Map<List<DoctorDto>>(doctors));
    }

    /// <summary>Get doctor by ID</summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var doctor = await _db.Doctors
            .Include(d => d.User)
            .Include(d => d.Department)
            .FirstOrDefaultAsync(d => d.DoctorId == id);

        if (doctor == null) return NotFound(new { message = "Doctor not found." });
        return Ok(_mapper.Map<DoctorDto>(doctor));
    }

    /// <summary>Update doctor profile (self or admin)</summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Doctor,Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateDoctorDto dto)
    {
        var doctor = await _db.Doctors.FindAsync(id);
        if (doctor == null) return NotFound();

        doctor.DepartmentId = dto.DepartmentId;
        doctor.Specialization = dto.Specialization;
        doctor.ExperienceYears = dto.ExperienceYears;
        doctor.Availability = dto.Availability;
        doctor.ConsultationFee = dto.ConsultationFee;
        doctor.Qualification = dto.Qualification;
        doctor.IsAvailable = dto.IsAvailable;

        await _db.SaveChangesAsync();
        return Ok(new { message = "Doctor updated." });
    }

    /// <summary>Get doctor's dashboard</summary>
    [HttpGet("{id}/dashboard")]
    [Authorize(Roles = "Doctor,Admin")]
    public async Task<IActionResult> DoctorDashboard(int id)
    {
        var today = DateTime.Today;

        var todayAppts = await _db.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor).ThenInclude(d => d.User)
            .Include(a => a.Doctor).ThenInclude(d => d.Department)
            .Include(a => a.Prescription)
            .Include(a => a.Bill)
            .Where(a => a.DoctorId == id && a.AppointmentDate.Date == today)
            .OrderBy(a => a.AppointmentDate)
            .ToListAsync();

        var dto = new DoctorDashboardDto
        {
            TodayAppointments = await _db.Appointments.CountAsync(a => a.DoctorId == id && a.AppointmentDate.Date == today),
            PendingAppointments = await _db.Appointments.CountAsync(a => a.DoctorId == id && a.Status == "Booked"),
            CompletedToday = await _db.Appointments.CountAsync(a => a.DoctorId == id && a.AppointmentDate.Date == today && a.Status == "Completed"),
            TotalPatients = await _db.Appointments.Where(a => a.DoctorId == id).Select(a => a.PatientId).Distinct().CountAsync(),
            TodaySchedule = _mapper.Map<List<AppointmentDto>>(todayAppts)
        };

        return Ok(dto);
    }

    /// <summary>Get all appointments for a doctor</summary>
    [HttpGet("{id}/appointments")]
    [Authorize(Roles = "Doctor,Admin")]
    public async Task<IActionResult> GetAppointments(int id,
        [FromQuery] string? status,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to)
    {
        var query = _db.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor).ThenInclude(d => d.User)
            .Include(a => a.Doctor).ThenInclude(d => d.Department)
            .Include(a => a.Prescription)
            .Include(a => a.Bill)
            .Where(a => a.DoctorId == id)
            .AsQueryable();

        if (!string.IsNullOrEmpty(status))
            query = query.Where(a => a.Status == status);
        if (from.HasValue)
            query = query.Where(a => a.AppointmentDate >= from);
        if (to.HasValue)
            query = query.Where(a => a.AppointmentDate <= to);

        var appts = await query.OrderByDescending(a => a.AppointmentDate).ToListAsync();
        return Ok(_mapper.Map<List<AppointmentDto>>(appts));
    }
}
