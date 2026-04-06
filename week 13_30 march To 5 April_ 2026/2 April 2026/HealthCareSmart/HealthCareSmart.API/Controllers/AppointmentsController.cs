using AutoMapper;
using HealthCareSmart.API.Data;
using HealthCareSmart.Core.DTOs;
using HealthCareSmart.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthCareSmart.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AppointmentsController> _logger;

        public AppointmentsController(AppDbContext context, IMapper mapper,
                                      ILogger<AppointmentsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/appointments — Any logged in user can view
        [Authorize(Roles = "Admin,Doctor,Patient")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var appointments = await _context.Appointments
                .AsNoTracking()
                .Select(a => new AppointmentDTO
                {
                    Id = a.Id,
                    AppointmentDate = a.AppointmentDate,
                    Status = a.Status,
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.FullName,
                    PatientName = a.Patient.User.FullName,
                    Notes = a.Notes,
                    PrescriptionInstructions = a.Prescription != null ? a.Prescription.Instructions : string.Empty,
                    PrescriptionMedicines = a.Prescription != null && a.Prescription.Medicines.Any()
                        ? string.Join(", ", a.Prescription.Medicines.Select(m => m.Name))
                        : string.Empty
                })
                .ToListAsync();

            return Ok(appointments);
        }

        // GET: api/appointments/5
        [Authorize(Roles = "Admin,Doctor,Patient")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var appointment = await _context.Appointments
                .AsNoTracking()
                .Where(a => a.Id == id)
                .Select(a => new AppointmentDTO
                {
                    Id = a.Id,
                    AppointmentDate = a.AppointmentDate,
                    Status = a.Status,
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.FullName,
                    PatientName = a.Patient.User.FullName,
                    Notes = a.Notes,
                    PrescriptionInstructions = a.Prescription != null ? a.Prescription.Instructions : string.Empty,
                    PrescriptionMedicines = a.Prescription != null && a.Prescription.Medicines.Any()
                        ? string.Join(", ", a.Prescription.Medicines.Select(m => m.Name))
                        : string.Empty
                })
                .FirstOrDefaultAsync();

            if (appointment == null)
                return NotFound(new { message = "Appointment not found" });

            return Ok(appointment);
        }

        // POST: api/appointments — Only Patients can book
        [Authorize(Roles = "Patient")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(ms => ms.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray());

                _logger.LogWarning("Invalid appointment create model state: {Errors}", errors);
                return BadRequest(new { message = "Invalid appointment request.", errors });
            }

            var patientExists = await _context.Patients.AnyAsync(p => p.Id == dto.PatientId);
            if (!patientExists)
                return BadRequest(new { message = "Patient not found." });

            var doctorExists = await _context.Doctors.AnyAsync(d => d.Id == dto.DoctorId);
            if (!doctorExists)
                return BadRequest(new { message = "Doctor not found." });

            var appointment = _mapper.Map<Appointment>(dto);
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            _logger.LogInformation("New appointment booked. AppointmentId: {Id}", appointment.Id);
            return CreatedAtAction(nameof(GetById), new { id = appointment.Id }, appointment);
        }

        // PUT: api/appointments/5 — Doctor or Admin can update
        [Authorize(Roles = "Doctor,Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateAppointmentDTO dto)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            appointment.AppointmentDate = dto.AppointmentDate;
            appointment.DoctorId = dto.DoctorId;
            appointment.Notes = dto.Notes;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Updated successfully" });
        }

        // POST: api/appointments/5/prescription — Doctor or Admin creates prescription and billing
        [Authorize(Roles = "Doctor,Admin")]
        [HttpPost("{id:int}/prescription")]
        public async Task<IActionResult> AddOrUpdatePrescription(int id, [FromBody] CreatePrescriptionDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient).ThenInclude(p => p.User)
                .Include(a => a.Prescription).ThenInclude(p => p.Medicines)
                .Include(a => a.Billing)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
                return NotFound(new { message = "Appointment not found" });

            if (appointment.Prescription == null)
            {
                appointment.Prescription = new Prescription
                {
                    AppointmentId = appointment.Id,
                    Instructions = dto.Instructions,
                    IssuedDate = DateTime.UtcNow,
                    Medicines = dto.Medicines.Select(m => new Medicine
                    {
                        Name = m.Name,
                        Dosage = m.Dosage,
                        Price = m.Price
                    }).ToList()
                };
                _context.Prescriptions.Add(appointment.Prescription);
            }
            else
            {
                appointment.Prescription.Instructions = dto.Instructions;

                if (appointment.Prescription.Medicines.Any())
                {
                    _context.Medicines.RemoveRange(appointment.Prescription.Medicines);
                }

                appointment.Prescription.Medicines = dto.Medicines.Select(m => new Medicine
                {
                    Name = m.Name,
                    Dosage = m.Dosage,
                    Price = m.Price,
                    Prescription = appointment.Prescription
                }).ToList();
            }

            var medicineCost = appointment.Prescription.Medicines.Sum(m => m.Price);
            var consultationFee = appointment.Doctor.ConsultationFee;

            if (appointment.Billing == null)
            {
                appointment.Billing = new Billing
                {
                    AppointmentId = appointment.Id,
                    ConsultationFee = consultationFee,
                    MedicineCost = medicineCost,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Billings.Add(appointment.Billing);
            }
            else
            {
                appointment.Billing.ConsultationFee = consultationFee;
                appointment.Billing.MedicineCost = medicineCost;
            }

            appointment.Status = "Completed";
            await _context.SaveChangesAsync();

            var bill = new BillingDTO
            {
                Id = appointment.Billing.Id,
                AppointmentId = appointment.Id,
                DoctorName = appointment.Doctor.FullName,
                PatientName = appointment.Patient.User.FullName,
                ConsultationFee = appointment.Billing.ConsultationFee,
                MedicineCost = appointment.Billing.MedicineCost,
                TotalAmount = appointment.Billing.TotalAmount,
                CreatedAt = appointment.Billing.CreatedAt
            };

            return Ok(bill);
        }

        // PATCH: api/appointments/5/status — Doctor updates status
        [Authorize(Roles = "Doctor")]
        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            appointment.Status = status;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Status updated" });
        }

        // DELETE: api/appointments/5 — Only Admin can delete
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Deleted successfully" });
        }

        // GET: api/appointments/search?date=2026-04-01
        [Authorize(Roles = "Admin,Doctor")]
        [HttpGet("search")]
        public async Task<IActionResult> SearchByDate([FromQuery] DateTime date)
        {
            var appointments = await _context.Appointments
                .Where(a => a.AppointmentDate.Date == date.Date)
                .Include(a => a.Doctor)
                .ToListAsync();

            return Ok(_mapper.Map<List<AppointmentDTO>>(appointments));
        }

        // GET: api/appointments/{id}/prescription — Patient views prescription details
        [HttpGet("{id:int}/prescription")]
        public async Task<IActionResult> GetPrescription(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Prescription).ThenInclude(p => p.Medicines)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
                return NotFound(new { message = "Appointment not found" });

            if (appointment.Prescription == null)
                return NotFound(new { message = "No prescription yet for this appointment" });

            var prescriptionDTO = new
            {
                Id = appointment.Prescription.Id,
                Instructions = appointment.Prescription.Instructions,
                IssuedDate = appointment.Prescription.IssuedDate,
                AppointmentId = appointment.Prescription.AppointmentId,
                DoctorName = appointment.Doctor.FullName,
                Medicines = appointment.Prescription.Medicines.Select(m => new
                {
                    Id = m.Id,
                    Name = m.Name,
                    Dosage = m.Dosage,
                    Price = m.Price
                }).ToList()
            };

            return Ok(prescriptionDTO);
        }

        // PUT: api/appointments/{id}/pay — Mark appointment as paid/completed
        [HttpPut("{id:int}/pay")]
        public async Task<IActionResult> MarkAsPaid(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
                return NotFound(new { message = "Appointment not found" });

            appointment.Status = "Completed";
            await _context.SaveChangesAsync();

            return Ok(new { message = "Payment successful. Appointment marked as completed." });
        }
    }
}