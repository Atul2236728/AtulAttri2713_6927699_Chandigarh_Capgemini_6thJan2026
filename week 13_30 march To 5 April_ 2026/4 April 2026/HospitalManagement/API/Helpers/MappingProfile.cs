using AutoMapper;
using HospitalAPI.DTOs;
using HospitalAPI.Models;

namespace HospitalAPI.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User
        CreateMap<User, UserDto>();
        CreateMap<RegisterDto, User>()
            .ForMember(d => d.PasswordHash, o => o.Ignore());

        // Department
        CreateMap<Department, DepartmentDto>()
            .ForMember(d => d.DoctorCount, o => o.MapFrom(s => s.Doctors.Count(x => x.IsAvailable)));
        CreateMap<CreateDepartmentDto, Department>();

        // Doctor
        CreateMap<Doctor, DoctorDto>()
            .ForMember(d => d.FullName, o => o.MapFrom(s => s.User.FullName))
            .ForMember(d => d.Email, o => o.MapFrom(s => s.User.Email))
            .ForMember(d => d.Phone, o => o.MapFrom(s => s.User.Phone))
            .ForMember(d => d.DepartmentName, o => o.MapFrom(s => s.Department.DepartmentName));

        // PatientProfile
        CreateMap<PatientProfile, PatientProfileDto>()
            .ForMember(d => d.FullName, o => o.MapFrom(s => s.User.FullName))
            .ForMember(d => d.Email, o => o.MapFrom(s => s.User.Email))
            .ForMember(d => d.Phone, o => o.MapFrom(s => s.User.Phone));

        // Appointment
        CreateMap<Appointment, AppointmentDto>()
            .ForMember(d => d.PatientName, o => o.MapFrom(s => s.Patient.FullName))
            .ForMember(d => d.PatientEmail, o => o.MapFrom(s => s.Patient.Email))
            .ForMember(d => d.PatientPhone, o => o.MapFrom(s => s.Patient.Phone))
            .ForMember(d => d.DoctorName, o => o.MapFrom(s => s.Doctor.User.FullName))
            .ForMember(d => d.DepartmentName, o => o.MapFrom(s => s.Doctor.Department.DepartmentName))
            .ForMember(d => d.ConsultationFee, o => o.MapFrom(s => s.Doctor.ConsultationFee))
            .ForMember(d => d.HasPrescription, o => o.MapFrom(s => s.Prescription != null))
            .ForMember(d => d.HasBill, o => o.MapFrom(s => s.Bill != null));

        // Prescription
        CreateMap<Prescription, PrescriptionDto>()
            .ForMember(d => d.PatientName, o => o.MapFrom(s => s.Appointment.Patient.FullName))
            .ForMember(d => d.DoctorName, o => o.MapFrom(s => s.Appointment.Doctor.User.FullName))
            .ForMember(d => d.AppointmentDate, o => o.MapFrom(s => s.Appointment.AppointmentDate));

        CreateMap<PrescriptionMedicine, PrescriptionMedicineDto>()
            .ForMember(d => d.MedicineName, o => o.MapFrom(s => s.Medicine.MedicineName))
            .ForMember(d => d.UnitPrice, o => o.MapFrom(s => s.Medicine.UnitPrice));

        // Medicine
        CreateMap<Medicine, MedicineDto>();
        CreateMap<CreateMedicineDto, Medicine>();

        // Bill
        CreateMap<Bill, BillDto>()
            .ForMember(d => d.PatientName, o => o.MapFrom(s => s.Appointment.Patient.FullName))
            .ForMember(d => d.DoctorName, o => o.MapFrom(s => s.Appointment.Doctor.User.FullName))
            .ForMember(d => d.AppointmentDate, o => o.MapFrom(s => s.Appointment.AppointmentDate));
    }
}
