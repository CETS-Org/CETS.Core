using Application.Interfaces.ACAD;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_CoursePackage.Requests;
using DTOs.ACAD.ACAD_CoursePackage.Responses;
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
    }
}
