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

            CreateMap<ACAD_AcademicRequest, AcademicRequestResponse>();
            CreateMap<ACAD_AcademicRequestHistory, AcademicRequestHistoryResponse>();
        }
    }
}
