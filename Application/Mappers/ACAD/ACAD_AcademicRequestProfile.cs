using AutoMapper;
using Domain.Entities;
using DTOs.ACAD.ACAD_AcademicRequest.Requests;
using DTOs.ACAD.ACAD_AcademicRequest.Responses;
using DTOs.ACAD.ACAD_AcademicRequestHistory.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers.ACAD
{
    public class ACAD_AcademicRequestProfile : Profile
    {
        public ACAD_AcademicRequestProfile()
        {
            CreateMap<CreateAcademicRequest, ACAD_AcademicRequest>();
            CreateMap<ProcessAcademicRequest, ACAD_AcademicRequestHistory>()
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.StaffID))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<ACAD_AcademicRequest, AcademicRequestResponse>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.Account.FullName))
                .ForMember(dest => dest.StudentEmail, opt => opt.MapFrom(src => src.Student.Account.Email))
                .ForMember(dest => dest.RequestTypeName, opt => opt.MapFrom(src => src.RequestType.Name))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.AcademicRequestStatus.Name))
                .ForMember(dest => dest.FromClassName, opt => opt.MapFrom(src => src.FromClass.ClassName))
                .ForMember(dest => dest.ToClassName, opt => opt.MapFrom(src => src.ToClass.ClassName))
                .ForMember(dest => dest.ProcessedByName, opt => opt.MapFrom(src => src.ProcessedByNavigation.FullName))
                .ForMember(dest => dest.MeetingInfo, opt => opt.MapFrom(src => 
                    src.ClassMeeting != null 
                        ? $"{src.ClassMeeting.Date:yyyy-MM-dd} - {src.ClassMeeting.Slot.Name}" 
                        : null))
                .ForMember(dest => dest.NewSlotName, opt => opt.MapFrom(src => src.NewSlot != null ? src.NewSlot.Name : null))
                .ForMember(dest => dest.NewRoomName, opt => opt.MapFrom(src => src.NewRoom != null ? src.NewRoom.RoomCode : null));

            CreateMap<ACAD_AcademicRequestHistory, AcademicRequestHistoryResponse>();
        }
    }
}
