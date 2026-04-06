using Microsoft.EntityFrameworkCore;
using SmartHealthcare.Models.Entities;

namespace SmartHealthcare.API.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext db)
    {
        await SeedSpecializationsAsync(db);   // ✅ MUST BE FIRST
        await SeedDoctorsAsync(db);
        await SeedPatientsAsync(db);
        await SeedAppointmentsAsync(db);
        await SeedPrescriptionsAsync(db);
    }

    /* ========================= */
    /* 🔥 SPECIALIZATIONS */
    /* ========================= */
    private static async Task SeedSpecializationsAsync(ApplicationDbContext db)
    {
        if (await db.Specializations.AnyAsync())
            return;

        var specializations = new List<Specialization>
        {
            new() { Name = "Cardiology" },
            new() { Name = "Dermatology" },
            new() { Name = "Neurology" },
            new() { Name = "Orthopedics" },
            new() { Name = "Pediatrics" },
            new() { Name = "General Medicine" }
        };

        await db.Specializations.AddRangeAsync(specializations);
        await db.SaveChangesAsync();
    }

    /* ========================= */
    /* 🔥 DOCTORS */
    /* ========================= */
    private static async Task SeedDoctorsAsync(ApplicationDbContext db)
    {
        if (await db.Doctors.AnyAsync())
            return;

        var doctorUsers = new List<User>
        {
            new() { FullName = "Dr. Aarav Mehta", Email = "doctor1@healthcare.com", PasswordHash = "123", Role = "Doctor" },
            new() { FullName = "Dr. Neha Iyer", Email = "doctor2@healthcare.com", PasswordHash = "123", Role = "Doctor" },
            new() { FullName = "Dr. Rohan Kapoor", Email = "doctor3@healthcare.com", PasswordHash = "123", Role = "Doctor" }
        };

        await db.Users.AddRangeAsync(doctorUsers);
        await db.SaveChangesAsync();

        var doctors = new List<Doctor>
        {
            new() { UserId = doctorUsers[0].Id, LicenseNumber = "LIC1", YearsOfExperience = 5, ConsultationFee = 500, Phone = "9999999991" },
            new() { UserId = doctorUsers[1].Id, LicenseNumber = "LIC2", YearsOfExperience = 8, ConsultationFee = 700, Phone = "9999999992" },
            new() { UserId = doctorUsers[2].Id, LicenseNumber = "LIC3", YearsOfExperience = 10, ConsultationFee = 900, Phone = "9999999993" }
        };

        await db.Doctors.AddRangeAsync(doctors);
        await db.SaveChangesAsync();

        var specs = await db.Specializations.ToListAsync();

        var doctorSpecializations = new List<DoctorSpecialization>
        {
            new() { DoctorId = doctors[0].Id, SpecializationId = specs[0].Id },
            new() { DoctorId = doctors[1].Id, SpecializationId = specs[1].Id },
            new() { DoctorId = doctors[2].Id, SpecializationId = specs[2].Id }
        };

        await db.DoctorSpecializations.AddRangeAsync(doctorSpecializations);
        await db.SaveChangesAsync();
    }

    /* ========================= */
    /* 🔥 PATIENTS */
    /* ========================= */
    private static async Task SeedPatientsAsync(ApplicationDbContext db)
    {
        if (await db.Patients.AnyAsync())
            return;

        var users = new List<User>
        {
            new() { FullName = "Raj Patel", Email = "patient1@test.com", PasswordHash = "123", Role = "Patient" },
            new() { FullName = "Amit Kumar", Email = "patient2@test.com", PasswordHash = "123", Role = "Patient" }
        };

        await db.Users.AddRangeAsync(users);
        await db.SaveChangesAsync();

        var patients = new List<Patient>
        {
            new() { UserId = users[0].Id, Phone = "8888888881" },
            new() { UserId = users[1].Id, Phone = "8888888882" }
        };

        await db.Patients.AddRangeAsync(patients);
        await db.SaveChangesAsync();
    }

    /* ========================= */
    /* 🔥 APPOINTMENTS */
    /* ========================= */
    private static async Task SeedAppointmentsAsync(ApplicationDbContext db)
    {
        if (await db.Appointments.AnyAsync())
            return;

        var patients = await db.Patients.ToListAsync();
        var doctors = await db.Doctors.ToListAsync();

        if (!patients.Any() || !doctors.Any())
            return;

        var appointments = new List<Appointment>
        {
            new()
            {
                PatientId = patients[0].Id,
                DoctorId = doctors[0].Id,
                AppointmentDate = DateTime.UtcNow.AddDays(2),
                Status = "Pending"
            }
        };

        await db.Appointments.AddRangeAsync(appointments);
        await db.SaveChangesAsync();
    }

    /* ========================= */
    /* 🔥 PRESCRIPTIONS */
    /* ========================= */
    private static async Task SeedPrescriptionsAsync(ApplicationDbContext db)
    {
        if (await db.Prescriptions.AnyAsync())
            return;

        var appointment = await db.Appointments.FirstOrDefaultAsync();
        if (appointment == null) return;

        var prescription = new Prescription
        {
            AppointmentId = appointment.Id,
            Diagnosis = "General Checkup",
            CreatedAt = DateTime.UtcNow
        };

        await db.Prescriptions.AddAsync(prescription);
        await db.SaveChangesAsync();
    }
}