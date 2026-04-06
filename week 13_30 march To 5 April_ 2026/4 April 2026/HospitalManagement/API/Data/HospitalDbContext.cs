using Microsoft.EntityFrameworkCore;
using HospitalAPI.Models;

namespace HospitalAPI.Data;

public class HospitalDbContext : DbContext
{
    public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<PatientProfile> PatientProfiles => Set<PatientProfile>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Prescription> Prescriptions => Set<Prescription>();
    public DbSet<Medicine> Medicines => Set<Medicine>();
    public DbSet<PrescriptionMedicine> PrescriptionMedicines => Set<PrescriptionMedicine>();
    public DbSet<Bill> Bills => Set<Bill>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        // User
        mb.Entity<User>().HasIndex(u => u.Email).IsUnique();

        // Doctor - User (1:1)
        mb.Entity<Doctor>()
            .HasOne(d => d.User)
            .WithOne(u => u.Doctor)
            .HasForeignKey<Doctor>(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Doctor - Department (M:1)
        mb.Entity<Doctor>()
            .HasOne(d => d.Department)
            .WithMany(dep => dep.Doctors)
            .HasForeignKey(d => d.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        // PatientProfile - User (1:1)
        mb.Entity<PatientProfile>()
            .HasOne(pp => pp.User)
            .WithOne(u => u.PatientProfile)
            .HasForeignKey<PatientProfile>(pp => pp.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Appointment - Patient (M:1)
        mb.Entity<Appointment>()
            .HasOne(a => a.Patient)
            .WithMany(u => u.Appointments)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        // Appointment - Doctor (M:1)
        mb.Entity<Appointment>()
            .HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Prescription - Appointment (1:1)
        mb.Entity<Prescription>()
            .HasOne(p => p.Appointment)
            .WithOne(a => a.Prescription)
            .HasForeignKey<Prescription>(p => p.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);

        mb.Entity<Prescription>()
            .HasIndex(p => p.AppointmentId)
            .IsUnique();

        // Bill - Appointment (1:1)
        mb.Entity<Bill>()
            .HasOne(b => b.Appointment)
            .WithOne(a => a.Bill)
            .HasForeignKey<Bill>(b => b.AppointmentId)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<Bill>()
            .HasIndex(b => b.AppointmentId)
            .IsUnique();

        // Bill computed column
        mb.Entity<Bill>()
            .Property(b => b.TotalAmount)
            .HasComputedColumnSql("[ConsultationFee] + [MedicineCharges] + [OtherCharges] - [Discount]", stored: true);

        // PrescriptionMedicine
        mb.Entity<PrescriptionMedicine>()
            .HasOne(pm => pm.Prescription)
            .WithMany(p => p.PrescriptionMedicines)
            .HasForeignKey(pm => pm.PrescriptionId)
            .OnDelete(DeleteBehavior.Cascade);

        mb.Entity<PrescriptionMedicine>()
            .HasOne(pm => pm.Medicine)
            .WithMany()
            .HasForeignKey(pm => pm.MedicineId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
