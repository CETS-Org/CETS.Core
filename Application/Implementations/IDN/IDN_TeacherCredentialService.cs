using Application.Interfaces.IDN;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.CORE;
using Domain.Interfaces.IDN;
using DTOs.IDN_TeacherCredential.Requests;
using DTOs.IDN_TeacherCredential.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations.IDN
{
    public class IDN_TeacherCredentialService : BaseService<IDN_TeacherCredential, TeacherCredentialResponse, UpdateTeacherCredentialRequest, CreateTeacherCredentialRequest>,IIDN_TeacherCredentialService
    {
        private readonly ICORE_LookUpRepository _lookUpRepository;
        public IDN_TeacherCredentialService(IIDN_TeacherCredentialRepository repository, ICORE_LookUpRepository lookUpRepository, IUnitOfWork unitOfWork, IMapper mapper) 
            : base(repository, unitOfWork, mapper)
        {
            _lookUpRepository = lookUpRepository;
        }

        public async Task<IReadOnlyList<TeacherCredentialResponse>> GetCredentialsByTeacherIdAsync(Guid teacherId)
        {
            var credentials = await _repository.FindFirstAsync(cr => cr.Teacher.Id == teacherId);
            return _mapper.Map<IReadOnlyList<TeacherCredentialResponse>>(credentials);
        }
        public async Task<IReadOnlyList<TeacherCredentialResponse>> GetCredentialsByTeacherCodeAsync(string teacherCode)
        {
            var credentials = await _repository.FindFirstAsync(cr => cr.Teacher.TeacherCode == teacherCode);
            return _mapper.Map<IReadOnlyList<TeacherCredentialResponse>>(credentials);
        }
        public async Task<IReadOnlyList<CredentialTypeResponse>> GetCredentialTypesAsync()
        {
            var lookup = await _lookUpRepository.GetByTypeAsync(LookUpTypes.CredentialType);
            return _mapper.Map<IReadOnlyList<CredentialTypeResponse>>(lookup);
        }
    }
}
