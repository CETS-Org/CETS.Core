using AutoMapper;
using Domain.Entities;
using DTOs.IDN_Role.Requests;
using DTOs.IDN_Role.Responses;

namespace Application.Mappers
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


