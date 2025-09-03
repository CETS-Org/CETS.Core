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
    public class ACAD_EnrollmentService : IACAD_EnrollmentService
    {
        private readonly IACAD_EnrollmentRepository _enrollmentRepo;
        private readonly IUnitOfWork _unitOfWork;

        public ACAD_EnrollmentService(
            IACAD_EnrollmentRepository enrollmentRepo,
            IUnitOfWork unitOfWork)
        {
            _enrollmentRepo = enrollmentRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<ACAD_Enrollment> EnrollAsync(Guid studentId, Guid courseId, Guid? classId)
        {
            var enrollment = new ACAD_Enrollment
            {
                Id = Guid.NewGuid(),
                StudentID = studentId,
                CourseID = courseId,
                ClassID = classId,
                EnrollmentStatusID = Guid.Empty,
                CreatedAt = DateTime.UtcNow
            };

            _enrollmentRepo.Add(enrollment);
            await _unitOfWork.SaveChangesAsync();
            return enrollment;
        }

        public async Task ApproveEnrollmentAsync(Guid enrollmentId, Guid staffId)
        {
            var enrollment = await _enrollmentRepo.GetByIdAsync(enrollmentId);
            if (enrollment == null) throw new Exception("Enrollment not found");

            enrollment.EnrollmentStatusID = Guid.Empty; 
            enrollment.UpdatedBy = staffId;
            enrollment.UpdatedAt = DateTime.UtcNow;

            _enrollmentRepo.Update(enrollment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RejectEnrollmentAsync(Guid enrollmentId, Guid staffId, string reason)
        {
            var enrollment = await _enrollmentRepo.GetByIdAsync(enrollmentId);
            if (enrollment == null) throw new Exception("Enrollment not found");

            enrollment.EnrollmentStatusID = Guid.Empty; 
            enrollment.UpdatedBy = staffId;
            enrollment.UpdatedAt = DateTime.UtcNow;

            _enrollmentRepo.Update(enrollment);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<IEnumerable<ACAD_Enrollment>> GetStudentEnrollmentsAsync(Guid studentId)
            => await _enrollmentRepo.GetByStudentAsync(studentId);

        public async Task<IEnumerable<ACAD_Enrollment>> GetClassEnrollmentsAsync(Guid classId)
            => await _enrollmentRepo.GetByClassAsync(classId);

        public async Task<ACAD_Enrollment?> GetEnrollmentDetailAsync(Guid enrollmentId)
            => await _enrollmentRepo.GetDetailAsync(enrollmentId);
    }
}
