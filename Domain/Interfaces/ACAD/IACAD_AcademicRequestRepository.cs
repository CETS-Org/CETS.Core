using Domain.Entities;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_AcademicRequestRepository : IBaseRepository<ACAD_AcademicRequest>
    {
        Task<IEnumerable<ACAD_AcademicRequest>> GetByStudentAsync(Guid studentId);
        Task<IEnumerable<ACAD_AcademicRequest>> GetByStatusAsync(Guid statusId);
        Task<ACAD_AcademicRequest?> GetDetailsAsync(Guid requestId);
    }
}


