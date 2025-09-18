using Application.Interfaces;
using Application.Interfaces.IDN;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.CORE;
using Domain.Interfaces.IDN;
using DTOs.IDN.IDN_Account.Requests;
using DTOs.IDN.IDN_Account.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
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
        private readonly IConfiguration _configuration;
        private readonly IIDN_RoleRepository _roleRepository;
        private readonly IIDN_StudentRepository _studentRepository;

        public IDN_AccountService(IIDN_AccountRepository accountRepository, ICORE_LookUpRepository lookUpRepository, IUnitOfWork unitOfWork, IMapper mapper,IPasswordHasher passwordHasher, IMailService mailService, IConfiguration configuration, IIDN_RoleRepository roleRepository, IIDN_StudentRepository studentRepository)
        {
            _accountRepository = accountRepository;
            _lookUpRepository = lookUpRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _mailService = mailService;
            _configuration = configuration;
            _roleRepository = roleRepository;
            _studentRepository = studentRepository;
        }

        public async Task<IReadOnlyList<AccountStatusResponse>> GetAccountStatusesAsync()
        {
            var lookup = await _lookUpRepository.GetByTypeAsync(LookUpTypes.AccountStatus);

            return _mapper.Map<IReadOnlyList<AccountStatusResponse>>(lookup);
        }

        public async Task<AccountResponse> CreateAccountAsync(CreateAccountRequest dto)
        {
            if (!await IsEmailUniqueAsync(dto.Email))
            {
                throw new InvalidOperationException($"Email {dto.Email} is already in use.");
            }
            if (!await IsPhoneUniqueAsync(dto.PhoneNumber))
            {
                throw new InvalidOperationException($"Phone number {dto.PhoneNumber} is already in use.");
            }
            var account = _mapper.Map<IDN_Account>(dto);

            var rawPassword = Guid.NewGuid().ToString("N")[..8];
            var verificationCode = GenerateVerificationCode();

            account.Password = _passwordHasher.HashPassword(rawPassword);
            
            var activeStatus = await _lookUpRepository.GetByCodeAsync(LookUpTypes.AccountStatus, AccountStatuses.Active.ToString());
            if (activeStatus == null)
            {
                throw new InvalidOperationException("Active status not found in lookup.");
            }

            //Set account attributes
            account.Id = Guid.NewGuid();
            account.AccountStatusID = activeStatus.Id;
            account.IsVerified = false;
            account.IsDeleted = false;
            account.VerifiedCode = HashVerificationCode(verificationCode); // Hash the verification code
            account.VerifiedCodeExpiresAt = DateTime.UtcNow.AddMinutes(15); // Code expires in 15 minutes
            account.IDN_AccountRoles = new List<IDN_AccountRole>
            {
                new IDN_AccountRole
                {
                    AccountID = account.Id,
                    RoleID = dto.RoleID
                }
            };

            _accountRepository.Add(account);
            await _unitOfWork.SaveChangesAsync();

            // Send verification email
            await SendVerificationEmailAsync(account.Email, account.FullName, verificationCode);

            var createdAccount = await _accountRepository.GetDetailByIdAsync(account.Id);

            return _mapper.Map<AccountResponse>(createdAccount);
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
                // Get default role (Student) for Google accounts
                var defaultRoleId = await _roleRepository.GetRoleIdByNameAsync("Student");
                if (defaultRoleId == Guid.Empty)
                {
                    throw new InvalidOperationException("Default role 'Student' not found. Please contact administrator.");
                }

                // Tạo tài khoản mới nếu chưa tồn tại
                account = new Domain.Entities.IDN_Account
                {
                    Id = Guid.NewGuid(),
                    Email = googleLoginRequest.Email,
                    FullName = googleLoginRequest.FullName,
                    AvatarUrl = googleLoginRequest.picture,
                    AccountStatusID = (await _lookUpRepository.GetByCodeAsync(LookUpTypes.AccountStatus, AccountStatuses.Active.ToString()))?.Id,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                    IsVerified = false,
                    // Các trường khác có thể để null hoặc giá trị mặc định
                };

                // Add default role for Google account
                account.IDN_AccountRoles = new List<IDN_AccountRole>
                {
                    new IDN_AccountRole
                    {
                        AccountID = account.Id,
                        RoleID = defaultRoleId
                    }
                };

                // Generate verification code for Google account
                var verificationCode = GenerateVerificationCode();
                account.VerifiedCode = HashVerificationCode(verificationCode); // Hash the verification code
                account.VerifiedCodeExpiresAt = DateTime.UtcNow.AddMinutes(15);
                
                _accountRepository.Add(account);
                await _unitOfWork.SaveChangesAsync();
                
                // Send verification email
                await SendVerificationEmailAsync(account.Email, account.FullName, verificationCode);
            }
            var accountResponse = _mapper.Map<AccountResponse>(account);
            return _mapper.Map<LoginAccountResponse>(accountResponse);
        }
        #endregion

        #region Account Verification
        public async Task<bool> VerifyAccountAsync(VerifyAccountRequest dto)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var account = await _accountRepository.GetDetailByIdAsync(await GetAccountIdByEmailAsync(dto.Email));
                if (account == null)
                {
                    throw new KeyNotFoundException($"Account with email {dto.Email} not found.");
                }
                if (account.IsVerified)
                {
                    throw new InvalidOperationException("Account is already verified.");
                }
                if (string.IsNullOrEmpty(account.VerifiedCode))
                {
                    throw new InvalidOperationException("No verification code found for this account.");
                }
                if (account.VerifiedCodeExpiresAt.HasValue && account.VerifiedCodeExpiresAt.Value < DateTime.UtcNow)
                {
                    throw new InvalidOperationException("Verification code has expired.");
                }
                if (!VerifyVerificationCode(dto.VerificationCode, account.VerifiedCode))
                {
                    throw new InvalidOperationException("Invalid verification code.");
                }

                // Mark account as verified and clear verification code
                account.IsVerified = true;
                account.VerifiedCode = null;
                account.VerifiedCodeExpiresAt = null;
                _accountRepository.Update(account);

                // Get Student role
                var studentRoleId = await _roleRepository.GetRoleIdByNameAsync("Student");
                if (studentRoleId != Guid.Empty)
                {
                    // Check if role already exists (avoid duplicates)
                    if (!account.IDN_AccountRoles.Any(ar => ar.RoleID == studentRoleId))
                    {
                        // Add Student role to the same tracked entity
                        var studentRole = new IDN_AccountRole
                        {
                            AccountID = account.Id,
                            RoleID = studentRoleId
                        };
                        account.IDN_AccountRoles.Add(studentRole);

                        // Create Student record
                        var student = new IDN_Student
                        {
                            Id = account.Id,
                            StudentCode = GenerateStudentCode(),
                            StudentNumber = await GetNextStudentNumberAsync(),
                            CreatedAt = DateTime.UtcNow,
                            IsDeleted = false
                        };
                        _studentRepository.Add(student);
                    }
                }

                // Single save operation for all changes
                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> ResendVerificationCodeAsync(string email)
        {
            var account = await _accountRepository.GetUserByEmailAsync(email);
            if (account == null)
            {
                throw new KeyNotFoundException($"Account with email {email} not found.");
            }

            if (account.IsVerified)
            {
                throw new InvalidOperationException("Account is already verified.");
            }

            // Generate new verification code
            var verificationCode = GenerateVerificationCode();
            account.VerifiedCode = HashVerificationCode(verificationCode); // Hash the verification code
            account.VerifiedCodeExpiresAt = DateTime.UtcNow.AddMinutes(15); // Code expires in 15 minutes

            _accountRepository.Update(account);
            await _unitOfWork.SaveChangesAsync();

            // Send verification email
            await SendVerificationEmailAsync(account.Email, account.FullName, verificationCode);

            return true;
        }
        #endregion

        #region Private Helper Methods
        private string GenerateVerificationCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString(); // Generate 6-digit code
        }

        private string HashVerificationCode(string code)
        {
            // Create a shorter hash using SHA256 and truncate to 20 characters
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(code));
            var base64Hash = Convert.ToBase64String(hashBytes);
            // Take first 20 characters and remove any special characters that might cause issues
            return base64Hash.Replace("+", "").Replace("/", "").Replace("=", "").Substring(0, Math.Min(20, base64Hash.Length));
        }

        private bool VerifyVerificationCode(string code, string hashedCode)
        {
            // Generate hash from input code and compare
            var inputHash = HashVerificationCode(code);
            return inputHash.Equals(hashedCode, StringComparison.OrdinalIgnoreCase);
        }

        private string GenerateStudentCode()
        {
            var currentYear = DateTime.Now.Year;
            var random = new Random();
            var randomPart = random.Next(1000, 9999);
            return $"ST{currentYear}{randomPart}"; // Format: ST20250001
        }

        private async Task<int> GetNextStudentNumberAsync()
        {
            var lastStudent = await _studentRepository.FindAsync(s => !s.IsDeleted);
            if (!lastStudent.Any())
            {
                return 1; // First student
            }
            
            var maxStudentNumber = lastStudent.Max(s => s.StudentNumber);
            return maxStudentNumber + 1;
        }

        private async Task<Guid> GetAccountIdByEmailAsync(string email)
        {
            var account = await _accountRepository.GetUserByEmailAsync(email);
            return account?.Id ?? Guid.Empty;
        }

        private async Task SendVerificationEmailAsync(string email, string fullName, string verificationCode)
        {
            string subject = "CETS Account Verification";

            // Create verification URL using configuration
            string apiBaseUrl = _configuration["VerificationSettings:ApiBaseUrl"] ?? "https://localhost:7000";
            string verificationUrl = $"{apiBaseUrl}/api/IDN_Account/verify-by-link?email={Uri.EscapeDataString(email)}&code={verificationCode}";

            string body = $@"
                <div style='max-width:600px;margin:0 auto;padding:20px;font-family:Arial,Helvetica,sans-serif;background:#ffffff;border-radius:8px;box-shadow:0 2px 6px rgba(0,0,0,0.1);'>
                  <!-- Logo -->
                  <div style='margin-bottom:20px;'>
                    <img src='https://i.ibb.co/0c2dT3L/cets-logo.png' alt='CETS Logo' style='height:40px;'>
                  </div>
                  <!-- Title -->
                  <div style='font-size:20px;font-weight:bold;color:#333;margin-bottom:10px;'>
                    CETS Account Verification
                  </div>
                  <!-- Greeting -->
                  <div style='font-size:16px;color:#333;margin-bottom:20px;'>
                    Hello {fullName},
                  </div>
                  <!-- Message -->
                  <div style='font-size:14px;color:#555;margin-bottom:20px;line-height:1.6;'>
                    Thank you for registering an account at CETS English Center. To complete your registration, please click the verification button below:
                  </div>
                  <!-- Verification Button -->
                  <div style='text-align:center;margin:30px 0;'>
                    <a href='{verificationUrl}' 
                       style='background:#4CAF50;color:#fff;padding:15px 30px;border-radius:8px;text-decoration:none;font-weight:bold;font-size:16px;display:inline-block;box-shadow:0 2px 4px rgba(0,0,0,0.1);'>
                      Verify My Account
                    </a>
                  </div>
                  <!-- Alternative Method -->
                  <div style='background:#f8f9fa;padding:15px;border-radius:6px;margin:20px 0;'>
                    <div style='font-size:12px;color:#666;margin-bottom:8px;'>
                      <strong>Alternative method:</strong> If the button doesn't work, copy and paste this link into your browser:
                    </div>
                    <div style='font-size:12px;color:#4CAF50;word-break:break-all;'>
                      {verificationUrl}
                    </div>
                  </div>
                  <!-- Instructions -->
                  <div style='font-size:14px;color:#555;margin-bottom:20px;line-height:1.6;'>
                    <strong>Instructions:</strong><br/>
                    1. Click the ""Verify My Account"" button above<br/>
                    2. You will be redirected to complete the verification<br/>
                    3. The verification link will expire in 15 minutes<br/>
                    4. If you did not request to create this account, please ignore this email
                  </div>
                  <!-- Footer -->
                  <div style='font-size:12px;color:#888;border-top:1px solid #e0e0e0;padding-top:20px;'>
                    You requested to create an account at CETS English Center.<br/><br/>
                    <a href='#' style='color:#4CAF50;text-decoration:none;'>Manage Preferences</a> | 
                    <a href='#' style='color:#4CAF50;text-decoration:none;'>Contact Us</a> | 
                    <a href='#' style='color:#4CAF50;text-decoration:none;'>Privacy Policy</a>
                    <br/><br/>
                    © 2025 CETS English Center. All rights reserved.<br/>
                    CETS, 123 ABC Street, District 1, Ho Chi Minh City.
                  </div>
                </div>";

            await _mailService.SendEmailAsync(email, subject, body);
        }
        #endregion

        #region Account Registration
        public async Task<AccountResponse> RegisterAsync(RegisterRequest dto)
        {
            // Check if email already exists
            if (!await IsEmailUniqueAsync(dto.Email))
            {
                throw new InvalidOperationException($"Email {dto.Email} is already in use.");
            }

            // Get default role (Student) - you can change this to any default role
            var defaultRoleId = await _roleRepository.GetRoleIdByNameAsync("Student");
            if (defaultRoleId == Guid.Empty)
            {
                throw new InvalidOperationException("Default role 'Student' not found. Please contact administrator.");
            }

            // Create account entity
            var account = new IDN_Account
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                FullName = dto.FullName,
                Password = _passwordHasher.HashPassword(dto.Password),
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false,
                IsVerified = false
            };

            // Generate verification code
            var verificationCode = GenerateVerificationCode();
            account.VerifiedCode = HashVerificationCode(verificationCode); // Hash the verification code
            account.VerifiedCodeExpiresAt = DateTime.UtcNow.AddMinutes(15);

            // Get active status
            var activeStatus = await _lookUpRepository.GetByCodeAsync(LookUpTypes.AccountStatus, AccountStatuses.Active.ToString());
            if (activeStatus == null)
            {
                throw new InvalidOperationException("Active status not found in lookup.");
            }
            account.AccountStatusID = activeStatus.Id;

            // Add default role
            account.IDN_AccountRoles = new List<IDN_AccountRole>
            {
                new IDN_AccountRole
                {
                    AccountID = account.Id,
                    RoleID = defaultRoleId
                }
            };

            // Save account
            _accountRepository.Add(account);
            await _unitOfWork.SaveChangesAsync();

            // Send verification email
            await SendVerificationEmailAsync(account.Email, account.FullName, verificationCode);

            // Return created account
            var createdAccount = await _accountRepository.GetDetailByIdAsync(account.Id);
            return _mapper.Map<AccountResponse>(createdAccount);
        }
        #endregion
    }
}
