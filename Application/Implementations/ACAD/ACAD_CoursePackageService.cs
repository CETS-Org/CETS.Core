using Application.Interfaces.ACAD;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_CoursePackage.Requests;
using DTOs.ACAD.ACAD_CoursePackage.Responses;
using DTOs.ACAD.ACAD_CoursePackageItem.Requests;
using DTOs.ACAD.ACAD_CoursePackageItem.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations.ACAD
{
    public class ACAD_CoursePackageService : IACAD_CoursePackageService
    {
        private readonly IACAD_CoursePackageRepository _packageRepo;
        private readonly IACAD_CoursePackageItemRepository _itemRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ACAD_CoursePackageService(
            IACAD_CoursePackageRepository packageRepo,
            IACAD_CoursePackageItemRepository itemRepo,
            IUnitOfWork uow,
            IMapper mapper)
        {
            _packageRepo = packageRepo;
            _itemRepo = itemRepo;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Guid> CreatePackageAsync(CreateCoursePackageRequest request)
        {
            return await _uow.ExecuteInTransactionAsync(async () =>
            {
                var entity = _mapper.Map<ACAD_CoursePackage>(request);
                entity.Id = Guid.NewGuid();
                entity.CreatedAt = DateTime.UtcNow;

                _packageRepo.Add(entity);
                await _uow.SaveChangesAsync();

                return entity.Id;
            });
        }

        public async Task AddCourseToPackageAsync(AddCourseToPackageRequest request)
        {
            await _uow.ExecuteInTransactionAsync(async () =>
            {
                var entity = _mapper.Map<ACAD_CoursePackageItem>(request);
                entity.Id = Guid.NewGuid();

                _itemRepo.Add(entity);
                await _uow.SaveChangesAsync();
            });
        }

        public async Task<IEnumerable<CoursePackageResponse>> GetActivePackagesAsync()
        {
            var packages = await _packageRepo.GetActivePackagesAsync();
            return _mapper.Map<IEnumerable<CoursePackageResponse>>(packages);
        }

        public async Task<CoursePackageDetailResponse?> GetPackageDetailAsync(Guid packageId)
        {
            var package = await _packageRepo.GetDetailAsync(packageId);
            return package == null
                ? null
                : _mapper.Map<CoursePackageDetailResponse>(package);
        }

        public async Task UpdatePackageAsync(UpdateCoursePackageRequest request)
        {
            await _uow.ExecuteInTransactionAsync(async () =>
            {
                var entity = await _packageRepo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new KeyNotFoundException("Course package not found");

                _mapper.Map(request, entity);
                entity.UpdatedAt = DateTime.UtcNow;

                _packageRepo.Update(entity);
                await _uow.SaveChangesAsync();
            });
        }

        public async Task SoftDeletePackageAsync(Guid packageId)
        {
            await _uow.ExecuteInTransactionAsync(async () =>
            {
                var entity = await _packageRepo.GetByIdAsync(packageId);
                if (entity == null)
                    throw new KeyNotFoundException("Course package not found");

                entity.IsDeleted = true;
                entity.UpdatedAt = DateTime.UtcNow;

                _packageRepo.Update(entity);
                await _uow.SaveChangesAsync();
            });
        }

        public async Task<IEnumerable<CoursePackageResponse>> GetAllPackagesAsync()
        {
            var packages = await _packageRepo.FindAsync(p => !p.IsDeleted);
            return _mapper.Map<IEnumerable<CoursePackageResponse>>(packages);
        }

        public async Task<CoursePackageResponse?> GetPackageByIdAsync(Guid packageId)
        {
            var package = await _packageRepo.GetByIdAsync(packageId);
            return package == null ? null : _mapper.Map<CoursePackageResponse>(package);
        }

        public async Task ActivatePackageAsync(Guid packageId)
        {
            await _uow.ExecuteInTransactionAsync(async () =>
            {
                var entity = await _packageRepo.GetByIdAsync(packageId);
                if (entity == null)
                    throw new KeyNotFoundException("Course package not found");

                entity.IsActive = true;
                entity.UpdatedAt = DateTime.UtcNow;

                _packageRepo.Update(entity);
                await _uow.SaveChangesAsync();
            });
        }

        public async Task DeactivatePackageAsync(Guid packageId)
        {
            await _uow.ExecuteInTransactionAsync(async () =>
            {
                var entity = await _packageRepo.GetByIdAsync(packageId);
                if (entity == null)
                    throw new KeyNotFoundException("Course package not found");

                entity.IsActive = false;
                entity.UpdatedAt = DateTime.UtcNow;

                _packageRepo.Update(entity);
                await _uow.SaveChangesAsync();
            });
        }

        public async Task RemoveCourseFromPackageAsync(RemoveCourseFromPackageRequest request)
        {
            await _uow.ExecuteInTransactionAsync(async () =>
            {
                var item = await _itemRepo.FindFirstAsync(i => 
                    i.PackageID == request.PackageID && 
                    i.CourseID == request.CourseID && 
                    !i.IsDeleted);

                if (item == null)
                    throw new KeyNotFoundException("Course package item not found");

                item.IsDeleted = true;
                _itemRepo.Update(item);
                await _uow.SaveChangesAsync();
            });
        }

        public async Task<IEnumerable<CoursePackageItemResponse>> GetPackageItemsAsync(Guid packageId)
        {
            var items = await _itemRepo.GetByPackageIdAsync(packageId);
            return _mapper.Map<IEnumerable<CoursePackageItemResponse>>(items);
        }

        public async Task UpdatePackageItemSequenceAsync(Guid packageItemId, int newSequence)
        {
            await _uow.ExecuteInTransactionAsync(async () =>
            {
                var item = await _itemRepo.GetByIdAsync(packageItemId);
                if (item == null)
                    throw new KeyNotFoundException("Course package item not found");

                item.Sequence = newSequence;
                _itemRepo.Update(item);
                await _uow.SaveChangesAsync();
            });
        }
    }
}
