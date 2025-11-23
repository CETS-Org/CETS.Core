using AutoMapper;
using Domain.Entities;
using DTOs.ACAD.ACAD_CourseTeacherAssignment.Responses;

namespace Application.Mappers.ACAD
{
    public class ACAD_CourseTeacherAssignmentProfile : Profile
    {
        public ACAD_CourseTeacherAssignmentProfile()
        {
            CreateMap<ACAD_CourseTeacherAssignment, CourseListAssignmentResponse>()
             .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.Course.Id))
             .ForMember(dest => dest.CourseCode, opt => opt.MapFrom(src => src.Course.CourseCode))
             .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.CourseName))
             .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Course.Description))
             .ForMember(dest => dest.CourseImageUrl, opt => opt.MapFrom(src => src.Course.CourseImageUrl))
             .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Course.Category.Name))
             .ForMember(dest => dest.CourseLevelName, opt => opt.MapFrom(src => src.Course.CourseLevel.Name))
             .ForMember(dest => dest.CourseFormatName, opt => opt.MapFrom(src => src.Course.CourseFormat.Name))
             .ForMember(dest => dest.StudentCount, opt => opt.MapFrom(src => src.Course.ACAD_Enrollments.Count))
             .ForMember(dest => dest.AssignedAt, opt => opt.MapFrom(src => src.Course.ACAD_CourseTeacherAssignments 
                                                            .OrderByDescending(a => a.AssignedAt)
                                                            .FirstOrDefault()!.AssignedAt));

            CreateMap<ACAD_Course, CourseListAssignmentResponse>()
            .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CourseCode, opt => opt.MapFrom(src => src.CourseCode))
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.CourseName))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.CourseImageUrl, opt => opt.MapFrom(src => src.CourseImageUrl))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.CourseLevelName, opt => opt.MapFrom(src => src.CourseLevel.Name))
            .ForMember(dest => dest.CourseFormatName, opt => opt.MapFrom(src => src.CourseFormat.Name))
            .ForMember(dest => dest.StudentCount, opt => opt.MapFrom(src => src.ACAD_Enrollments.Count));

            CreateMap<ACAD_CourseTeacherAssignment, CourseTeacherAssignmentResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AssignmentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CourseID, opt => opt.MapFrom(src => src.CourseID))
                .ForMember(dest => dest.TeacherID, opt => opt.MapFrom(src => src.TeacherID))
                .ForMember(dest => dest.AssignedAt, opt => opt.MapFrom(src => src.AssignedAt))
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course != null ? src.Course.CourseName : null))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Teacher != null ? src.Teacher.Account.FullName : null))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Teacher != null ? src.Teacher.Account.Email : null))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Teacher != null ? src.Teacher.Account.PhoneNumber : null))
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.Teacher != null ? src.Teacher.Account.AvatarUrl : null))
                .ForMember(dest => dest.YearsExperience, opt => opt.MapFrom(src => src.Teacher != null ? src.Teacher.YearsExperience : null))
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher != null ? src.Teacher.Account.FullName : null))
                .ForMember(dest => dest.TeacherEmail, opt => opt.MapFrom(src => src.Teacher != null ? src.Teacher.Account.Email : null))
                .ForMember(dest => dest.TeacherAvatarUrl, opt => opt.MapFrom(src => src.Teacher != null ? src.Teacher.Account.AvatarUrl : null))
                .ForMember(dest => dest.TeacherCode, opt => opt.MapFrom(src => src.Teacher != null ? src.Teacher.TeacherCode : null));
        }
    }
}
