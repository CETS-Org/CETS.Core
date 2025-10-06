using Application.Interfaces.IDN;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.IDN;
using DTOs.IDN.IDN_Student.Requests;
using DTOs.IDN.IDN_Student.Responses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations.IDN
{
    public class IDN_StudentService : IIDN_StudentService
    {
        private readonly IIDN_StudentRepository _studentRepository;
        private readonly IIDN_AccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public IDN_StudentService(IIDN_StudentRepository studentRepository, IIDN_AccountRepository accountRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /* Get methods */
        public async Task<IReadOnlyList<StudentResponse>> GetAllStudentsAsync()
        {
            var students = await _studentRepository.GetAllAsync();
            return _mapper.Map<IReadOnlyList<StudentResponse>>(students);
        }
        public async Task<StudentResponse?> GetStudentByIdAsync(Guid id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            return _mapper.Map<StudentResponse?>(student);
        }

        public async Task<StudentResponse?> GetStudentByCodeAsync(string code)
        {
            var student = await _studentRepository.FindFirstAsync(st => st.StudentCode == code);
            return _mapper.Map<StudentResponse?>(student);
        }

        /* Post methods */
        public async Task<StudentResponse> CreateStudentAsync(CreateStudentRequest dto)
        {
            var existingAccount = await _accountRepository.GetByIdAsync(dto.AccountId);
            if (existingAccount == null)
            {
                throw new KeyNotFoundException($"Account with ID {dto.AccountId} not found.");
            }

            var student = _mapper.Map<IDN_Student>(dto);
            student.IsDeleted = false;
            //student.CreatedAt = DateTime.Now;

            _studentRepository.Add(student);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<StudentResponse>(student);
        }


        /* Update methods */
        public async Task<StudentResponse?> UpdateStudentAsync(Guid id, UpdateStudentRequest dto)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                throw new KeyNotFoundException($"Student with id {id} not found.");
            }
            _mapper.Map(dto, student);
            _studentRepository.Update(student);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<StudentResponse>(student);
        }

        public async Task<StudentProfileResponse?> UpdateStudentProfileAsync(
    Guid accountId,
    UpdateStudentProfileRequest dto,
    ClaimsPrincipal user)
        {
            var student = await _studentRepository.GetStudentWithAccountAsync(accountId);
            if (student == null) return null;

            var account = student.Account ?? throw new InvalidOperationException("Student does not have linked Account");

            bool isPrivileged = user.IsInRole("AcademicStaff") || user.IsInRole("Admin");

            // Kiểm tra quyền trước khi update CID
           // if (!isPrivileged && !string.IsNullOrEmpty(dto.CID) && dto.CID != account.CID)
           //     throw new UnauthorizedAccessException("Student is not allowed to update CID");

            // Map DTO vào student + account
            _mapper.Map(dto, account);
            _mapper.Map(dto, student);

            // Cập nhật thời gian sửa đổi
            account.UpdatedAt = DateTime.UtcNow;
            student.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<StudentProfileResponse>(student);
        }


        public async Task<StudentResponse> RestoreStudentAsync(Guid id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                throw new KeyNotFoundException($"Student with id {id} not found.");
            }
            student.IsDeleted = false;
            _studentRepository.Update(student);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<StudentResponse>(student);
        }

        /* Delete methods */
        public async Task<StudentResponse> SoftDeleteStudentAsync(Guid id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                throw new KeyNotFoundException($"Student with id {id} not found.");
            }
            student.IsDeleted = true;
            _studentRepository.Update(student);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<StudentResponse>(student);
        }
    }
}
