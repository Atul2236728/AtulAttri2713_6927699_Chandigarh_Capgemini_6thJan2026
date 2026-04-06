using AutoMapper;
using HealthCareSmart.Core.DTOs;
using HealthCareSmart.Core.Models;

namespace HealthCareSmart.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Doctor, DoctorDTO>()
                .ForMember(dest => dest.Specializations,
                           opt => opt.MapFrom(src => src.Specializations.Select(s => s.Name).ToList()));

            CreateMap<Appointment, AppointmentDTO>()
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.FullName))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.User.FullName))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.PrescriptionInstructions, opt => opt.MapFrom(src => src.Prescription != null ? src.Prescription.Instructions : string.Empty))
                .ForMember(dest => dest.PrescriptionMedicines, opt => opt.MapFrom(src => src.Prescription != null
                    ? string.Join(", ", src.Prescription.Medicines.Select(m => $"{m.Name} ({m.Dosage})"))
                    : string.Empty));

            CreateMap<Billing, BillingDTO>()
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Appointment.Doctor.FullName))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Appointment.Patient.User.FullName))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount));

            CreateMap<CreateAppointmentDTO, Appointment>();

        }
    }
}