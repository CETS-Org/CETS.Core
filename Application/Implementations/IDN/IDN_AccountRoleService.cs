using Application.Interfaces.IDN;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.IDN;
using DTOs.IDN.IDN_AccountRole.Requests;
using DTOs.IDN.IDN_AccountRole.Responses;

namespace Application.Implementations.IDN
{
    public class IDN_AccountRoleService : IIDN_AccountRoleService
    {
        private readonly IIDN_AccountRoleRepository _accountRoleRepository;
        private readonly IIDN_AccountRepository _accountRepository;
        private readonly IIDN_RoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public IDN_AccountRoleService(
            IIDN_AccountRoleRepository accountRoleRepository,
            IIDN_AccountRepository accountRepository,
            IIDN_RoleRepository roleRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _accountRoleRepository = accountRoleRepository;
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<AccountRoleResponse>> GetRolesByAccountIdAsync(Guid accountId)
        {
            var list = await _accountRoleRepository.GetByAccountIdAsync(accountId);
            return _mapper.Map<IReadOnlyList<AccountRoleResponse>>(list);
        }

        public async Task<AccountRoleResponse> AssignRoleAsync(AssignRoleRequest request)
        {
            var accountExists = await _accountRepository.ExistsByIdAsync(request.AccountId);
            if (!accountExists)
            {
                throw new KeyNotFoundException($"Account {request.AccountId} not found.");
            }
            var roleExists = await _roleRepository.ExistsByIdAsync(request.RoleId);
            if (!roleExists)
            {
                throw new KeyNotFoundException($"Role {request.RoleId} not found.");
            }
            var existing = await _accountRoleRepository.GetByIdAsync(request.AccountId, request.RoleId);
            if (existing != null)
            {
                throw new InvalidOperationException("Account already has this role.");
            }

            var entity = _mapper.Map<IDN_AccountRole>(request);
            _accountRoleRepository.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            // Reload with Role included
            var withRole = await _accountRoleRepository.GetByIdAsync(request.AccountId, request.RoleId);
            if (withRole == null)
            {
                withRole = entity;
            }
            return _mapper.Map<AccountRoleResponse>(withRole);
        }

        public async Task<bool> UnassignRoleAsync(UnassignRoleRequest request)
        {
            var existing = await _accountRoleRepository.GetByIdAsync(request.AccountId, request.RoleId);
            if (existing == null)
            {
                return false;
            }
            _accountRoleRepository.Remove(existing);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}


