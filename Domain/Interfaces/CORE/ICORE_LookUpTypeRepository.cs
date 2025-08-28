using Domain.Entities;

namespace Domain.Interfaces.CORE
{
    public interface ICORE_LookUpTypeRepository : IBaseRepository<CORE_LookUpType>
    {
        Task<CORE_LookUpType?> GetByCodeAsync(string code);
        Task<CORE_LookUpType?> GetByNameAsync(string name);
    }
}


