using Application.Interfaces.Common.Email;
using Application.Interfaces.ExternalServices.Security;
using Application.Interfaces.IDN;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.CORE;
using Domain.Interfaces.IDN;
using DTOs.IDN.IDN_Account.Responses;
using DTOs.IDN.IDN_Teacher.Requests;
using DTOs.IDN.IDN_Teacher.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Implementations.IDN
{
    public class IDN_TeacherService : IIDN_TeacherService
    {
        private readonly IIDN_TeacherRepository _teacherRepository;
        private readonly IIDN_TeacherCredentialRepository _teacherCredentialRepository;
        private readonly IIDN_AccountRepository _accountRepository;
        private readonly IIDN_RoleRepository _roleRepository;
        private readonly ICORE_LookUpRepository _lookUpRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMailService _mailService;

        public IDN_TeacherService(IIDN_TeacherRepository teacherRepository, 
            IIDN_TeacherCredentialRepository teacherCredentialRepository,
            IIDN_AccountRepository accountRepository, 
            IIDN_RoleRepository roleRepository,
            ICORE_LookUpRepository lookUpRepository,
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IPasswordHasher passwordHasher,
            IMailService mailService)
        {
            _teacherRepository = teacherRepository;
            _teacherCredentialRepository = teacherCredentialRepository;
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _lookUpRepository = lookUpRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _mailService = mailService;
        }

        private string HashCID(string cid)
        {
            // Hash CID using SHA256 for security
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(cid));
            var hashHex = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            return hashHex;
        }

        /* Get methods */
        public async Task<IReadOnlyList<TeacherResponse>> GetAllTeachersAsync()
        {
            var teachers = await _teacherRepository.GetAllAsync();
            return _mapper.Map<IReadOnlyList<TeacherResponse>>(teachers);
        }
        public async Task<TeacherResponse?> GetTeacherByIdAsync(Guid id)
        {
            var teacher = await _teacherRepository.GetByIdAsync(id);
            return _mapper.Map<TeacherResponse?>(teacher);
        }

        public async Task<TeacherResponse?> GetTeacherByCodeAsync(string teacherCode)
        {
            var teacher = await _teacherRepository.FindFirstAsync(t => t.TeacherCode == teacherCode && !t.IsDeleted);
            return _mapper.Map<TeacherResponse?>(teacher);
        }
        public async Task<TeacherResponse?> GetTeacherByEmailAsync(string email)
        {
            var teacher = await _teacherRepository.FindFirstAsync(t => t.Account.Email == email && !t.IsDeleted);
            return _mapper.Map<TeacherResponse?>(teacher);
        }

        public async Task<TeacherDetailResponse?> GetTeacherDetailsAsync(Guid id)
        {
            var teacher = await _teacherRepository.GetTeacherDetailsByIdAsync(id);
            return _mapper.Map<TeacherDetailResponse?>(teacher);
        }

        public async Task<TeacherDetailResponse> CreateTeacherWithAccountAsync(CreateTeacherRequest dto)
        {
            if (!await _accountRepository.IsEmailUniqueAsync(dto.Email))
                throw new InvalidOperationException("Email already exists.");
            if (await _accountRepository.IsPhoneUniqueAsync(dto.PhoneNumber))
                throw new InvalidOperationException("Phone number already exists.");
            if (string.IsNullOrWhiteSpace(dto.FullName))
                throw new ArgumentException("FullName is required.");

            var account = _mapper.Map<IDN_Account>(dto);
            var activeStatus = await _lookUpRepository.GetByCodeAsync(LookUpTypes.AccountStatus, AccountStatuses.Active.ToString());
            var rawPassword = Guid.NewGuid().ToString("N")[..8];

            //Set account attributes
            account.Id = Guid.NewGuid();
            account.AccountStatusID = activeStatus.Id;
            account.Password = _passwordHasher.HashPassword(rawPassword);
            account.IsVerified = false;
            
            // Hash CID for security
            if (!string.IsNullOrWhiteSpace(account.CID))
                account.CID = HashCID(account.CID);

            account.IDN_AccountRoles = new List<IDN_AccountRole>
            {
                new IDN_AccountRole
                {
                    RoleID = await _roleRepository.GetRoleIdByNameAsync("Teacher"),
                    AccountID = account.Id
                }
            };

            _accountRepository.Add(account);

            var teacher = _mapper.Map<IDN_Teacher>(dto);
            teacher.Id = account.Id;
            teacher.CreatedAt = DateTime.UtcNow;

            foreach (var credDto in dto.Credentials)
            {
                var credentialType = await _lookUpRepository.GetByIdAsync(credDto.CredentialTypeId);
                if (credentialType == null)
                    throw new InvalidOperationException($"CredentialType {credDto.CredentialTypeId} không tồn tại.");

                if (string.IsNullOrWhiteSpace(credDto.Name))
                    throw new ArgumentException("Credential name is required.");
                if (string.IsNullOrWhiteSpace(credDto.Level))
                    throw new ArgumentException("Credential level is required.");

                var credential = _mapper.Map<IDN_TeacherCredential>(credDto);
                teacher.IDN_TeacherCredentials.Add(credential);
            }

            _teacherRepository.Add(teacher);
            await _unitOfWork.SaveChangesAsync();

            // Send login credentials email to teacher
            await SendLoginCredentialsEmailAsync(account.Email, account.FullName, rawPassword, "Teacher");

            var createdTeacher = await _teacherRepository.GetTeacherDetailsByIdAsync(teacher.Id);

            var response = _mapper.Map<TeacherDetailResponse>(createdTeacher);

            return response;
        }

        private async Task SendLoginCredentialsEmailAsync(string email, string fullName, string password, string roleName)
        {
            string subject = "CETS Account Created - Login Credentials";
            string roleDisplayName = roleName switch
            {
                "Teacher" => "Teacher",
                _ => "Staff"
            };

            string body = $@"
                <div style='max-width:600px;margin:0 auto;padding:20px;font-family:Arial,Helvetica,sans-serif;background:#ffffff;border-radius:8px;box-shadow:0 2px 6px rgba(0,0,0,0.1);'>
                  <!-- Logo -->
                  <div style='margin-bottom:20px;'>
                    <img src='https://i.ibb.co/0c2dT3L/cets-logo.png' alt='CETS Logo' style='height:40px;'>
                  </div>
                  <!-- Title -->
                  <div style='font-size:20px;font-weight:bold;color:#333;margin-bottom:10px;'>
                    Welcome to CETS English Center
                  </div>
                  <!-- Greeting -->
                  <div style='font-size:16px;color:#333;margin-bottom:20px;'>
                    Hello {fullName},
                  </div>
                  <!-- Message -->
                  <div style='font-size:14px;color:#555;margin-bottom:20px;line-height:1.6;'>
                    Your {roleDisplayName} account has been successfully created at CETS English Center. Below are your login credentials:
                  </div>
                  <!-- Credentials Box -->
                  <div style='background:#f8f9fa;padding:20px;border-radius:6px;margin:20px 0;border-left:4px solid #4CAF50;'>
                    <div style='font-size:14px;color:#333;margin-bottom:10px;'>
                      <strong>Email:</strong> {email}
                    </div>
                    <div style='font-size:14px;color:#333;'>
                      <strong>Password:</strong> <span style='font-family:monospace;font-size:16px;color:#4CAF50;font-weight:bold;'>{password}</span>
                    </div>
                  </div>
                  <!-- Security Notice -->
                  <div style='background:#fff3cd;padding:15px;border-radius:6px;margin:20px 0;border-left:4px solid #ffc107;'>
                    <div style='font-size:13px;color:#856404;'>
                      <strong>⚠️ Security Notice:</strong><br/>
                      Please change your password after your first login for security purposes. Do not share your password with anyone.
                    </div>
                  </div>
                  <!-- Instructions -->
                  <div style='font-size:14px;color:#555;margin-bottom:20px;line-height:1.6;'>
                    <strong>Next Steps:</strong><br/>
                    1. Use the credentials above to log in to the CETS system<br/>
                    2. Change your password immediately after first login<br/>
                    3. If you have any questions, please contact the administrator
                  </div>
                  <!-- Footer -->
                  <div style='font-size:12px;color:#888;border-top:1px solid #e0e0e0;padding-top:20px;'>
                    This is an automated message from CETS English Center.<br/><br/>
                    <a href='#' style='color:#4CAF50;text-decoration:none;'>Contact Us</a> | 
                    <a href='#' style='color:#4CAF50;text-decoration:none;'>Privacy Policy</a>
                    <br/><br/>
                    © 2025 CETS English Center. All rights reserved.<br/>
                    CETS, 123 ABC Street, District 1, Ho Chi Minh City.
                  </div>
                </div>";

            await _mailService.SendEmailAsync(email, subject, body);
        }


        /* Put methods */
        public async Task<TeacherResponse> UpdateTeacherAsync(Guid id, UpdateTeacherRequest dto)
        {
            var teacher = await _teacherRepository.GetByIdAsync(id);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"Teacher with id {id} not found.");
            }
            _mapper.Map(dto, teacher);
            _teacherRepository.Update(teacher);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TeacherResponse>(teacher);
        }

        //public async Task<TeacherDetailResponse?> UpdateTeacherProfileAsync(Guid teacherId, UpdateTeacherProfileRequest dto, ClaimsPrincipal user)
        //{
        //    var teacher = await _teacherRepository.GetTeacherDetailsByIdAsync(teacherId);
        //    if (teacher == null) return null;

        //    teacher.TeacherCode = dto.TeacherCode ?? teacher.TeacherCode;
        //    teacher.YearsExperience = dto.YearsExperience ?? teacher.YearsExperience;
        //    teacher.Bio = dto.Bio ?? teacher.Bio;

        //    teacher.Account.FullName = dto.FullName ?? teacher.Account.FullName;
        //    teacher.Account.DateOfBirth = dto.DateOfBirth ?? teacher.Account.DateOfBirth;
        //    teacher.Account.CID = dto.CID ?? teacher.Account.CID;
        //    teacher.Account.Address = dto.Address ?? teacher.Account.Address;
        //    teacher.Account.AvatarUrl = dto.AvatarUrl ?? teacher.Account.AvatarUrl;

        //    teacher.UpdatedAt = DateTime.UtcNow;
        //    teacher.Account.UpdatedAt = DateTime.UtcNow;

        //    _teacherRepository.Update(teacher);
        //    await _unitOfWork.SaveChangesAsync();
        //    return _mapper.Map<TeacherDetailResponse>(teacher);
        //}
        public async Task<TeacherDetailResponse?> UpdateTeacherProfileAsync(
       Guid teacherId,
       UpdateTeacherProfileRequest dto,
       ClaimsPrincipal user)
        {
            var teacher = await _teacherRepository.GetTeacherDetailsByIdAsync(teacherId);
            if (teacher == null)
            {
                Debug.WriteLine($"❌ Teacher {teacherId} not found in DB");
                return null;
            }

            // --- Update profile ---
            _mapper.Map(dto, teacher);
            _mapper.Map(dto, teacher.Account);

            teacher.UpdatedAt = DateTime.UtcNow;
            teacher.Account.UpdatedAt = DateTime.UtcNow;

            Debug.WriteLine($"✅ Updating Teacher {teacher.Id}, Code={teacher.TeacherCode}, Name={teacher.Account.FullName}");
     
            // --- Handle credentials (Remove / Update / Add) ---
            var existingCreds = teacher.IDN_TeacherCredentials.ToList();
            var incomingIds = dto.Credentials?
                .Where(c => c.CredentialId.HasValue && c.CredentialId.Value != Guid.Empty)
                .Select(c => c.CredentialId!.Value)
                .ToHashSet() ?? new HashSet<Guid>();

            // 1️⃣ Remove các credential không còn trong DTO
            var toRemove = existingCreds.Where(c => !incomingIds.Contains(c.Id)).ToList();
            foreach (var removeCred in toRemove)
            {
                Debug.WriteLine($"🗑️ Removing credential {removeCred.Id} ({removeCred.Name})");
                teacher.IDN_TeacherCredentials.Remove(removeCred);
            }

            // --- Update / Add credentials ---
            if (dto.Credentials?.Any() == true)
            {
                foreach (var credDto in dto.Credentials)
                {
                    Debug.WriteLine($"👉 CredentialDto: Id={credDto.CredentialId}, TypeId={credDto.CredentialTypeId}, Name={credDto.Name}");

                    if (credDto.CredentialId.HasValue && credDto.CredentialId.Value != Guid.Empty)
                    {
                        var existing = existingCreds
                            .FirstOrDefault(c => c.Id == credDto.CredentialId.Value);

                        if (existing != null)
                        {
                            Debug.WriteLine($"🔄 Updating existing credential {existing.Id}");
                            _mapper.Map(credDto, existing);
                            existing.UpdatedAt = DateTime.UtcNow;
                        }
                        else
                        {
                            Debug.WriteLine($"⚠️ Client gửi CredentialId={credDto.CredentialId} nhưng DB không có → ADD");
                            var newCred = _mapper.Map<IDN_TeacherCredential>(credDto);
                            newCred.Id = Guid.NewGuid();
                            newCred.TeacherID = teacherId;
                            newCred.UpdatedAt = DateTime.UtcNow;
                            teacher.IDN_TeacherCredentials.Add(newCred);
                        }
                    }
                    else
                    {
                        Debug.WriteLine("➕ Adding NEW credential (no Id or Guid.Empty)");

                        var newCred = _mapper.Map<IDN_TeacherCredential>(credDto);

                        // Đảm bảo không bị copy Id từ DTO
                        newCred.Id = Guid.NewGuid();
                        newCred.TeacherID = teacherId;
                        newCred.UpdatedAt = DateTime.UtcNow;

                        // Force trạng thái = Added
                        _teacherCredentialRepository.Add(newCred); 

                        //teacher.IDN_TeacherCredentials.Add(newCred);
                    }


                }
            }

            // Trước khi SaveChanges: in ra tất cả credentials
            foreach (var c in teacher.IDN_TeacherCredentials)
            {
                Debug.WriteLine($"📌 Credential in memory: Id={c.Id}, Teacher={c.TeacherID}, Type={c.CredentialTypeID}, Name={c.Name}");
            }


            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TeacherDetailResponse>(teacher);
        }







        public async Task<TeacherResponse> RestoreTeacherAsync(Guid id)
        {
            var teacher = await _teacherRepository.GetByIdAsync(id);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"Teacher with id {id} not found.");
            }
            teacher.IsDeleted = false;
            _teacherRepository.Update(teacher);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TeacherResponse>(teacher);
        }


        /* Delete methods */
        public async Task<TeacherResponse> SoftDeleteTeacherAsync(Guid id)
        {
            var teacher = await _teacherRepository.GetByIdAsync(id);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"Teacher with id {id} not found.");
            }
            teacher.IsDeleted = true;
            _teacherRepository.Update(teacher);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TeacherResponse>(teacher);
        }

    }
}
