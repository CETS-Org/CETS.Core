using Domain.Entities;

namespace Domain.Interfaces.CORE
{
    public interface ICORE_LookUpTypeRepository : IBaseRepository<CORE_LookUpType>
    {
        public Task<CORE_LookUpType?> GetByCodeAsync(string code);
        public Task<CORE_LookUpType?> GetByNameAsync(string name);
    }
}


