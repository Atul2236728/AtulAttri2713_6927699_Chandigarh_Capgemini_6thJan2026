using System.ComponentModel.DataAnnotations;

namespace HospitalWeb.Models;

// =============================================
// AUTH
// =============================================
public class LoginViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }
}

public class RegisterViewModel
{
    [Required, Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Phone]
    public string? Phone { get; set; }
}

// =============================================
// SHARED API RESPONSE DTOS (mirror of API DTOs)
// =============================================
public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int? DoctorId { get; set; }
}

public class DepartmentDto
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string IconClass { get; set; } = "fa-stethoscope";
    public int DoctorCount { get; set; }
    public bool IsActive { get; set; }
}

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

public class PatientProfileDto
{
    public int ProfileId { get; set; }
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? BloodGroup { get; set; }
    public string? Address { get; set; }
    public string? EmergencyContact { get; set; }
    public string? MedicalHistory { get; set; }
}

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
    public string? FollowUpDate { get; set; }
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

public class MedicineDto
{
    public int MedicineId { get; set; }
    public string MedicineName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public string Unit { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

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

public class AdminDashboardDto
{
    public int TotalPatients { get; set; }
    public int TotalDoctors { get; set; }
    public int TotalAppointments { get; set; }
    public int TodayAppointments { get; set; }
    public decimal TotalRevenue { get; set; }
    public int PendingBills { get; set; }
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

// =============================================
// FORM VIEW MODELS
// =============================================
public class BookAppointmentViewModel
{
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public decimal ConsultationFee { get; set; }
    public string? Availability { get; set; }

    [Required]
    public DateTime AppointmentDate { get; set; } = DateTime.Today.AddDays(1);

    public string? TimeSlot { get; set; }

    [MaxLength(500)]
    public string? Reason { get; set; }
}

public class CreateDoctorViewModel
{
    [Required] public string FullName { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required, MinLength(6)] public string Password { get; set; } = string.Empty;
    public string? Phone { get; set; }
    [Required] public int DepartmentId { get; set; }
    [Required] public string Specialization { get; set; } = string.Empty;
    public int ExperienceYears { get; set; }
    public string? Availability { get; set; }
    [Required] public decimal ConsultationFee { get; set; }
    public string? Qualification { get; set; }
    public List<DepartmentDto> Departments { get; set; } = new();
}

public class CreatePrescriptionViewModel
{
    public int AppointmentId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string Diagnosis { get; set; } = string.Empty;
    public string? Medicines { get; set; }
    public string? Notes { get; set; }
    public string? FollowUpDate { get; set; }
    public List<MedicineDto> AllMedicines { get; set; } = new();
    public List<SelectedMedicineItem> SelectedMedicines { get; set; } = new();
}

public class SelectedMedicineItem
{
    public int MedicineId { get; set; }
    public int Quantity { get; set; } = 1;
    public string? Dosage { get; set; }
    public string? Duration { get; set; }
}
