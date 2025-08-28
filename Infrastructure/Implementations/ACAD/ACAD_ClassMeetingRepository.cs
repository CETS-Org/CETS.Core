using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;

namespace Infrastructure.Repositories.ACAD
{
    public class ACAD_ClassMeetingRepository : BaseRepository<ACAD_ClassMeeting>, IACAD_ClassMeetingRepository
    {
        public ACAD_ClassMeetingRepository(AppDbContext context) : base(context)
        {
        }
    }
}


