using Application.Interfaces.ACAD;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
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

        public SubmissionService(
            IACAD_SubmissionRepository submissionRepository,
            IUnitOfWork unitOfWork)
        {
            _submissionRepository = submissionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ACAD_Submission> SubmitAssignmentAsync(ACAD_Submission submission)
        {
            _submissionRepository.Add(submission);
            await _unitOfWork.SaveChangesAsync();
            return submission;
        }

        public async Task<IEnumerable<ACAD_Submission>> GetSubmissionsByAssignmentAsync(Guid assignmentId)
            => await _submissionRepository.GetByAssignmentAsync(assignmentId);

        public async Task<IEnumerable<ACAD_Submission>> GetSubmissionsByStudentAsync(Guid studentId)
            => await _submissionRepository.GetByStudentAsync(studentId);

        public async Task GradeSubmissionAsync(Guid submissionId, decimal score, string? feedback)
        {
            var submission = await _submissionRepository.GetByIdAsync(submissionId);
            if (submission == null) throw new Exception("Submission not found");

            submission.Score = score;
            submission.Feedback = feedback;
            _submissionRepository.Update(submission);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
