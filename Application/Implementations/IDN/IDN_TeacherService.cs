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

        public IDN_TeacherService(IIDN_TeacherRepository teacherRepository, 
            IIDN_TeacherCredentialRepository teacherCredentialRepository,
            IIDN_AccountRepository accountRepository, 
            IIDN_RoleRepository roleRepository,
            ICORE_LookUpRepository lookUpRepository,
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IPasswordHasher passwordHasher)
        {
            _teacherRepository = teacherRepository;
            _teacherCredentialRepository = teacherCredentialRepository;
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _lookUpRepository = lookUpRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
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
            if (!string.IsNullOrEmpty(dto.PhoneNumber) &&
                !await _accountRepository.IsPhoneUniqueAsync(dto.PhoneNumber))
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

            var createdTeacher = await _teacherRepository.GetTeacherDetailsByIdAsync(teacher.Id);

            var response = _mapper.Map<TeacherDetailResponse>(createdTeacher);

            return response;
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
