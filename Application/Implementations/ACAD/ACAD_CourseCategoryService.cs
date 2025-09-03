using Application.Interfaces.ACAD;
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

        public ACAD_CourseCategoryService(
            IACAD_CourseCategoryRepository categoryRepo,
            IUnitOfWork uow)
        {
            _categoryRepo = categoryRepo;
            _uow = uow;
        }

        public async Task<Guid> CreateCourseCategoryAsync(CreateCourseCategoryRequest request)
        {
            return await _uow.ExecuteInTransactionAsync(() =>
            {
                var entity = new ACAD_CourseCategory
                {
                    Id = Guid.NewGuid(),
                    Code = request.Code,
                    Name = request.Name
                };

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

                entity.Code = request.Code;
                entity.Name = request.Name;

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
            return categories.Select(MapToResponse);
        }

        public async Task<CourseCategoryResponse?> GetCourseCategoryByIdAsync(Guid id)
        {
            var entity = await _categoryRepo.GetByIdAsync(id);
            return entity == null ? null : MapToResponse(entity);
        }

        public async Task<CourseCategoryResponse?> GetCourseCategoryByCodeAsync(string code)
        {
            var entity = await _categoryRepo.GetByCodeAsync(code);
            return entity == null ? null : MapToResponse(entity);
        }

        private static CourseCategoryResponse MapToResponse(ACAD_CourseCategory c) =>
            new CourseCategoryResponse
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name
            };
    }

}
