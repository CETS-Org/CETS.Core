using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.RPT;

namespace Infrastructure.Repositories.RPT
{
    public class RPT_ReportRepository : BaseRepository<RPT_Report>, IRPT_ReportRepository
    {
        public RPT_ReportRepository(AppDbContext context) : base(context)
        {
        }
    }
}


