using Domain.Entities.EntityBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public interface IBaseService<TEntity, TDto, TUpdateDto, TCreateDto> where TEntity : class, IEntityBase
    {
        Task<TDto?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<TDto>> GetAllAsync();
        Task<TDto> CreateAsync(TCreateDto createDto);
        Task<TDto> UpdateAsync(Guid id, TUpdateDto dto);
        Task DeleteAsync(Guid id);
    }
}
