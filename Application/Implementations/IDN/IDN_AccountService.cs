using Application.Interfaces.IDN;
using AutoMapper;
using Domain.Constants;
using Domain.Interfaces;
using Domain.Interfaces.CORE;
using Domain.Interfaces.IDN;
using DTOs.IDN.IDN_Account.Requests;
using DTOs.IDN.IDN_Account.Responses;
using Microsoft.EntityFrameworkCore;

namespace Application.Implementations.IDN
{
    public class IDN_AccountService : IIDN_AccountService
    {
        private readonly IIDN_AccountRepository _accountRepository;
        private readonly ICORE_LookUpRepository _lookUpRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public IDN_AccountService(IIDN_AccountRepository accountRepository, ICORE_LookUpRepository lookUpRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _lookUpRepository = lookUpRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<AccountStatusResponse>> GetAccountStatusesAsync()
        {
            var lookup = await _lookUpRepository.GetByTypeAsync(LookUpTypes.AccountStatus);

            return _mapper.Map<IReadOnlyList<AccountStatusResponse>>(lookup);
        }

        public async Task<IEnumerable<AccountResponse>> GetAllAccountsAsync(AccountFilterRequest filter)
        {
            var query = _accountRepository.QueryWithRoles();

            if (!string.IsNullOrEmpty(filter.RoleName))
            {
                query = query.Where(a => a.IDN_AccountRoles.Any(r => r.Role.RoleName == filter.RoleName));
            }

            if (filter.CurrentRole == "Staff")
            {
                query = query.Where(a => a.IDN_AccountRoles.Any(r =>
                    r.Role.RoleName == "Student" || r.Role.RoleName == "Teacher"));
            }

            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(a => a.FullName.Contains(filter.Name));

            if (!string.IsNullOrEmpty(filter.Email))
                query = query.Where(a => a.Email.Contains(filter.Email));

            if (!string.IsNullOrEmpty(filter.PhoneNumber))
                query = query.Where(a => a.PhoneNumber.Contains(filter.PhoneNumber));

            // Filter theo Status
            if (!string.IsNullOrEmpty(filter.StatusName))
            {
                query = query.Where(a => a.AccountStatus.Code == filter.StatusName);
            }
            //Sort 
            if (!string.IsNullOrEmpty(filter.SortOrder) && filter.SortOrder.ToLower() == "desc")
            {
                query = query.OrderByDescending(a => a.FullName);
            }
            else
            {
                query = query.OrderBy(a => a.FullName); 
            }
            var accounts = await query.ToListAsync();
            return _mapper.Map<IEnumerable<AccountResponse>>(accounts);
        }
        public async Task<AccountResponse?> GetAccountByIdAsync(Guid id)
        {
            var account = await _accountRepository.GetDetailByIdAsync(id);
            return _mapper.Map<AccountResponse?>(account);
        }
        public async Task<AccountResponse> GetAccountByEmailAsync(string email)
        {
            var account = await _accountRepository.FindFirstAsync(ac => ac.Email == email);
            if (account == null)
            {
                throw new KeyNotFoundException($"Account with email {email} not found.");
            }
            return _mapper.Map<AccountResponse>(account);
        }

        public async Task<AccountResponse> UpdateAccountAsync(Guid id, UpdateAccountRequest dto)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
            {
                throw new KeyNotFoundException($"Account with id {id} not found.");
            }

            _mapper.Map(dto, account);
            _accountRepository.Update(account);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<AccountResponse>(account);
        }

        public async Task<AccountResponse?> DeactivateAccountAsync(Guid id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
            {
                throw new KeyNotFoundException($"Account with id {id} not found.");
            }
            var inactiveStatus = await _lookUpRepository.GetByCodeAsync(LookUpTypes.AccountStatus, AccountStatuses.Locked.ToString());
            if (inactiveStatus == null)
            {
                throw new InvalidOperationException("Inactive status not found in lookup.");
            }
            account.AccountStatusID = inactiveStatus.Id;
            _accountRepository.Update(account);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<AccountResponse?>(account);
        }

        public async Task<AccountResponse?> ActivateAccountAsync(Guid id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
            {
                throw new KeyNotFoundException($"Account with id {id} not found.");
            }
            var activeStatus = await _lookUpRepository.GetByCodeAsync(LookUpTypes.AccountStatus, AccountStatuses.Active.ToString());
            if (activeStatus == null)
            {
                throw new InvalidOperationException("Active status not found in lookup.");
            }
            account.AccountStatusID = activeStatus.Id;
            _accountRepository.Update(account);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<AccountResponse?>(account);
        }

        public async Task<AccountResponse> SoftDeleteAccountAsync(Guid id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
            {
                throw new KeyNotFoundException($"Account with id {id} not found.");
            }
            account.IsDeleted = true;
            _accountRepository.Update(account);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<AccountResponse>(account);
        }

        public async Task<AccountResponse> RestoreAccountAsync(Guid id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
            {
                throw new KeyNotFoundException($"Account with id {id} not found.");
            }
            account.IsDeleted = false;
            _accountRepository.Update(account);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<AccountResponse>(account);
        }


    }
}
