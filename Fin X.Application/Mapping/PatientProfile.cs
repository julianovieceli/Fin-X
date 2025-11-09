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
                .ForMember(to => to.Placement, opt => opt.MapFrom(from => from.PlacementId))
                .ReverseMap();



            CreateMap<PatientHistory, ResponsePatientHistoryDto>()
                .ForMember(to => to.Placement, opt => opt.MapFrom(from => from.Placement))
                .ForMember(to => to.Id, opt => opt.MapFrom(from => from.DocumentId))
                .ReverseMap();
        }
    }
}
