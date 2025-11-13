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

            CreateMap<CreateClassMeetingRequest, ACAD_ClassMeeting>();
            CreateMap<UpdateClassMeetingRequest, ACAD_ClassMeeting>();
        }
    }
}
