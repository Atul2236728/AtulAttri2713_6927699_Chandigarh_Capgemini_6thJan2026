using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.DTOs;

// =============================================
// AUTH DTOs
// =============================================
public class LoginDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

public class RegisterDto
{
    [Required, MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [MaxLength(15)]
    public string? Phone { get; set; }

    public string Role { get; set; } = "Patient";
}

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int? DoctorId { get; set; }
}

// =============================================
// USER DTOs
// =============================================
public class UserDto
{
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Role { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class UpdateUserDto
{
    [Required, MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(15)]
    public string? Phone { get; set; }
}

// =============================================
// DEPARTMENT DTOs
// =============================================
public class DepartmentDto
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string IconClass { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int DoctorCount { get; set; }
}

public class CreateDepartmentDto
{
    [Required, MaxLength(100)]
    public string DepartmentName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public string IconClass { get; set; } = "fa-stethoscope";
}

// =============================================
// DOCTOR DTOs
// =============================================
public class DoctorDto
{
    public int DoctorId { get; set; }
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public int ExperienceYears { get; set; }
    public string? Availability { get; set; }
    public decimal ConsultationFee { get; set; }
    public string? Qualification { get; set; }
    public string ProfileImage { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
}

public class CreateDoctorDto
{
    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;

    public string? Phone { get; set; }

    [Required]
    public int DepartmentId { get; set; }

    [Required]
    public string Specialization { get; set; } = string.Empty;

    public int ExperienceYears { get; set; }

    public string? Availability { get; set; }

    [Required]
    public decimal ConsultationFee { get; set; }

    public string? Qualification { get; set; }
}

public class UpdateDoctorDto
{
    public int DepartmentId { get; set; }
    public string Specialization { get; set; } = string.Empty;
    public int ExperienceYears { get; set; }
    public string? Availability { get; set; }
    public decimal ConsultationFee { get; set; }
    public string? Qualification { get; set; }
    public bool IsAvailable { get; set; }
}

// =============================================
// PATIENT PROFILE DTOs
// =============================================
public class PatientProfileDto
{
    public int ProfileId { get; set; }
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? BloodGroup { get; set; }
    public string? Address { get; set; }
    public string? EmergencyContact { get; set; }
    public string? MedicalHistory { get; set; }
}

public class UpdatePatientProfileDto
{
    public DateOnly? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? BloodGroup { get; set; }
    public string? Address { get; set; }
    public string? EmergencyContact { get; set; }
    public string? MedicalHistory { get; set; }
}

// =============================================
// APPOINTMENT DTOs
// =============================================
public class AppointmentDto
{
    public int AppointmentId { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string PatientEmail { get; set; } = string.Empty;
    public string? PatientPhone { get; set; }
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public decimal ConsultationFee { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string? TimeSlot { get; set; }
    public string? Reason { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool HasPrescription { get; set; }
    public bool HasBill { get; set; }
}

public class BookAppointmentDto
{
    [Required]
    public int DoctorId { get; set; }

    [Required]
    public DateTime AppointmentDate { get; set; }

    public string? TimeSlot { get; set; }

    [MaxLength(500)]
    public string? Reason { get; set; }
}

public class UpdateAppointmentStatusDto
{
    [Required]
    public string Status { get; set; } = string.Empty;

    public string? Notes { get; set; }
}

// =============================================
// PRESCRIPTION DTOs
// =============================================
public class PrescriptionDto
{
    public int PrescriptionId { get; set; }
    public int AppointmentId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string DoctorName { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string? Medicines { get; set; }
    public string? Notes { get; set; }
    public DateOnly? FollowUpDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<PrescriptionMedicineDto> PrescriptionMedicines { get; set; } = new();
}

public class PrescriptionMedicineDto
{
    public int Id { get; set; }
    public int MedicineId { get; set; }
    public string MedicineName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public string? Dosage { get; set; }
    public string? Duration { get; set; }
    public decimal TotalPrice => UnitPrice * Quantity;
}

public class CreatePrescriptionDto
{
    [Required]
    public int AppointmentId { get; set; }

    [Required]
    public string Diagnosis { get; set; } = string.Empty;

    public string? Medicines { get; set; }

    public string? Notes { get; set; }

    public DateOnly? FollowUpDate { get; set; }

    public List<AddPrescriptionMedicineDto> PrescriptionMedicines { get; set; } = new();
}

public class AddPrescriptionMedicineDto
{
    public int MedicineId { get; set; }
    public int Quantity { get; set; } = 1;
    public string? Dosage { get; set; }
    public string? Duration { get; set; }
}

// =============================================
// MEDICINE DTOs
// =============================================
public class MedicineDto
{
    public int MedicineId { get; set; }
    public string MedicineName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public string Unit { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class CreateMedicineDto
{
    [Required]
    public string MedicineName { get; set; } = string.Empty;
    [Required]
    public decimal UnitPrice { get; set; }
    public string Unit { get; set; } = "Tablet";
}

// =============================================
// BILL DTOs
// =============================================
public class BillDto
{
    public int BillId { get; set; }
    public int AppointmentId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string DoctorName { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    public decimal ConsultationFee { get; set; }
    public decimal MedicineCharges { get; set; }
    public decimal OtherCharges { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public string? PaymentMethod { get; set; }
    public DateTime? PaidAt { get; set; }
    public DateTime GeneratedAt { get; set; }
}

public class GenerateBillDto
{
    [Required]
    public int AppointmentId { get; set; }

    [Required]
    public decimal ConsultationFee { get; set; }

    public decimal MedicineCharges { get; set; } = 0;

    public decimal OtherCharges { get; set; } = 0;

    public decimal Discount { get; set; } = 0;
}

public class UpdatePaymentDto
{
    [Required]
    public string PaymentStatus { get; set; } = string.Empty;

    public string? PaymentMethod { get; set; }
}

// =============================================
// DASHBOARD
// =============================================
public class AdminDashboardDto
{
    public int TotalPatients { get; set; }
    public int TotalDoctors { get; set; }
    public int TotalAppointments { get; set; }
    public int TodayAppointments { get; set; }
    public decimal TotalRevenue { get; set; }
    public int PendingBills { get; set; }
    public List<DepartmentDto> Departments { get; set; } = new();
    public List<AppointmentDto> RecentAppointments { get; set; } = new();
}

public class DoctorDashboardDto
{
    public int TodayAppointments { get; set; }
    public int TotalPatients { get; set; }
    public int PendingAppointments { get; set; }
    public int CompletedToday { get; set; }
    public List<AppointmentDto> TodaySchedule { get; set; } = new();
}
