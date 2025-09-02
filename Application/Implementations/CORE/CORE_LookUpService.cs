using Application.Interfaces.CORE;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.CORE;
using DTOs.CORE.LookUp.Requests;
using DTOs.CORE.LookUp.Responses;

namespace Application.Implementations.CORE
{
    public class CORE_LookUpService : ICORE_LookUpService
    {
        private readonly ICORE_LookUpRepository _lookUpRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CORE_LookUpService(ICORE_LookUpRepository lookUpRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _lookUpRepository = lookUpRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<LookUpResponse>> GetAllAsync()
        {
            var entities = await _lookUpRepository.GetAllAsync();
            return _mapper.Map<IReadOnlyList<LookUpResponse>>(entities);
        }

        public async Task<LookUpResponse?> GetByIdAsync(Guid id)
        {
            var entity = await _lookUpRepository.GetByIdAsync(id);
            return _mapper.Map<LookUpResponse?>(entity);
        }

        public async Task<IReadOnlyList<LookUpResponse>> GetByTypeIdAsync(Guid lookUpTypeId)
        {
            var entities = await _lookUpRepository.GetByTypeAsync(lookUpTypeId);
            return _mapper.Map<IReadOnlyList<LookUpResponse>>(entities);
        }

        public async Task<IReadOnlyList<LookUpResponse>> GetByTypeCodeAsync(string lookUpTypeCode)
        {
            var entities = await _lookUpRepository.GetByTypeAsync(lookUpTypeCode);
            return _mapper.Map<IReadOnlyList<LookUpResponse>>(entities);
        }

        public async Task<LookUpResponse> CreateAsync(CreateLookUpRequest dto)
        {
            var entity = _mapper.Map<CORE_LookUp>(dto);
            _lookUpRepository.Add(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<LookUpResponse>(entity);
        }

        public async Task<LookUpResponse> UpdateAsync(Guid id, UpdateLookUpRequest dto)
        {
            var entity = await _lookUpRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"LookUp with id {id} not found.");
            }
            _mapper.Map(dto, entity);
            _lookUpRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<LookUpResponse>(entity);
        }

        public async Task<LookUpResponse> DeactivateAsync(Guid id)
        {
            var entity =  await _lookUpRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"LookUp with id {id} not found.");
            }

            entity.IsActive = false;
            _lookUpRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<LookUpResponse>(entity);
        }

        public async Task<LookUpResponse> ActivateAsync(Guid id)
        {
            var entity = await _lookUpRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"LookUp with id {id} not found.");
            }
            entity.IsActive = true;
            _lookUpRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<LookUpResponse>(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _lookUpRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"LookUp with id {id} not found.");
            }
            _lookUpRepository.Remove(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
