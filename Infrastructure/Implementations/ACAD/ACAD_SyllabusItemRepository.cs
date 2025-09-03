using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.ACAD
{
    public class ACAD_SyllabusItemRepository : BaseRepository<ACAD_SyllabusItem>, IACAD_SyllabusItemRepository
    {
        public ACAD_SyllabusItemRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<ACAD_SyllabusItem>> GetBySyllabusIdAsync(Guid syllabusId)
        {
            return await _context.ACAD_SyllabusItems
                .Where(i => i.SyllabusID == syllabusId)
                .ToListAsync();
        }
    }
}


