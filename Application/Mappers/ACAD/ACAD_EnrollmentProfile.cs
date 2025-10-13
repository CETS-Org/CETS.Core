using AutoMapper;
using Domain.Entities;
using DTOs.ACAD.ACAD_Course.Responses;
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

            CreateMap<ACAD_Enrollment, CourseEnrollmentListResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Course != null ? src.Course.Id : Guid.Empty))
            .ForMember(dest => dest.CourseCode, opt => opt.MapFrom(src => src.Course != null ? src.Course.CourseCode : string.Empty))
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course != null ? src.Course.CourseName : string.Empty))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Course.Description))
            .ForMember(dest => dest.CourseImageUrl, opt => opt.MapFrom(src => src.Course.CourseImageUrl))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.Course != null && src.Course.IsActive))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.Teachers, opt => opt.MapFrom(src =>
                src.Course != null && src.Course.ACAD_CourseTeacherAssignments != null
                    ? src.Course.ACAD_CourseTeacherAssignments
                        .Where(ta => ta.Teacher != null)
                        .Select(ta => ta.Teacher.Account.FullName)
                        .ToList()
                    : new List<string>()))
            .ForMember(dest => dest.EnrollmentStatus, opt => opt.MapFrom(src => src.EnrollmentStatus.Name != null ? src.EnrollmentStatus.Name.ToString() : string.Empty));

            CreateMap<ACAD_Enrollment, CourseItemResponse>()
            .ForMember(d => d.CourseId, o => o.MapFrom(s => s.CourseID))
            .ForMember(d => d.CourseCode, o => o.MapFrom(s => s.Course.CourseCode))
            .ForMember(d => d.CourseName, o => o.MapFrom(s => s.Course.CourseName))
            .ForMember(d => d.TeacherNames, o => o.MapFrom(s =>
                s.Course != null && s.Course.ACAD_CourseTeacherAssignments != null
                    ? s.Course.ACAD_CourseTeacherAssignments
                        .Where(ta => ta.Teacher != null && ta.Teacher.Account != null)
                        .Select(ta => ta.Teacher.Account.FullName)
                        .ToList()                    
                    : new List<string>()))
            .ForMember(d => d.StatusCode, o => o.MapFrom(s => s.EnrollmentStatus.Code))
            .ForMember(d => d.StatusName, o => o.MapFrom(s => s.EnrollmentStatus.Name));

            CreateMap<ACAD_Enrollment, StudentCourseDetailResponse>()
                .ForMember(d => d.CourseId, o => o.MapFrom(s => s.CourseID))
                .ForMember(d => d.CourseCode, o => o.MapFrom(s => s.Course.CourseCode))
                .ForMember(d => d.CourseName, o => o.MapFrom(s => s.Course.CourseName))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Course.Description))
                .ForMember(d => d.TeacherNames, o => o.MapFrom(s =>
                    s.Course.ACAD_CourseTeacherAssignments
                        .Where(ta => ta.Teacher != null && ta.Teacher.Account != null)
                        .Select(ta => ta.Teacher.Account.FullName)
                        .ToList()))
                .ForMember(d => d.StatusCode, o => o.MapFrom(s => s.EnrollmentStatus.Code))
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.EnrollmentStatus.Name))
                .ForMember(d => d.Assignments, o => o.Ignore());
        }
    }
}
