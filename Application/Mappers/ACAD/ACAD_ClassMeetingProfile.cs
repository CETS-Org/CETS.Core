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
            CreateMap<ACAD_ClassMeeting, ClassMeetingResponse>();
            CreateMap<CreateClassMeetingRequest, ACAD_ClassMeeting>();
            CreateMap<UpdateClassMeetingRequest, ACAD_ClassMeeting>();
        }
    }
}
