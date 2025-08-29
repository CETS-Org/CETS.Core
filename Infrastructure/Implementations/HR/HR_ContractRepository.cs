using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.HR;

namespace Infrastructure.Repositories.HR
{
    public class HR_ContractRepository : BaseRepository<HR_Contract>, IHR_ContractRepository
    {
        public HR_ContractRepository(AppDbContext context) : base(context)
        {
        }
    }
}


