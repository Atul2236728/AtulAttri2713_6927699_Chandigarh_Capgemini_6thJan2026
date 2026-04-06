using AutoMapper;
using HealthCareSmart.API.Data;
using HealthCareSmart.Core.DTOs;
using HealthCareSmart.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace HealthCareSmart.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly ILogger<DoctorsController> _logger;
        private readonly IMapper _mapper;

        public DoctorsController(AppDbContext context, IMemoryCache cache,
                                 ILogger<DoctorsController> logger, IMapper mapper)
        {
            _context = context;
            _cache = cache;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: api/doctors — Cached, open to all logged in users
        [Authorize(Roles = "Admin,Doctor,Patient")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            const string cacheKey = "doctors_list";

            if (!_cache.TryGetValue(cacheKey, out List<DoctorDTO>? doctors))
            {
                _logger.LogInformation("Cache miss — fetching doctors from database");

                doctors = await _context.Doctors
                    .Include(d => d.Specializations)
                    .AsNoTracking()
                    .Select(d => new DoctorDTO
                    {
                        Id = d.Id,
                        FullName = d.FullName,
                        PhoneNumber = d.PhoneNumber,
                        ConsultationFee = d.ConsultationFee,
                        Specializations = d.Specializations.Select(s => s.Name).ToList()
                    })
                    .ToListAsync();

                _cache.Set(cacheKey, doctors, TimeSpan.FromMinutes(10));
            }
            else
            {
                _logger.LogInformation("Cache hit — returning doctors from cache");
            }

            return Ok(doctors);
        }

        // GET: api/doctors/5
        [Authorize(Roles = "Admin,Doctor,Patient")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var doctor = await _context.Doctors
                .Include(d => d.Specializations)
                .AsNoTracking()
                .Where(d => d.Id == id)
                .Select(d => new DoctorDTO
                {
                    Id = d.Id,
                    FullName = d.FullName,
                    PhoneNumber = d.PhoneNumber,
                    ConsultationFee = d.ConsultationFee,
                    Specializations = d.Specializations.Select(s => s.Name).ToList()
                })
                .FirstOrDefaultAsync();

            if (doctor == null)
                return NotFound(new { message = "Doctor not found" });

            return Ok(doctor);
        }

        // GET: api/doctors/search?specialization=Cardiology
        [Authorize(Roles = "Admin,Doctor,Patient")]
        [HttpGet("search")]
        public async Task<IActionResult> SearchBySpecialization([FromQuery] string specialization)
        {
            if (string.IsNullOrWhiteSpace(specialization))
                return await GetAll();

            var normalized = specialization.Trim();
            if (normalized == "*" || string.Equals(normalized, "all", StringComparison.OrdinalIgnoreCase))
                return await GetAll();

            var searchKeys = normalized
                .Split(new[] { ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(key => key.Trim())
                .Where(key => !string.IsNullOrWhiteSpace(key))
                .ToList();

            if (!searchKeys.Any())
                return await GetAll();

            try
            {
                var doctors = await _context.Doctors
                    .Include(d => d.Specializations)
                    .AsNoTracking()
                    .Where(d => d.Specializations.Any(s => searchKeys.Contains(s.Name)))
                    .Select(d => new DoctorDTO
                    {
                        Id = d.Id,
                        FullName = d.FullName,
                        PhoneNumber = d.PhoneNumber,
                        ConsultationFee = d.ConsultationFee,
                        Specializations = d.Specializations.Select(s => s.Name).ToList()
                    })
                    .ToListAsync();

                if (!doctors.Any())
                {
                    doctors = await _context.Doctors
                        .Include(d => d.Specializations)
                        .AsNoTracking()
                        .Where(d => d.Specializations.Any(s => searchKeys.Any(key => EF.Functions.Like(s.Name, "%" + key + "%"))))
                        .Select(d => new DoctorDTO
                        {
                            Id = d.Id,
                            FullName = d.FullName,
                            PhoneNumber = d.PhoneNumber,
                            ConsultationFee = d.ConsultationFee,
                            Specializations = d.Specializations.Select(s => s.Name).ToList()
                        })
                        .ToListAsync();
                }

                return Ok(doctors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching doctors by specialization '{Specialization}'", specialization);
                return StatusCode(500, new { message = "Unable to search doctors by specialization right now." });
            }
        }

        // POST: api/doctors — Only Admin can add doctors
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Doctor doctor)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            // Clear cache so next GET fetches fresh data
            _cache.Remove("doctors_list");

            return CreatedAtAction(nameof(GetById), new { id = doctor.Id }, doctor);
        }

        // PUT: api/doctors/5 — Only Admin can update
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Doctor updated)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) return NotFound();

            doctor.FullName = updated.FullName;
            doctor.PhoneNumber = updated.PhoneNumber;

            await _context.SaveChangesAsync();
            _cache.Remove("doctors_list");

            return Ok(new { message = "Doctor updated successfully" });
        }

        // DELETE: api/doctors/5 — Only Admin
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) return NotFound();

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            _cache.Remove("doctors_list");

            return Ok(new { message = "Doctor deleted successfully" });
        }
    }
}