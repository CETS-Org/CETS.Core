using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.ACAD
{
    public class ACAD_AcademicRequestHistoryRepository : BaseRepository<ACAD_AcademicRequestHistory>, IACAD_AcademicRequestHistoryRepository
    {
        public ACAD_AcademicRequestHistoryRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<ACAD_AcademicRequestHistory>> GetByRequestAsync(Guid requestId)
        {
            return await _context.ACAD_AcademicRequestHistories
                .Where(h => h.RequestID == requestId)
                .OrderByDescending(h => h.UpdatedAt)
                .ToListAsync();
        }
    }
}


