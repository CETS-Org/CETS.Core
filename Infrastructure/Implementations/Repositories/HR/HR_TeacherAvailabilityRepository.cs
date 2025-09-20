using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.HR;
using Infrastructure.Implementations.Repositories;

namespace Infrastructure.Implementations.Repositories.HR
{
    public class HR_TeacherAvailabilityRepository : BaseRepository<HR_TeacherAvailability>, IHR_TeacherAvailabilityRepository
    {
        public HR_TeacherAvailabilityRepository(AppDbContext context) : base(context)
        {
        }
    }
}


