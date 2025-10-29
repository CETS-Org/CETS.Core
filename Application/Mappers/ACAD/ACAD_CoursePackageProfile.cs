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
            CreateMap<CreateCoursePackageRequest, ACAD_CoursePackage>()
                .ForMember(dest => dest.ACAD_CoursePackageItems, opt => opt.Ignore()); // Handled separately in service
            CreateMap<UpdateCoursePackageRequest, ACAD_CoursePackage>()
                .ForMember(dest => dest.ACAD_CoursePackageItems, opt => opt.Ignore()); // Handled separately in service
            CreateMap<ACAD_CoursePackage, CoursePackageResponse>()
                .ForMember(dest => dest.TotalIndividualPrice,
                           opt => opt.MapFrom(src => src.ACAD_CoursePackageItems.Where(i => !i.IsDeleted).Sum(i => i.Course.StandardPrice)))
                .ForMember(dest => dest.CourseNames,
                           opt => opt.MapFrom(src => src.ACAD_CoursePackageItems.Where(i => !i.IsDeleted).OrderBy(i => i.Sequence).Select(i => i.Course.CourseName).ToList()));
            CreateMap<ACAD_CoursePackage, CoursePackageDetailResponse>()
                .ForMember(dest => dest.Courses,
                           opt => opt.MapFrom(src => src.ACAD_CoursePackageItems))
                .ForMember(dest => dest.TotalIndividualPrice,
                           opt => opt.MapFrom(src => src.ACAD_CoursePackageItems.Where(i => !i.IsDeleted).Sum(i => i.Course.StandardPrice)))
                .ForMember(dest => dest.CreatedAt,
                           opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.CreatedBy,
                           opt => opt.MapFrom(src => src.CreatedByNavigation != null ? src.CreatedByNavigation.FullName : null))
                .ForMember(dest => dest.UpdatedAt,
                           opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.UpdatedBy,
                           opt => opt.MapFrom(src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.FullName : null));

            // CoursePackageItem mappings
            CreateMap<AddCourseToPackageRequest, ACAD_CoursePackageItem>();
            CreateMap<CreateCoursePackageItemRequest, ACAD_CoursePackageItem>();
            CreateMap<UpdateCoursePackageItemRequest, ACAD_CoursePackageItem>();
            
            CreateMap<ACAD_CoursePackageItem, CourseInPackageResponse>()
                .ForMember(dest => dest.CourseId,
                           opt => opt.MapFrom(src => src.CourseID))
                .ForMember(dest => dest.CourseName,
                           opt => opt.MapFrom(src => src.Course.CourseName))
                .ForMember(dest => dest.StandardPrice,
                           opt => opt.MapFrom(src => src.Course.StandardPrice))
                .ForMember(dest => dest.Description,
                           opt => opt.MapFrom(src => src.Course.Description))
                .ForMember(dest => dest.Duration,
                           opt => opt.MapFrom(src => CalculateCourseDuration(src.Course)))
                .ForMember(dest => dest.CourseLevel,
                           opt => opt.MapFrom(src => src.Course.CourseLevel != null ? src.Course.CourseLevel.Name : null))
                .ForMember(dest => dest.CategoryName,
                           opt => opt.MapFrom(src => src.Course.Category != null ? src.Course.Category.Name : null))
                .ForMember(dest => dest.CourseObjective,
                           opt => opt.MapFrom(src => src.Course.CourseObjective != null ? src.Course.CourseObjective : new List<string>()))
                .ForMember(dest => dest.Rating,
                           opt => opt.MapFrom(src => src.Course.AverageRating ?? 0))
                .ForMember(dest => dest.StudentsCount,
                           opt => opt.MapFrom(src => src.Course.ACAD_Enrollments.Count(e => !e.IsDeleted)));

            CreateMap<ACAD_CoursePackageItem, CoursePackageItemResponse>()
                .ForMember(dest => dest.CourseName,
                           opt => opt.MapFrom(src => src.Course.CourseName))
                .ForMember(dest => dest.CourseCode,
                           opt => opt.MapFrom(src => src.Course.CourseCode));
        }

        private static string CalculateCourseDuration(ACAD_Course course)
        {
            if (course.ACAD_Syllabi == null || !course.ACAD_Syllabi.Any())
                return "Self-paced learning";

            var totalSlots = course.ACAD_Syllabi
                .Where(s => !s.IsDeleted)
                .SelectMany(s => s.ACAD_SyllabusItems)
                .Where(item => !item.IsDeleted && item.TotalSlots.HasValue)
                .Sum(item => item.TotalSlots.Value);

            if (totalSlots == 0)
                return "Self-paced learning";

            return $"{totalSlots} slot{(totalSlots > 1 ? "s" : "")}";
        }
    }
}
