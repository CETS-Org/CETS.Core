using Domain.Entities;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_AcademicRequestRepository : IBaseRepository<ACAD_AcademicRequest>
    {
        Task<IEnumerable<ACAD_AcademicRequest>> GetByStudentAsync(Guid studentId);
        Task<IEnumerable<ACAD_AcademicRequest>> GetByStatusAsync(Guid statusId);
        Task<IEnumerable<ACAD_AcademicRequest>> GetAllAsync();
        Task<ACAD_AcademicRequest?> GetDetailsAsync(Guid requestId);
    }
}


