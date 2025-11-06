using AutoMapper;
using Domain.Entities;
using DTOs.ACAD.ACAD_Class.Requests;
using DTOs.ACAD.ACAD_Class.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers.ACAD
{
    public class ACAD_ClassProfile : Profile
    {
        public ACAD_ClassProfile()
        {
            // Request -> Entity
            CreateMap<CreateClassRequest, ACAD_Class>();
            CreateMap<UpdateClassRequest, ACAD_Class>();

            // Entity -> Response
            CreateMap<ACAD_Class, ClassResponse>()
                .ForMember(dest => dest.StatusName,
                           opt => opt.MapFrom(src => src.ClassStatus != null ? src.ClassStatus.Name : string.Empty));
            CreateMap<ACAD_Class, ClassStaffViewResponse>()
                .ForMember(dest => dest.ClassFormat,
                           opt => opt.MapFrom(src => src.CourseFormat != null ? src.CourseFormat.Name : string.Empty))
                .ForMember(dest => dest.ClassStatus,
                            opt => opt.MapFrom(src => src.ClassStatus != null ? src.ClassStatus.Name : string.Empty))
                .ForMember(dest => dest.CourseName,
                            opt => opt.MapFrom(src => src.TeacherAssignment != null && src.TeacherAssignment.Course != null ? src.TeacherAssignment.Course.CourseName : string.Empty))
                .ForMember(dest => dest.TeacherName,
                            opt => opt.MapFrom(src => src.TeacherAssignment != null && src.TeacherAssignment.Teacher.Account != null ? src.TeacherAssignment.Teacher.Account.FullName : string.Empty))
                .ForMember(dest => dest.CourseName,
                            opt => opt.MapFrom(src => src.TeacherAssignment != null && src.TeacherAssignment.Course != null ? src.TeacherAssignment.Course.CourseName : string.Empty))
                ;

        }
    }
}
