using Domain.Entities;
using DTOs.IDN.IDN_Role.Requests;
using DTOs.IDN.IDN_Role.Responses;

namespace Application.Interfaces.IDN
{
    public interface IIDN_RoleService : IBaseService<IDN_Role, RoleResponse, UpdateRoleRequest, CreateRoleRequest>
    {
    }
}


