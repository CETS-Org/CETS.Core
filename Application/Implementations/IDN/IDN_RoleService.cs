using Application.Interfaces.IDN;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.IDN;
using DTOs.IDN.IDN_Role.Requests;
using DTOs.IDN.IDN_Role.Responses;

namespace Application.Implementations.IDN
{
    public class IDN_RoleService : BaseService<IDN_Role, RoleResponse, UpdateRoleRequest, CreateRoleRequest>, IIDN_RoleService
    {
        private readonly IIDN_RoleRepository _roleRepository;
        public IDN_RoleService(IIDN_RoleRepository roleRepository, IUnitOfWork unitOfWork, IMapper mapper)
            : base(roleRepository, unitOfWork, mapper)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IReadOnlyList<IDN_Role>> SearchRolesByKeywordAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<IDN_Role>();

            return await _roleRepository.SearchRolesByKeywordAsync(keyword);
        }


    }
}


