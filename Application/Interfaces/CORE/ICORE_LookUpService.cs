using DTOs.CORE.LookUp.Requests;
using DTOs.CORE.LookUp.Responses;

namespace Application.Interfaces.CORE
{
    public interface ICORE_LookUpService
    {
        Task<IReadOnlyList<LookUpResponse>> GetAllAsync();
        Task<LookUpResponse?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<LookUpResponse>> GetByTypeIdAsync(Guid lookUpTypeId);
        Task<IReadOnlyList<LookUpResponse>> GetByTypeCodeAsync(string lookUpTypeCode);
        Task<LookUpResponse> CreateAsync(CreateLookUpRequest dto);
        Task<LookUpResponse> UpdateAsync(Guid id, UpdateLookUpRequest dto);
        Task<LookUpResponse> DeactivateAsync(Guid id);
        Task<LookUpResponse> ActivateAsync(Guid id);
        Task DeleteAsync(Guid id);
    }
}
