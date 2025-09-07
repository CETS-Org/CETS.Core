using AutoMapper;
using Domain.Entities;
using DTOs.ACAD.ACAD_Enrollment.Requests;
using DTOs.ACAD.ACAD_Enrollment.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers.ACAD
{
    public class EnrollmentProfile : Profile
    {
        public EnrollmentProfile()
        {
            CreateMap<CreateEnrollmentRequest, ACAD_Enrollment>();

            CreateMap<ACAD_Enrollment, EnrollmentResponse>();
            CreateMap<ACAD_Enrollment, EnrollmentDetailResponse>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.Account.FullName : null))
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course != null ? src.Course.CourseName : null))
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class != null ? src.Class.ClassName : null))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.EnrollmentStatus != null ? src.EnrollmentStatus.Name : null));
        }
    }
}
