using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.IDN;
using Infrastructure.Implementations.Repositories;

namespace Infrastructure.Implementations.Repositories.IDN
{
    public class IDN_TeacherCredentialRepository : BaseRepository<IDN_TeacherCredential>, IIDN_TeacherCredentialRepository
    {
        public IDN_TeacherCredentialRepository(AppDbContext context) : base(context)
        {
        }
    }
}


