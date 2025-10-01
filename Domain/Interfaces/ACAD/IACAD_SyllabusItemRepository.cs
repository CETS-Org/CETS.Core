using Domain.Entities;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_SyllabusItemRepository : IBaseRepository<ACAD_SyllabusItem>
    {
        Task<IEnumerable<ACAD_SyllabusItem>> GetBySyllabusIdAsync(Guid syllabusId);

    }
}


