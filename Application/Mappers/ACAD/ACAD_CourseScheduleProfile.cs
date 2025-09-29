using AutoMapper;
using Domain.Entities;
using DTOs.ACAD.ACAD_CourseSchedule.Requests;
using DTOs.ACAD.ACAD_CourseSchedule.Responses;

namespace Application.Mappers.ACAD
{
    public class ACAD_CourseScheduleProfile : Profile
    {
        public ACAD_CourseScheduleProfile()
        {
            CreateMap<ACAD_CourseSchedule, CourseScheduleResponse>()
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course != null ? src.Course.CourseName : null))
                .ForMember(dest => dest.TimeSlotName, opt => opt.MapFrom(src => src.TimeSlot != null ? src.TimeSlot.Name : null));

            CreateMap<CreateCourseScheduleRequest, ACAD_CourseSchedule>();

            CreateMap<UpdateCourseScheduleRequest, ACAD_CourseSchedule>();
        }
    }
}
