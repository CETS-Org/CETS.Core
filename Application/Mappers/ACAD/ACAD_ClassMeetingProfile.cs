using Domain.Entities;
using DTOs.ACAD.ACAD_ClassMeetings.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;


namespace Application.Mappers.ACAD
{
    public  class ACAD_ClassMeetingProfile : Profile
    {
        public ACAD_ClassMeetingProfile()
        {
            CreateMap<ACAD_ClassMeeting, ClassMeetingResponse>();
        }
    }
}
