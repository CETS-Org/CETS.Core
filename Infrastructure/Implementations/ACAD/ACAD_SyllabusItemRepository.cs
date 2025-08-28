using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;

namespace Infrastructure.Repositories.ACAD
{
    public class ACAD_SyllabusItemRepository : BaseRepository<ACAD_SyllabusItem>, IACAD_SyllabusItemRepository
    {
        public ACAD_SyllabusItemRepository(AppDbContext context) : base(context)
        {
        }
    }
}


