using AutoMapper;
using HospitalAPI.Data;
using HospitalAPI.DTOs;
using HospitalAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Controllers;

// =============================================
// APPOINTMENTS CONTROLLER
// =============================================
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppointmentsController : ControllerBase
{
    private readonly HospitalDbContext _db;
    private readonly IMapper _mapper;

    public AppointmentsController(HospitalDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    private IQueryable<Appointment> BaseQuery() => _db.Appointments
        .Include(a => a.Patient)
        .Include(a => a.Doctor).ThenInclude(d => d.User)
        .Include(a => a.Doctor).ThenInclude(d => d.Department)
        .Include(a => a.Prescription)
        .Include(a => a.Bill);

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var appt = await BaseQuery().FirstOrDefaultAsync(a => a.AppointmentId == id);
        if (appt == null) return NotFound();
        return Ok(_mapper.Map<AppointmentDto>(appt));
    }

    [HttpGet("patient/{patientId}")]
    public async Task<IActionResult> GetByPatient(int patientId)
    {
        var appts = await BaseQuery()
            .Where(a => a.PatientId == patientId)
            .OrderByDescending(a => a.AppointmentDate)
            .ToListAsync();
        return Ok(_mapper.Map<List<AppointmentDto>>(appts));
    }

    [HttpPost]
    [Authorize(Roles = "Patient,Admin")]
    public async Task<IActionResult> Book([FromBody] BookAppointmentDto dto)
    {
        var userId = int.Parse(User.FindFirst("userId")!.Value);

        // Check doctor exists and is available
        var doctor = await _db.Doctors.FindAsync(dto.DoctorId);
        if (doctor == null || !doctor.IsAvailable)
            return BadRequest(new { message = "Doctor not available." });

        // Check no duplicate booking same day same doctor
        var existing = await _db.Appointments.AnyAsync(a =>
            a.DoctorId == dto.DoctorId &&
            a.PatientId == userId &&
            a.AppointmentDate.Date == dto.AppointmentDate.Date &&
            a.Status != "Cancelled");

        if (existing)
            return BadRequest(new { message = "You already have an appointment with this doctor on this date." });

        var appt = new Appointment
        {
            PatientId = userId,
            DoctorId = dto.DoctorId,
            AppointmentDate = dto.AppointmentDate,
            TimeSlot = dto.TimeSlot,
            Reason = dto.Reason,
            Status = "Booked"
        };

        _db.Appointments.Add(appt);
        await _db.SaveChangesAsync();

        var result = await BaseQuery().FirstAsync(a => a.AppointmentId == appt.AppointmentId);
        return Ok(_mapper.Map<AppointmentDto>(result));
    }

    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Doctor,Admin")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateAppointmentStatusDto dto)
    {
        var appt = await _db.Appointments.FindAsync(id);
        if (appt == null) return NotFound();

        appt.Status = dto.Status;
        if (!string.IsNullOrEmpty(dto.Notes))
            appt.Notes = dto.Notes;

        await _db.SaveChangesAsync();
        return Ok(new { message = "Status updated." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Cancel(int id)
    {
        var appt = await _db.Appointments.FindAsync(id);
        if (appt == null) return NotFound();
        if (appt.Status == "Completed")
            return BadRequest(new { message = "Cannot cancel a completed appointment." });

        appt.Status = "Cancelled";
        await _db.SaveChangesAsync();
        return Ok(new { message = "Appointment cancelled." });
    }
}

// =============================================
// PRESCRIPTIONS CONTROLLER
// =============================================
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PrescriptionsController : ControllerBase
{
    private readonly HospitalDbContext _db;
    private readonly IMapper _mapper;

    public PrescriptionsController(HospitalDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    private IQueryable<Prescription> BaseQuery() => _db.Prescriptions
        .Include(p => p.Appointment).ThenInclude(a => a.Patient)
        .Include(p => p.Appointment).ThenInclude(a => a.Doctor).ThenInclude(d => d.User)
        .Include(p => p.PrescriptionMedicines).ThenInclude(pm => pm.Medicine);

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var p = await BaseQuery().FirstOrDefaultAsync(x => x.PrescriptionId == id);
        if (p == null) return NotFound();
        return Ok(_mapper.Map<PrescriptionDto>(p));
    }

    [HttpGet("appointment/{appointmentId}")]
    public async Task<IActionResult> GetByAppointment(int appointmentId)
    {
        var p = await BaseQuery().FirstOrDefaultAsync(x => x.AppointmentId == appointmentId);
        if (p == null) return NotFound(new { message = "No prescription for this appointment." });
        return Ok(_mapper.Map<PrescriptionDto>(p));
    }

    [HttpGet("patient/{patientId}")]
    public async Task<IActionResult> GetByPatient(int patientId)
    {
        var list = await BaseQuery()
            .Where(p => p.Appointment.PatientId == patientId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
        return Ok(_mapper.Map<List<PrescriptionDto>>(list));
    }

    [HttpPost]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> Create([FromBody] CreatePrescriptionDto dto)
    {
        if (await _db.Prescriptions.AnyAsync(p => p.AppointmentId == dto.AppointmentId))
            return BadRequest(new { message = "Prescription already exists for this appointment." });

        var appt = await _db.Appointments.FindAsync(dto.AppointmentId);
        if (appt == null) return NotFound(new { message = "Appointment not found." });

        var pres = new Prescription
        {
            AppointmentId = dto.AppointmentId,
            Diagnosis = dto.Diagnosis,
            Medicines = dto.Medicines,
            Notes = dto.Notes,
            FollowUpDate = dto.FollowUpDate
        };

        _db.Prescriptions.Add(pres);
        await _db.SaveChangesAsync();

        // Add medicines
        foreach (var med in dto.PrescriptionMedicines)
        {
            _db.PrescriptionMedicines.Add(new PrescriptionMedicine
            {
                PrescriptionId = pres.PrescriptionId,
                MedicineId = med.MedicineId,
                Quantity = med.Quantity,
                Dosage = med.Dosage,
                Duration = med.Duration
            });
        }

        // Mark appointment completed
        appt.Status = "Completed";
        await _db.SaveChangesAsync();

        // Auto-generate bill
        var doc = await _db.Doctors.FindAsync(appt.DoctorId);
        decimal medCharges = 0;
        foreach (var pm in dto.PrescriptionMedicines)
        {
            var med = await _db.Medicines.FindAsync(pm.MedicineId);
            if (med != null) medCharges += med.UnitPrice * pm.Quantity;
        }

        _db.Bills.Add(new Bill
        {
            AppointmentId = appt.AppointmentId,
            ConsultationFee = doc?.ConsultationFee ?? 500,
            MedicineCharges = medCharges,
            PaymentStatus = "Unpaid"
        });
        await _db.SaveChangesAsync();

        var result = await BaseQuery().FirstAsync(p => p.PrescriptionId == pres.PrescriptionId);
        return Ok(_mapper.Map<PrescriptionDto>(result));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> Update(int id, [FromBody] CreatePrescriptionDto dto)
    {
        var pres = await _db.Prescriptions
            .Include(p => p.PrescriptionMedicines)
            .FirstOrDefaultAsync(p => p.PrescriptionId == id);
        if (pres == null) return NotFound();

        pres.Diagnosis = dto.Diagnosis;
        pres.Medicines = dto.Medicines;
        pres.Notes = dto.Notes;
        pres.FollowUpDate = dto.FollowUpDate;

        // Replace medicines
        _db.PrescriptionMedicines.RemoveRange(pres.PrescriptionMedicines);
        foreach (var med in dto.PrescriptionMedicines)
        {
            _db.PrescriptionMedicines.Add(new PrescriptionMedicine
            {
                PrescriptionId = pres.PrescriptionId,
                MedicineId = med.MedicineId,
                Quantity = med.Quantity,
                Dosage = med.Dosage,
                Duration = med.Duration
            });
        }

        await _db.SaveChangesAsync();
        return Ok(new { message = "Prescription updated." });
    }
}

// =============================================
// BILLS CONTROLLER
// =============================================
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BillsController : ControllerBase
{
    private readonly HospitalDbContext _db;
    private readonly IMapper _mapper;

    public BillsController(HospitalDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    private IQueryable<Bill> BaseQuery() => _db.Bills
        .Include(b => b.Appointment).ThenInclude(a => a.Patient)
        .Include(b => b.Appointment).ThenInclude(a => a.Doctor).ThenInclude(d => d.User);

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var bill = await BaseQuery().FirstOrDefaultAsync(b => b.BillId == id);
        if (bill == null) return NotFound();
        return Ok(_mapper.Map<BillDto>(bill));
    }

    [HttpGet("appointment/{appointmentId}")]
    public async Task<IActionResult> GetByAppointment(int appointmentId)
    {
        var bill = await BaseQuery().FirstOrDefaultAsync(b => b.AppointmentId == appointmentId);
        if (bill == null) return NotFound(new { message = "No bill found." });
        return Ok(_mapper.Map<BillDto>(bill));
    }

    [HttpGet("patient/{patientId}")]
    public async Task<IActionResult> GetByPatient(int patientId)
    {
        var bills = await BaseQuery()
            .Where(b => b.Appointment.PatientId == patientId)
            .OrderByDescending(b => b.GeneratedAt)
            .ToListAsync();
        return Ok(_mapper.Map<List<BillDto>>(bills));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<IActionResult> Generate([FromBody] GenerateBillDto dto)
    {
        if (await _db.Bills.AnyAsync(b => b.AppointmentId == dto.AppointmentId))
            return BadRequest(new { message = "Bill already exists." });

        var bill = new Bill
        {
            AppointmentId = dto.AppointmentId,
            ConsultationFee = dto.ConsultationFee,
            MedicineCharges = dto.MedicineCharges,
            OtherCharges = dto.OtherCharges,
            Discount = dto.Discount,
            PaymentStatus = "Unpaid"
        };

        _db.Bills.Add(bill);
        await _db.SaveChangesAsync();

        var result = await BaseQuery().FirstAsync(b => b.BillId == bill.BillId);
        return Ok(_mapper.Map<BillDto>(result));
    }

    [HttpPatch("{id}/payment")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdatePayment(int id, [FromBody] UpdatePaymentDto dto)
    {
        var bill = await _db.Bills.FindAsync(id);
        if (bill == null) return NotFound();

        bill.PaymentStatus = dto.PaymentStatus;
        bill.PaymentMethod = dto.PaymentMethod;
        if (dto.PaymentStatus == "Paid")
            bill.PaidAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Ok(new { message = "Payment updated." });
    }
}

// =============================================
// MEDICINES CONTROLLER
// =============================================
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MedicinesController : ControllerBase
{
    private readonly HospitalDbContext _db;
    private readonly IMapper _mapper;

    public MedicinesController(HospitalDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search)
    {
        var query = _db.Medicines.Where(m => m.IsActive).AsQueryable();
        if (!string.IsNullOrEmpty(search))
            query = query.Where(m => m.MedicineName.Contains(search));
        var meds = await query.OrderBy(m => m.MedicineName).ToListAsync();
        return Ok(_mapper.Map<List<MedicineDto>>(meds));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateMedicineDto dto)
    {
        var med = _mapper.Map<Medicine>(dto);
        _db.Medicines.Add(med);
        await _db.SaveChangesAsync();
        return Ok(_mapper.Map<MedicineDto>(med));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateMedicineDto dto)
    {
        var med = await _db.Medicines.FindAsync(id);
        if (med == null) return NotFound();
        med.MedicineName = dto.MedicineName;
        med.UnitPrice = dto.UnitPrice;
        med.Unit = dto.Unit;
        await _db.SaveChangesAsync();
        return Ok(_mapper.Map<MedicineDto>(med));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var med = await _db.Medicines.FindAsync(id);
        if (med == null) return NotFound();
        med.IsActive = false;
        await _db.SaveChangesAsync();
        return Ok(new { message = "Medicine removed." });
    }
}

// =============================================
// PATIENTS CONTROLLER
// =============================================
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PatientsController : ControllerBase
{
    private readonly HospitalDbContext _db;
    private readonly IMapper _mapper;

    public PatientsController(HospitalDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<IActionResult> GetAll()
    {
        var patients = await _db.PatientProfiles
            .Include(p => p.User)
            .Where(p => p.User.IsActive)
            .ToListAsync();
        return Ok(_mapper.Map<List<PatientProfileDto>>(patients));
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetById(int userId)
    {
        var profile = await _db.PatientProfiles
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.UserId == userId);
        if (profile == null) return NotFound();
        return Ok(_mapper.Map<PatientProfileDto>(profile));
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> Update(int userId, [FromBody] UpdatePatientProfileDto dto)
    {
        var profile = await _db.PatientProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        if (profile == null) return NotFound();

        profile.DateOfBirth = dto.DateOfBirth;
        profile.Gender = dto.Gender;
        profile.BloodGroup = dto.BloodGroup;
        profile.Address = dto.Address;
        profile.EmergencyContact = dto.EmergencyContact;
        profile.MedicalHistory = dto.MedicalHistory;

        await _db.SaveChangesAsync();
        return Ok(new { message = "Profile updated." });
    }
}
