using Application.Interfaces.ACAD;
using Application.Interfaces.Common.Storage;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using Domain.Interfaces.FIN;
using DTOs.ACAD.ACAD_CoursePackage.Requests;
using DTOs.ACAD.ACAD_CoursePackage.Responses;
using DTOs.ACAD.ACAD_CoursePackage.Search;
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
        private readonly IFIN_InvoiceItemRepository _invoiceItemRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;

        public ACAD_CoursePackageService(
            IACAD_CoursePackageRepository packageRepo,
            IACAD_CoursePackageItemRepository itemRepo,
            IFIN_InvoiceItemRepository invoiceItemRepo,
            IUnitOfWork uow,
            IMapper mapper,
            IFileStorageService fileStorageService)
        {
            _packageRepo = packageRepo;
            _itemRepo = itemRepo;
            _invoiceItemRepo = invoiceItemRepo;
            _uow = uow;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }

        public async Task<Guid> CreatePackageAsync(CreateCoursePackageRequest request)
        {
            return await _uow.ExecuteInTransactionAsync(async () =>
            {
                var entity = _mapper.Map<ACAD_CoursePackage>(request);

                _packageRepo.Add(entity);
                await _uow.SaveChangesAsync();

                // Create package items if course IDs are provided
                if (request.CourseIDs != null && request.CourseIDs.Any())
                {
                    int sequence = 1;
                    foreach (var courseId in request.CourseIDs)
                    {
                        var packageItem = new ACAD_CoursePackageItem
                        {
                            PackageID = entity.Id,
                            CourseID = courseId,
                            Sequence = sequence++,
                            IsDeleted = false
                        };
                        _itemRepo.Add(packageItem);
                    }
                    await _uow.SaveChangesAsync();
                }

                return entity.Id;
            });
        }

        public async Task AddCourseToPackageAsync(Guid packageId, AddCourseToPackageRequest request)
        {
            await _uow.ExecuteInTransactionAsync(async () =>
            {
                var entity = _mapper.Map<ACAD_CoursePackageItem>(request);
                entity.PackageID = packageId;

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

                _packageRepo.Update(entity);
                await _uow.SaveChangesAsync();

                // Update package items if course IDs are provided
                if (request.CourseIDs != null)
                {
                    // Get existing package items
                    var existingItems = await _itemRepo.FindAsync(i => i.PackageID == request.Id && !i.IsDeleted);
                    var existingCourseIds = existingItems.Select(i => i.CourseID).ToList();

                    // Remove items that are no longer in the list
                    foreach (var item in existingItems)
                    {
                        if (!request.CourseIDs.Contains(item.CourseID))
                        {
                            item.IsDeleted = true;
                            _itemRepo.Update(item);
                        }
                    }

                    // Add new items
                    int sequence = 1;
                    foreach (var courseId in request.CourseIDs)
                    {
                        if (!existingCourseIds.Contains(courseId))
                        {
                            var packageItem = new ACAD_CoursePackageItem
                            {
                                PackageID = request.Id,
                                CourseID = courseId,
                                Sequence = sequence,
                                IsDeleted = false
                            };
                            _itemRepo.Add(packageItem);
                        }
                        else
                        {
                            // Update sequence for existing items
                            var existingItem = existingItems.FirstOrDefault(i => i.CourseID == courseId);
                            if (existingItem != null)
                            {
                                existingItem.Sequence = sequence;
                                _itemRepo.Update(existingItem);
                            }
                        }
                        sequence++;
                    }

                    await _uow.SaveChangesAsync();
                }
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

        public async Task<CoursePackageSearchResult> SearchBasicAsync(CoursePackageSearchQuery query, CancellationToken ct)
        {
            return await _packageRepo.SearchBasicAsync(query, ct);
        }

        public async Task<CoursePackageStatisticsResponse> GetStatisticsAsync()
        {
            // Get all non-deleted packages
            var allPackages = await _packageRepo.FindAsync(p => !p.IsDeleted);
            var packagesList = allPackages.ToList();

            // Get active packages count
            var activePackages = packagesList.Count(p => p.IsActive);

            // Get invoice items with packages to calculate actual revenue and packages sold
            var invoiceItems = await _invoiceItemRepo.FindAsync(ii => ii.CoursePackageID != null);
            var invoiceItemsList = invoiceItems.ToList();

            // Calculate total revenue from actual sales (sum of Total from invoice items)
            var totalRevenue = invoiceItemsList.Sum(ii => ii.Total);

            // Get packages sold count (sum of quantities from invoice items)
            var packagesSold = invoiceItemsList.Sum(ii => ii.Quantity);

            return new CoursePackageStatisticsResponse
            {
                TotalPackages = packagesList.Count,
                ActivePackages = activePackages,
                TotalRevenue = totalRevenue,
                PackagesSold = packagesSold
            };
        }

        public async Task<CoursePackageImageUploadResponse> GetImageUploadUrlAsync(string fileName, string contentType)
        {
            // Get presigned upload URL and generated file path
            var (uploadUrl, filePath) = await _fileStorageService.GetPresignedPutUrlAsync("packages", fileName, contentType);
            var publicUrl = _fileStorageService.GetPublicUrl(filePath);

            return new CoursePackageImageUploadResponse
            {
                UploadUrl = uploadUrl,
                FilePath = filePath,
                PublicUrl = publicUrl
            };
        }
    }
}
