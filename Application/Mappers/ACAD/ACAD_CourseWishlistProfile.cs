using AutoMapper;
using Domain.Entities;
using DTOs.ACAD.ACAD_CourseWishlist.Responses;
using System.Linq;

namespace Application.Mappers.ACAD
{
    public class ACAD_CourseWishlistProfile : Profile
    {
        public ACAD_CourseWishlistProfile()
        {
            CreateMap<ACAD_CourseWishlist, WishlistItemResponse>()
                .ForMember(dest => dest.CourseCode, opt => opt.MapFrom(src => src.Course.CourseCode))
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.CourseName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Course.Description))
                .ForMember(dest => dest.CourseImageUrl, opt => opt.MapFrom(src => src.Course.CourseImageUrl))
                .ForMember(dest => dest.StandardPrice, opt => opt.MapFrom(src => src.Course.StandardPrice))
                .ForMember(dest => dest.CourseLevel, opt => opt.MapFrom(src => src.Course.CourseLevel != null ? src.Course.CourseLevel.Name : ""))
                .ForMember(dest => dest.CourseFormat, opt => opt.MapFrom(src => src.Course.CourseFormat != null ? src.Course.CourseFormat.Name : ""))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => 
                    src.Course.ACAD_Syllabi.SelectMany(s => s.ACAD_SyllabusItems).Sum(i => i.TotalSlots ?? 0) + " slots"))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => (double)(src.Course.AverageRating ?? 0)))
                .ForMember(dest => dest.StudentsCount, opt => opt.MapFrom(src => 
                    src.Course.ACAD_Enrollments.Count(e => !e.IsDeleted)))
                .ForMember(dest => dest.TeacherDetails, opt => opt.MapFrom(src => 
                    src.Course.ACAD_CourseTeacherAssignments.Select(a => a.Teacher).ToList()));
        }
    }
}

