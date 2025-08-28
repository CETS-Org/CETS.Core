using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.IDN;

namespace Infrastructure.Repositories.IDN
{
    public class IDN_TeacherCredentialRepository : BaseRepository<IDN_TeacherCredential>, IIDN_TeacherCredentialRepository
    {
        public IDN_TeacherCredentialRepository(AppDbContext context) : base(context)
        {
        }
    }
}


