using Domain.Entities;
using DTOs.ACAD.ACAD_ClassMeetings.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DTOs.ACAD.ACAD_Class.Requests;
using DTOs.ACAD.ACAD_ClassMeetings.Requests;


namespace Application.Mappers.ACAD
{
    public  class ACAD_ClassMeetingProfile : Profile
    {
        public ACAD_ClassMeetingProfile()
        {
            CreateMap<ACAD_ClassMeeting, ClassMeetingResponse>()
                 .ForMember(dest => dest.slot,
                           opt => opt.MapFrom(src => src.Slot.Name))
                .ForMember(dest => dest.RoomID,
                           opt => opt.MapFrom(src => src.Room != null ? src.Room.RoomCode : string.Empty));

            CreateMap<ACAD_ClassMeeting, ClassMeetingStaffViewResponse>()
                .ForMember(dest => dest.TeacherName,
                    opt => opt.MapFrom(src =>
                        src.TeacherAssignment == null ||
                        src.TeacherAssignment.Teacher == null ||
                        src.TeacherAssignment.Teacher.Account == null
                            ? string.Empty
                            : src.TeacherAssignment.Teacher.Account.FullName))

                .ForMember(dest => dest.CourseName,
                    opt => opt.MapFrom(src =>
                        src.TeacherAssignment == null ||
                        src.TeacherAssignment.Course == null
                            ? string.Empty
                            : src.TeacherAssignment.Course.CourseName))

                .ForMember(dest => dest.CourseId,
                    opt => opt.MapFrom(src =>
                        src.TeacherAssignment != null
                            ? src.TeacherAssignment.Course.Id
                            : Guid.Empty
                    ))

                .ForMember(dest => dest.CoveredTopic,
                    opt => opt.MapFrom(src =>
                        src.CoveredTopic == null
                            ? string.Empty
                            : src.CoveredTopic.TopicTitle))

                .ForMember(dest => dest.slot,
                    opt => opt.MapFrom(src =>
                        src.Slot == null
                            ? string.Empty
                            : src.Slot.Name))

                 .ForMember(dest => dest.RoomCode,
                    opt => opt.MapFrom(src =>
                        src.Room == null
                            ? string.Empty
                            : src.Room.RoomCode))

                .ForMember(dest => dest.RoomID,
                    opt => opt.MapFrom(src =>
                        src.Room == null
                            ? Guid.Empty
                            : src.Room.Id));



            CreateMap<CreateClassMeetingRequest, ACAD_ClassMeeting>();
            CreateMap<UpdateClassMeetingRequest, ACAD_ClassMeeting>();
        }
    }
}
