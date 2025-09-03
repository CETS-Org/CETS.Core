using Application.Interfaces.CORE;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.CORE;
using DTOs.CORE.LookUpType.Requests;
using DTOs.CORE.LookUpType.Responses;

namespace Application.Implementations.CORE
{
    public class CORE_LookUpTypeService : 
        BaseService<CORE_LookUpType, LookUpTypeResponse, UpdateLookUpTypeRequest, CreateLookUpTypeRequest>, 
        ICORE_LookUpTypeService
    {

        private readonly ICORE_LookUpTypeRepository _lookUpTypeRepository;

        public CORE_LookUpTypeService(ICORE_LookUpTypeRepository lookUpTypeRepository, IUnitOfWork unitOfWork, IMapper mapper)
         : base(lookUpTypeRepository, unitOfWork, mapper)
        {
            _lookUpTypeRepository = lookUpTypeRepository;
        }


        public async Task<LookUpTypeResponse?> GetByCodeAsync(string code)
        {
            var entity = await _lookUpTypeRepository.GetByCodeAsync(code);
            return _mapper.Map<LookUpTypeResponse?>(entity);
        }

        public async Task<LookUpTypeResponse?> GetByNameAsync(string name)
        {
            var entity = await _lookUpTypeRepository.GetByNameAsync(name);
            return _mapper.Map<LookUpTypeResponse?>(entity);
        }
    }
}
