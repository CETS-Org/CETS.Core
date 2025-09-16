using Application.Interfaces;
using Application.Interfaces.IDN;
using AutoMapper;
using Domain.Constants;
using Domain.Interfaces;
using Domain.Interfaces.CORE;
using Domain.Interfaces.IDN;
using DTOs.IDN.IDN_Account.Requests;
using DTOs.IDN.IDN_Account.Responses;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Implementations.IDN
{
    public class IDN_AccountService : IIDN_AccountService
    {
        private readonly IIDN_AccountRepository _accountRepository;
        private readonly ICORE_LookUpRepository _lookUpRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMailService _mailService;

        public IDN_AccountService(IIDN_AccountRepository accountRepository, ICORE_LookUpRepository lookUpRepository, IUnitOfWork unitOfWork, IMapper mapper,IPasswordHasher passwordHasher, IMailService mailService)
        {
            _accountRepository = accountRepository;
            _lookUpRepository = lookUpRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _mailService = mailService;
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

            // Sort
            if (!string.IsNullOrEmpty(filter.SortBy))
            {
                bool isDesc = !string.IsNullOrEmpty(filter.SortOrder) &&
                              filter.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

                switch (filter.SortBy.ToLower())
                {
                    case "email":
                        query = isDesc ? query.OrderByDescending(a => a.Email)
                                       : query.OrderBy(a => a.Email);
                        break;
                    case "createdat":
                        query = isDesc ? query.OrderByDescending(a => a.CreatedAt)
                                       : query.OrderBy(a => a.CreatedAt);
                        break;
                    default:
                        query = isDesc ? query.OrderByDescending(a => a.FullName)
                                       : query.OrderBy(a => a.FullName);
                        break;
                }
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

        public async Task<AccountResponse?> UpdateAccountProfileAsync(Guid accountId,UpdateAccountProfileRequest dto, ClaimsPrincipal user)
        {
            var account = await _accountRepository.GetByIdAsync(accountId);
            if (account == null || account.IsDeleted)
            {
                return null;
            }

            // Lấy updaterId từ user
            Guid? updaterId = null;
            var subClaim = user.FindFirst("sub");
            if (subClaim != null && Guid.TryParse(subClaim.Value, out var parsedId))
            {
                updaterId = parsedId;
            }

            // Update các field profile
            if (!string.IsNullOrWhiteSpace(dto.FullName))
                account.FullName = dto.FullName;

            if (dto.DateOfBirth.HasValue)
                account.DateOfBirth = dto.DateOfBirth;

            if (!string.IsNullOrWhiteSpace(dto.CID))
                account.CID = dto.CID;

            if (!string.IsNullOrWhiteSpace(dto.Address))
                account.Address = dto.Address;

            if (!string.IsNullOrWhiteSpace(dto.AvatarUrl))
                account.AvatarUrl = dto.AvatarUrl;

            account.UpdatedAt = DateTime.UtcNow;
            account.UpdatedBy = updaterId; 

            _accountRepository.Update(account);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<AccountResponse?>(account);
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

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            var user = await _accountRepository.GetUserByEmailAsync(email);
            return user != null;
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return await _accountRepository.IsEmailUniqueAsync(email);
        }

        public async Task<bool> IsPhoneNumberExistsAsync(string phoneNumber)
        {
            var user = await _accountRepository.GetUserByPhoneAsync(phoneNumber);
            return user != null;
        }

        public async Task<bool> IsPhoneUniqueAsync(string phoneNumber)
        {
            return await _accountRepository.IsPhoneUniqueAsync(phoneNumber);
        }

        #region Validate User Credentials
        // Validate user credentials
        public async Task<LoginAccountResponse?> ValidateUserCredentialsAsync(string email, string password)
        {
            var account = await _accountRepository.GetUserByEmailAsync(email);
            if (account == null || !_passwordHasher.VerifyPassword(password, account.Password!))
            {
                return null;
            }
            var accountResponse = _mapper.Map<AccountResponse>(account);
            return _mapper.Map<LoginAccountResponse>(accountResponse);
        }
        #endregion

        #region Validate Google Account
        public async Task<LoginAccountResponse> ValidateGoogleAccountAsync(GoogleLoginRequest googleLoginRequest)
        {
            var account = await _accountRepository.GetUserByEmailAsync(googleLoginRequest.Email);
            if (account == null)
            {
                // Tạo tài khoản mới nếu chưa tồn tại
                account = new Domain.Entities.IDN_Account
                {
                    Email = googleLoginRequest.Email,
                    FullName = googleLoginRequest.FullName,
                    AvatarUrl = googleLoginRequest.picture,
                    AccountStatusID = (await _lookUpRepository.GetByCodeAsync(LookUpTypes.AccountStatus, AccountStatuses.Active.ToString()))?.Id,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                    IsVerified = false,
                    // Các trường khác có thể để null hoặc giá trị mặc định
                };
                _accountRepository.Add(account);
                await _unitOfWork.SaveChangesAsync();
                string subject = "Your OTP Code for Verification";
                string body = $@"
                <div style='font-family:Arial, sans-serif; font-size:16px; color:#333; padding:20px;'>
                    <h2 style='color:#007bff;'>Email Verification</h2>
                    <p>Dear user,</p>
                    <p>Your One-Time Password (OTP) is:</p>
                    <p>This code is valid for the next 10 minutes.</p>
                    <p>If you did not request this, please ignore this email.</p>
                    <br/>
                    <p>Thanks,<br/>Your App Team</p>
                </div>";
                await _mailService.SendEmailAsync(account.Email, subject, body);
            }
            var accountResponse = _mapper.Map<AccountResponse>(account);
            return _mapper.Map<LoginAccountResponse>(accountResponse);
        }
        #endregion
    }
}
