using HealthCareSmart.API.Data;
using HealthCareSmart.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HealthCareSmart.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BillingsController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Doctor,Patient")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = _context.Billings
                .Include(b => b.Appointment).ThenInclude(a => a.Doctor)
                .Include(b => b.Appointment).ThenInclude(a => a.Patient).ThenInclude(p => p.User)
                .AsQueryable();

            if (User.IsInRole("Patient"))
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(userIdClaim, out var userId))
                {
                    query = query.Where(b => b.Appointment.Patient.UserId == userId);
                }
            }

            var bills = await query
                .Select(b => new BillingDTO
                {
                    Id = b.Id,
                    AppointmentId = b.AppointmentId,
                    DoctorName = b.Appointment.Doctor.FullName,
                    PatientName = b.Appointment.Patient.User.FullName,
                    ConsultationFee = b.ConsultationFee,
                    MedicineCost = b.MedicineCost,
                    TotalAmount = b.TotalAmount,
                    CreatedAt = b.CreatedAt
                })
                .ToListAsync();

            return Ok(bills);
        }

        [Authorize(Roles = "Admin,Doctor,Patient")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var billing = await _context.Billings
                .Include(b => b.Appointment).ThenInclude(a => a.Doctor)
                .Include(b => b.Appointment).ThenInclude(a => a.Patient).ThenInclude(p => p.User)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (billing == null)
                return NotFound(new { message = "Billing record not found" });

            return Ok(new BillingDTO
            {
                Id = billing.Id,
                AppointmentId = billing.AppointmentId,
                DoctorName = billing.Appointment.Doctor.FullName,
                PatientName = billing.Appointment.Patient.User.FullName,
                ConsultationFee = billing.ConsultationFee,
                MedicineCost = billing.MedicineCost,
                TotalAmount = billing.TotalAmount,
                CreatedAt = billing.CreatedAt
            });
        }
    }
}
