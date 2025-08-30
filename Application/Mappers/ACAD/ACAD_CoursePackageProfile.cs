using AutoMapper;
using Domain.Entities;
using DTOs.ACAD.ACAD_CoursePackage.Requests;
using DTOs.ACAD.ACAD_CoursePackage.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers.ACAD
{
    public class ACAD_CoursePackageProfile : Profile
    {
        public ACAD_CoursePackageProfile()
        {
            CreateMap<CreateCoursePackageRequest, ACAD_CoursePackage>();
            CreateMap<AddCourseToPackageRequest, ACAD_CoursePackageItem>();

            CreateMap<ACAD_CoursePackage, CoursePackageResponse>();
            CreateMap<ACAD_CoursePackageItem, CourseInPackageResponse>()
                .ForMember(dest => dest.CourseName,
                           opt => opt.MapFrom(src => src.Course.CourseName));

            CreateMap<ACAD_CoursePackage, CoursePackageDetailResponse>()
                .ForMember(dest => dest.Courses,
                           opt => opt.MapFrom(src => src.ACAD_CoursePackageItems));
        }
    }
}
