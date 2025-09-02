using AutoMapper;
using Domain.Entities;
using DTOs.IDN_Account.Responses;
using DTOs.IDN_TeacherCredential.Responses;
using DTOs.CORE.LookUp.Responses;
using DTOs.CORE.LookUp.Requests;
using DTOs.CORE.LookUpType.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    public class CORE_LookUpProfile : Profile
    {
        public CORE_LookUpProfile()
        {
            CreateMap<CORE_LookUp, AccountStatusResponse>();
            CreateMap<CORE_LookUp, CredentialTypeResponse>();

            // LookUp mappings
            CreateMap<CORE_LookUp, LookUpResponse>()
                .ForMember(dest => dest.LookUpId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.LookUpTypeId, opt => opt.MapFrom(src => src.LookUpTypeID))
                .ForMember(dest => dest.LookUpTypeCode, opt => opt.MapFrom(src => src.LookUpType.Code));
            CreateMap<CreateLookUpRequest, CORE_LookUp>();
            CreateMap<UpdateLookUpRequest, CORE_LookUp>();

        
        }
    }
}
