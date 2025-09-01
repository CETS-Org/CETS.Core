using Application.Interfaces.IDN;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.IDN;
using DTOs.IDN_Role.Requests;
using DTOs.IDN_Role.Responses;

namespace Application.Implementations.IDN
{
    public class IDN_RoleService : BaseService<IDN_Role, RoleResponse, UpdateRoleRequest, CreateRoleRequest>, IIDN_RoleService
    {
        public IDN_RoleService(IIDN_RoleRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
            : base(repository, unitOfWork, mapper)
        {
        }
    }
}


