using AutoMapper;
using Domain.Entities;
using DTOs.IDN.IDN_AccountRole.Requests;
using DTOs.IDN.IDN_AccountRole.Responses;

namespace Application.Mappers.IDN
{
    public class IDN_AccountRoleProfile : Profile
    {
        public IDN_AccountRoleProfile()
        {
            CreateMap<IDN_AccountRole, AccountRoleResponse>()
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountID))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleID))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName));

            CreateMap<AssignRoleRequest, IDN_AccountRole>()
                .ForMember(dest => dest.AccountID, opt => opt.MapFrom(src => src.AccountId))
                .ForMember(dest => dest.RoleID, opt => opt.MapFrom(src => src.RoleId));
        }
    }
}


