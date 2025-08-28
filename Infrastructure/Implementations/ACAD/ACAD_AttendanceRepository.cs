using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;

namespace Infrastructure.Repositories.ACAD
{
    public class ACAD_AttendanceRepository : BaseRepository<ACAD_Attendance>, IACAD_AttendanceRepository
    {
        public ACAD_AttendanceRepository(AppDbContext context) : base(context)
        {
        }
    }
}


