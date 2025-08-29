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
    public class CORE_LookUpProfile : Profile
    {
        public CORE_LookUpProfile()
        {
            CreateMap<CORE_LookUp, AccountStatusResponse>();
        }
    }
}
