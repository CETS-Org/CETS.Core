using Application.Interfaces.ACAD;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_CourseCategory.Request;
using DTOs.ACAD.ACAD_CourseCategory.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Implementations.ACAD
{
    public class ACAD_CourseCategoryService : IACAD_CourseCategoryService
    {
        private readonly IACAD_CourseCategoryRepository _categoryRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ACAD_CourseCategoryService(
            IACAD_CourseCategoryRepository categoryRepo,
            IUnitOfWork uow,
            IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Guid> CreateCourseCategoryAsync(CreateCourseCategoryRequest request)
        {
            return await _uow.ExecuteInTransactionAsync(() =>
            {
                var entity = _mapper.Map<ACAD_CourseCategory>(request);
                entity.Id = Guid.NewGuid();

                _categoryRepo.Add(entity);
                return Task.FromResult(entity.Id);
            });
        }

        public async Task UpdateCourseCategoryAsync(UpdateCourseCategoryRequest request)
        {
            await _uow.ExecuteInTransactionAsync(async () =>
            {
                var entity = await _categoryRepo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new KeyNotFoundException($"Course category with Id {request.Id} not found.");

                _mapper.Map(request, entity);
                _categoryRepo.Update(entity);
            });
        }

        public async Task DeleteCourseCategoryAsync(Guid id)
        {
            await _uow.ExecuteInTransactionAsync(() =>
                _categoryRepo.RemoveByIdAsync(id)
            );
        }

        public async Task<IEnumerable<CourseCategoryResponse>> GetAllCourseCategoriesAsync()
        {
            var categories = await _categoryRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<CourseCategoryResponse>>(categories);
        }

        public async Task<CourseCategoryResponse?> GetCourseCategoryByIdAsync(Guid id)
        {
            var entity = await _categoryRepo.GetByIdAsync(id);
            return _mapper.Map<CourseCategoryResponse?>(entity);
        }

        public async Task<CourseCategoryResponse?> GetCourseCategoryByCodeAsync(string code)
        {
            var entity = await _categoryRepo.GetByCodeAsync(code);
            return _mapper.Map<CourseCategoryResponse?>(entity);
        }
    }
}
