using AutoMapper;
using Domain.Entities;
using DTOs.ACAD.ACAD_CourseBenefit.Requests;
using DTOs.ACAD.ACAD_CourseBenefit.Responses;

namespace Application.Mappers.ACAD
{
    public class CourseBenefitProfile : Profile
    {
        public CourseBenefitProfile()
        {
            CreateMap<CreateCourseBenefitRequest, ACAD_CourseBenefit>();
            CreateMap<UpdateCourseBenefitRequest, ACAD_CourseBenefit>();

            CreateMap<ACAD_CourseBenefit, CourseBenefitResponse>()
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course != null ? src.Course.CourseName : ""))
                .ForMember(dest => dest.BenefitName, opt => opt.MapFrom(src => src.Benefit != null ? src.Benefit.Name : ""));
        }
    }
}
