using Domain.Entities;
using DTOs.CORE.LookUpType.Requests;
using DTOs.CORE.LookUpType.Responses;

namespace Application.Interfaces.CORE
{
    public interface ICORE_LookUpTypeService 
        : IBaseService<CORE_LookUpType, LookUpTypeResponse, UpdateLookUpTypeRequest, CreateLookUpTypeRequest>
    {
        Task<LookUpTypeResponse?> GetByCodeAsync(string code);
        Task<LookUpTypeResponse?> GetByNameAsync(string name);
    }
}
