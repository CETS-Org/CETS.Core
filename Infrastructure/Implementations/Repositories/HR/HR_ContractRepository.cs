using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.HR;
using Infrastructure.Implementations.Repositories;

namespace Infrastructure.Implementations.Repositories.HR
{
    public class HR_ContractRepository : BaseRepository<HR_Contract>, IHR_ContractRepository
    {
        public HR_ContractRepository(AppDbContext context) : base(context)
        {
        }
    }
}


