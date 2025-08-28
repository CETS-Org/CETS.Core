using AutoMapper;
using Domain.Entities.EntityBase;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class BaseService<TEntity, TDto, TCreateDto> : IBaseService<TEntity, TDto, TCreateDto> where TEntity : class, IEntityBase
    {
        protected readonly IBaseRepository<TEntity> _repository;
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public BaseService(IBaseRepository<TEntity> repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public virtual async Task<TDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<TDto?>(entity);
        }

        public virtual async Task<IReadOnlyList<TDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IReadOnlyList<TDto>>(entities);
        }

        public virtual async Task<TDto> CreateAsync(TCreateDto createDto)
        {
            var entity = _mapper.Map<TEntity>(createDto);

            _repository.Add(entity);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TDto>(entity);
        }

        public virtual async Task UpdateAsync(Guid id, TDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
            {
                throw new KeyNotFoundException($"{typeof(TEntity).Name} with id {id} not found.");
            }

            _mapper.Map(dto, entity);

            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            await _repository.RemoveByIdAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}