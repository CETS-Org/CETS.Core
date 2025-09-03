using AutoMapper;
using Domain.Entities;
using DTOs.CORE.LookUpType.Requests;
using DTOs.CORE.LookUpType.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers.CORE
{
    public class CORE_LookUpTypeProfile : Profile
    {
        public CORE_LookUpTypeProfile()
        {
            CreateMap<CORE_LookUpType, LookUpTypeResponse>()
                .ForMember(dest => dest.LookUpTypeId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

            CreateMap<CORE_LookUpType, CreateLookUpTypeRequest>().ReverseMap();

            CreateMap<CORE_LookUpType, UpdateLookUpTypeRequest>().ReverseMap();
        }
    }
}
