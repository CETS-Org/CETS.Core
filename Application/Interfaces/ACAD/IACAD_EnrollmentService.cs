using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ACAD
{
    public interface IACAD_EnrollmentService
    {
        Task<ACAD_Enrollment> EnrollAsync(Guid studentId, Guid courseId, Guid? classId);
        Task ApproveEnrollmentAsync(Guid enrollmentId, Guid staffId);
        Task RejectEnrollmentAsync(Guid enrollmentId, Guid staffId, string reason);
        Task<IEnumerable<ACAD_Enrollment>> GetStudentEnrollmentsAsync(Guid studentId);
        Task<IEnumerable<ACAD_Enrollment>> GetClassEnrollmentsAsync(Guid classId);
        Task<ACAD_Enrollment?> GetEnrollmentDetailAsync(Guid enrollmentId);
    }
}
