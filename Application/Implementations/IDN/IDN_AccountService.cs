using Application.Interfaces.IDN;
using AutoMapper;
using Domain.Constants;
using Domain.Interfaces;
using Domain.Interfaces.CORE;
using Domain.Interfaces.IDN;
using DTOs.IDN.IDN_Account.Requests;
using DTOs.IDN.IDN_Account.Responses;

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

        public async Task<IReadOnlyList<AccountResponse>> GetAllAccountsAsync()
        {
            var account = await _accountRepository.GetAllAsync();
            return _mapper.Map<IReadOnlyList<AccountResponse>>(account);
        }

        public async Task<AccountResponse?> GetAccountByIdAsync(Guid id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
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
