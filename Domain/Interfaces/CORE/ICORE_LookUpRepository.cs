using Domain.Entities;

namespace Domain.Interfaces.CORE
{
    public interface ICORE_LookUpRepository : IBaseRepository<CORE_LookUp>
    {
        Task<CORE_LookUp?> GetByCodeAsync(string lookUpTypeCode, string code);
        Task<CORE_LookUp?> GetByNameAsync(string name);
        Task<IReadOnlyList<CORE_LookUp>> GetByTypeAsync(Guid lookUpTypeId);
        Task<IReadOnlyList<CORE_LookUp>> GetByTypeAsync(string lookUpTypeName);

    }
}


