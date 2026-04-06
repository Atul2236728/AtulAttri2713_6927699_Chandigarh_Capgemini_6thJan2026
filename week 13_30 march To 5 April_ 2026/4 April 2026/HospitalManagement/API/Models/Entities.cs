using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Models;

// =============================================
// USER
// =============================================
public class User
{
    [Key]
    public int UserId { get; set; }

    [Required, MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required, MaxLength(100), EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(255)]
    public string PasswordHash { get; set; } = string.Empty;

    [MaxLength(15)]
    public string? Phone { get; set; }

    [Required, MaxLength(20)]
    public string Role { get; set; } = "Patient"; // Admin | Doctor | Patient

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Doctor? Doctor { get; set; }
    public PatientProfile? PatientProfile { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}

// =============================================
// DEPARTMENT
// =============================================
public class Department
{
    [Key]
    public int DepartmentId { get; set; }

    [Required, MaxLength(100)]
    public string DepartmentName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(50)]
    public string IconClass { get; set; } = "fa-stethoscope";

    public bool IsActive { get; set; } = true;

    public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}

// =============================================
// DOCTOR
// =============================================
public class Doctor
{
    [Key]
    public int DoctorId { get; set; }

    public int UserId { get; set; }
    public int DepartmentId { get; set; }

    [Required, MaxLength(100)]
    public string Specialization { get; set; } = string.Empty;

    public int ExperienceYears { get; set; }

    [MaxLength(200)]
    public string? Availability { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal ConsultationFee { get; set; } = 500m;

    [MaxLength(200)]
    public string? Qualification { get; set; }

    [MaxLength(255)]
    public string ProfileImage { get; set; } = "default-doctor.png";

    public bool IsAvailable { get; set; } = true;

    // Navigation
    [ForeignKey("UserId")]
    public User User { get; set; } = null!;

    [ForeignKey("DepartmentId")]
    public Department Department { get; set; } = null!;

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}

// =============================================
// PATIENT PROFILE
// =============================================
public class PatientProfile
{
    [Key]
    public int ProfileId { get; set; }

    public int UserId { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    [MaxLength(10)]
    public string? Gender { get; set; }

    [MaxLength(5)]
    public string? BloodGroup { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    [MaxLength(15)]
    public string? EmergencyContact { get; set; }

    public string? MedicalHistory { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; } = null!;
}

// =============================================
// APPOINTMENT
// =============================================
public class Appointment
{
    [Key]
    public int AppointmentId { get; set; }

    public int PatientId { get; set; }
    public int DoctorId { get; set; }

    [Required]
    public DateTime AppointmentDate { get; set; }

    [MaxLength(20)]
    public string? TimeSlot { get; set; }

    [MaxLength(500)]
    public string? Reason { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "Booked";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(500)]
    public string? Notes { get; set; }

    [ForeignKey("PatientId")]
    public User Patient { get; set; } = null!;

    [ForeignKey("DoctorId")]
    public Doctor Doctor { get; set; } = null!;

    // One-to-One
    public Prescription? Prescription { get; set; }
    public Bill? Bill { get; set; }
}

// =============================================
// PRESCRIPTION
// =============================================
public class Prescription
{
    [Key]
    public int PrescriptionId { get; set; }

    public int AppointmentId { get; set; }

    [Required, MaxLength(500)]
    public string Diagnosis { get; set; } = string.Empty;

    public string? Medicines { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    public DateOnly? FollowUpDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("AppointmentId")]
    public Appointment Appointment { get; set; } = null!;

    public ICollection<PrescriptionMedicine> PrescriptionMedicines { get; set; } = new List<PrescriptionMedicine>();
}

// =============================================
// MEDICINE
// =============================================
public class Medicine
{
    [Key]
    public int MedicineId { get; set; }

    [Required, MaxLength(100)]
    public string MedicineName { get; set; } = string.Empty;

    [Column(TypeName = "decimal(10,2)")]
    public decimal UnitPrice { get; set; }

    [MaxLength(30)]
    public string Unit { get; set; } = "Tablet";

    public bool IsActive { get; set; } = true;
}

// =============================================
// PRESCRIPTION MEDICINE (junction)
// =============================================
public class PrescriptionMedicine
{
    [Key]
    public int Id { get; set; }

    public int PrescriptionId { get; set; }
    public int MedicineId { get; set; }
    public int Quantity { get; set; } = 1;

    [MaxLength(100)]
    public string? Dosage { get; set; }

    [MaxLength(50)]
    public string? Duration { get; set; }

    [ForeignKey("PrescriptionId")]
    public Prescription Prescription { get; set; } = null!;

    [ForeignKey("MedicineId")]
    public Medicine Medicine { get; set; } = null!;
}

// =============================================
// BILL
// =============================================
public class Bill
{
    [Key]
    public int BillId { get; set; }

    public int AppointmentId { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal ConsultationFee { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal MedicineCharges { get; set; } = 0;

    [Column(TypeName = "decimal(10,2)")]
    public decimal OtherCharges { get; set; } = 0;

    [Column(TypeName = "decimal(10,2)")]
    public decimal Discount { get; set; } = 0;

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalAmount { get; set; }

    [MaxLength(20)]
    public string PaymentStatus { get; set; } = "Unpaid";

    [MaxLength(30)]
    public string? PaymentMethod { get; set; }

    public DateTime? PaidAt { get; set; }

    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("AppointmentId")]
    public Appointment Appointment { get; set; } = null!;
}
