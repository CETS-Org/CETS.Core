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
    public class AssignmentService : IACAD_AssignmentService
    {
        private readonly IACAD_AssignmentRepository _assignmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssignmentService(
            IACAD_AssignmentRepository assignmentRepository,
            IUnitOfWork unitOfWork)
        {
            _assignmentRepository = assignmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ACAD_Assignment> CreateAssignmentAsync(ACAD_Assignment assignment)
        {
            _assignmentRepository.Add(assignment);
            await _unitOfWork.SaveChangesAsync();
            return assignment;
        }

        public async Task<IEnumerable<ACAD_Assignment>> GetAssignmentsByClassMeetingAsync(Guid classMeetingId)
            => await _assignmentRepository.GetByClassMeetingAsync(classMeetingId);

        public async Task<IEnumerable<ACAD_Assignment>> GetAssignmentsByTeacherAsync(Guid teacherId)
            => await _assignmentRepository.GetByTeacherAsync(teacherId);

        public async Task<ACAD_Assignment?> GetAssignmentByIdAsync(Guid id)
            => await _assignmentRepository.GetByIdAsync(id);

        public async Task UpdateAssignmentAsync(ACAD_Assignment assignment)
        {
            _assignmentRepository.Update(assignment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAssignmentAsync(Guid id)
        {
            await _assignmentRepository.RemoveByIdAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
