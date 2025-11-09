using AutoMapper;
using Fin_X.Domain;
using Fin_X.Dto;

namespace Porter.Application.Mapping
{
    public class PatientProfile : Profile
    {
        public PatientProfile()
        {
            
            CreateMap<RegisterPatientDto, Patient>().ReverseMap();

            CreateMap<ResponsePatientDto, Patient>()
                .ForMember(to => to.DocumentId, opt => opt.MapFrom(from => from.Id))
                .ReverseMap();


            CreateMap<RegisterPatientHistoryDto, PatientHistory>()
                .ForMember(to => to.PlaceId, opt => opt.MapFrom(from => from.PlaceId))
                .ReverseMap();



            CreateMap<PatientHistory, ResponsePatientHistoryDto>()
                .ForMember(to => to.PlaceId, opt => opt.MapFrom(from => from.PlaceId))
                .ForMember(to => to.PlaceDescription, opt => opt.MapFrom(from => from.PlaceId.ToString()))
                .ForMember(to => to.Id, opt => opt.MapFrom(from => from.DocumentId))
                .ForMember(to => to.RegisterDate, opt => opt.MapFrom(from => from.CreateTime))
                .ForMember(to => to.Exams, opt => opt.MapFrom(from => from.Exams.Select(p => p.Code + "-" + p.Name)))

                .ReverseMap();
        }
    }
}
