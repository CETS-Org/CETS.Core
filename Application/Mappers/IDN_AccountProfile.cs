using AutoMapper;
using Domain.Entities;
using DTOs.IDN_Account.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    public class IDN_AccountProfile : Profile
    {
        public IDN_AccountProfile()
        {
            CreateMap<IDN_Account, AccountResponse>()
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

        }
    }
}
