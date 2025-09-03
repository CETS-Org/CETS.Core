using Domain.Entities;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_AcademicRequestHistoryRepository : IBaseRepository<ACAD_AcademicRequestHistory>
    {
        Task<IEnumerable<ACAD_AcademicRequestHistory>> GetByRequestAsync(Guid requestId);
    }
}


