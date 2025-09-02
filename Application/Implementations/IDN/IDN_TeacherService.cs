using Application.Interfaces.IDN;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.IDN;
using DTOs.IDN_Teacher.Requests;
using DTOs.IDN_Teacher.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations.IDN
{
    public class IDN_TeacherService : IIDN_TeacherService
    {
        private readonly IIDN_TeacherRepository _teacherRepository;
        private readonly IIDN_AccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public IDN_TeacherService(IIDN_TeacherRepository teacherRepository, IIDN_AccountRepository accountRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _teacherRepository = teacherRepository;
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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


        /* Post methods */
        public async Task<TeacherResponse> CreateTeacherAsync(CreateTeacherRequest dto)
        {
            var account = await _accountRepository.GetByIdAsync(dto.AccountId);
            if (account == null || account.IsDeleted)
            {
                throw new KeyNotFoundException($"Account with id {dto.AccountId} not found.");
            }

            var existingTeacher = await _teacherRepository.FindFirstAsync(t => t.Id == dto.AccountId);
            if (existingTeacher != null)
            {
                throw new InvalidOperationException($"A teacher already exists for account {dto.AccountId}.");
            }

            var teacher = _mapper.Map<IDN_Teacher>(dto);

            _teacherRepository.Add(teacher);
            
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TeacherResponse>(teacher);
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
