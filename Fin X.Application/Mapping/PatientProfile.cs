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
        }
    }
}
