using Application.Interfaces.ACAD;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_Submission.Requests;
using DTOs.ACAD.ACAD_Submission.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations.ACAD
{
    public class SubmissionService : ISubmissionService
    {
        private readonly IACAD_SubmissionRepository _submissionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubmissionService(
            IACAD_SubmissionRepository submissionRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _submissionRepository = submissionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SubmissionResponse> SubmitAssignmentAsync(SubmitAssignmentRequest request)
        {
            var entity = _mapper.Map<ACAD_Submission>(request);
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;

            _submissionRepository.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<SubmissionResponse>(entity);
        }

        public async Task<IEnumerable<SubmissionResponse>> GetSubmissionsByAssignmentAsync(Guid assignmentId)
        {
            var submissions = await _submissionRepository.GetByAssignmentAsync(assignmentId);
            return _mapper.Map<IEnumerable<SubmissionResponse>>(submissions);
        }

        public async Task<IEnumerable<SubmissionResponse>> GetSubmissionsByStudentAsync(Guid studentId)
        {
            var submissions = await _submissionRepository.GetByStudentAsync(studentId);
            return _mapper.Map<IEnumerable<SubmissionResponse>>(submissions);
        }

        public async Task<SubmissionResponse> GradeSubmissionAsync(GradeSubmissionRequest request)
        {
            var entity = await _submissionRepository.GetByIdAsync(request.SubmissionID)
                         ?? throw new KeyNotFoundException("Submission not found");

            entity.Score = request.Score;
            entity.Feedback = request.Feedback;

            _submissionRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<SubmissionResponse>(entity);
        }
    }
}
