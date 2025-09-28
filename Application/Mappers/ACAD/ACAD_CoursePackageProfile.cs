using AutoMapper;
using Domain.Entities;
using DTOs.ACAD.ACAD_CoursePackage.Requests;
using DTOs.ACAD.ACAD_CoursePackage.Responses;
using DTOs.ACAD.ACAD_CoursePackageItem.Requests;
using DTOs.ACAD.ACAD_CoursePackageItem.Responses;
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
            // CoursePackage mappings
            CreateMap<CreateCoursePackageRequest, ACAD_CoursePackage>();
            CreateMap<UpdateCoursePackageRequest, ACAD_CoursePackage>();
            CreateMap<ACAD_CoursePackage, CoursePackageResponse>();
            CreateMap<ACAD_CoursePackage, CoursePackageDetailResponse>()
                .ForMember(dest => dest.Courses,
                           opt => opt.MapFrom(src => src.ACAD_CoursePackageItems));

            // CoursePackageItem mappings
            CreateMap<AddCourseToPackageRequest, ACAD_CoursePackageItem>();
            CreateMap<CreateCoursePackageItemRequest, ACAD_CoursePackageItem>();
            CreateMap<UpdateCoursePackageItemRequest, ACAD_CoursePackageItem>();
            
            CreateMap<ACAD_CoursePackageItem, CourseInPackageResponse>()
                .ForMember(dest => dest.CourseName,
                           opt => opt.MapFrom(src => src.Course.CourseName));

            CreateMap<ACAD_CoursePackageItem, CoursePackageItemResponse>()
                .ForMember(dest => dest.CourseName,
                           opt => opt.MapFrom(src => src.Course.CourseName))
                .ForMember(dest => dest.CourseCode,
                           opt => opt.MapFrom(src => src.Course.CourseCode));
        }
    }
}
