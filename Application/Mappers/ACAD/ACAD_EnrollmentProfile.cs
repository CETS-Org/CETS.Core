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

            // Map từ Enrollment -> CourseListItemResponse
            //CreateMap<ACAD_Enrollment, CourseListItemResponse>()
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Course.Id.ToString()))
            //    .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.CourseName))
            //    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Course.Description))
            //    .ForMember(dest => dest.Teacher, opt => opt.MapFrom(src =>
            //        string.Join(", ", src.Course.ACAD_CourseTeacherAssignments.Select(ta => ta.Teacher.Account.FullName))))
            //    .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Course.Duration))
            //    .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Course.Level.Name))
            //    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Course.Price))
            //    .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Course.Rating))
            //    .ForMember(dest => dest.StudentsCount, opt => opt.MapFrom(src => src.Course.Enrollments.Count))
            //    .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Course.Image))
            //    .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Course.Category.Name));
        }
    }
}
