using HealthCareSmart.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthCareSmart.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<DoctorSpecialization> DoctorSpecializations { get; set; }
        public DbSet<Billing> Billings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // One-to-One: User → Patient
            modelBuilder.Entity<User>()
                .HasOne(u => u.Patient)
                .WithOne(p => p.User)
                .HasForeignKey<Patient>(p => p.UserId);

            // One-to-Many: Doctor → Appointments
            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.Appointments)
                .WithOne(a => a.Doctor)
                .HasForeignKey(a => a.DoctorId);

            modelBuilder.Entity<Doctor>()
                .Property(d => d.ConsultationFee)
                .HasPrecision(18, 2);

            // Many-to-Many: Doctor ↔ DoctorSpecialization
            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.Specializations)
                .WithMany(s => s.Doctors);

            // One-to-Many: Prescription → Medicines
            modelBuilder.Entity<Prescription>()
                .HasMany(p => p.Medicines)
                .WithOne(m => m.Prescription)
                .HasForeignKey(m => m.PrescriptionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Billing>()
                .Property(b => b.ConsultationFee)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Billing>()
                .Property(b => b.MedicineCost)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Medicine>()
                .Property(m => m.Price)
                .HasPrecision(18, 2);

            // One-to-One: Appointment → Billing
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Billing)
                .WithOne(b => b.Appointment)
                .HasForeignKey<Billing>(b => b.AppointmentId);
        }
    }
}