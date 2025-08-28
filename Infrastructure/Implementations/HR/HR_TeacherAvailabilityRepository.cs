using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.HR;

namespace Infrastructure.Repositories.HR
{
    public class HR_TeacherAvailabilityRepository : BaseRepository<HR_TeacherAvailability>, IHR_TeacherAvailabilityRepository
    {
        public HR_TeacherAvailabilityRepository(AppDbContext context) : base(context)
        {
        }
    }
}


