using Domain.Entities;

namespace Domain.Interfaces.CORE
{
    public interface ICORE_LookUpRepository : IBaseRepository<CORE_LookUp>
    {
        public Task<CORE_LookUp?> GetByCodeAsync(string lookUpTypeId, string code);
        public Task<CORE_LookUp?> GetByNameAsync(string name);
        public Task<IReadOnlyList<CORE_LookUp>> GetByTypeAsync(Guid lookUpTypeId);
        public Task<IReadOnlyList<CORE_LookUp>> GetByTypeAsync(string lookUpTypeName);

    }
}


