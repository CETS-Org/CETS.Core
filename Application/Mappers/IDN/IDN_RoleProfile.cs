using AutoMapper;
using Domain.Entities;
using DTOs.IDN.IDN_Role.Requests;
using DTOs.IDN.IDN_Role.Responses;

namespace Application.Mappers.IDN
{
    public class IDN_RoleProfile : Profile
    {
        public IDN_RoleProfile()
        {
            CreateMap<IDN_Role, RoleResponse>().ReverseMap();
            CreateMap<CreateRoleRequest, IDN_Role>();
            CreateMap<UpdateRoleRequest, IDN_Role>();
        }
    }
}


