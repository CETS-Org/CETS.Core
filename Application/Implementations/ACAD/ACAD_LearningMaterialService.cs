using Application.Interfaces.ACAD;
using Application.Interfaces.Common.Storage;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_LearningMaterial.Requests;
using DTOs.ACAD.ACAD_LearningMaterial.Responses;
using Microsoft.EntityFrameworkCore;

namespace Application.Implementations.ACAD
{
    public class ACAD_LearningMaterialService : IACAD_LearningMaterialService
    {
        private readonly IACAD_LearningMaterialRepository _learningMaterialRepo;
        private readonly IFileStorageService _fileStorageService;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ACAD_LearningMaterialService(
            IACAD_LearningMaterialRepository learningMaterialRepo,
            IFileStorageService fileStorageService,
            IUnitOfWork uow,
            IMapper mapper)
        {
            _learningMaterialRepo = learningMaterialRepo;
            _fileStorageService = fileStorageService;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<LearningMaterialUploadResponse> CreateLearningMaterialAsync(CreateLearningMaterialRequest request)
        {
            return await _uow.ExecuteInTransactionAsync(async () =>
            {
                // Generate unique file path
                var fileExtension = Path.GetExtension(request.FileName);
                var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = $"learning-materials/{DateTime.Now:yyyy/MM/dd}/{uniqueFileName}";

                // Create entity using AutoMapper
                var entity = _mapper.Map<ACAD_LearningMaterial>(request);
                entity.StoreUrl = filePath;
                entity.IsDeleted = false;

                _learningMaterialRepo.Add(entity);
                await _uow.SaveChangesAsync();

                // Get presigned upload URL
                var uploadUrl = await _fileStorageService.GetPresignedPutUrlAsync(filePath, request.ContentType);

                return new LearningMaterialUploadResponse
                {
                    Id = entity.Id,
                    UploadUrl = uploadUrl,
                    FilePath = filePath,
                    Title = entity.Title
                };
            });
        }

        public async Task<LearningMaterialUploadResponse?> UpdateLearningMaterialAsync(UpdateLearningMaterialRequest request)
        {
            return await _uow.ExecuteInTransactionAsync(async () =>
            {
                var entity = await _learningMaterialRepo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new KeyNotFoundException("Learning material not found");

                if (entity.IsDeleted)
                    throw new InvalidOperationException("Cannot update deleted learning material");

                // Check there is updating file
                var isFileUpdate = !string.IsNullOrEmpty(request.ContentType) && !string.IsNullOrEmpty(request.FileName);
                string? oldFilePath = null;
                string? uploadUrl = null;

                if (isFileUpdate)
                {
                    // Store old file path for cleanup
                    oldFilePath = entity.StoreUrl;

                    // Generate new unique file path
                    var fileExtension = Path.GetExtension(request.FileName);
                    var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                    var newFilePath = $"learning-materials/{DateTime.Now:yyyy/MM/dd}/{uniqueFileName}";

                    // Update file path
                    entity.StoreUrl = newFilePath;

                    // Get presigned upload URL for new file
                    uploadUrl = await _fileStorageService.GetPresignedPutUrlAsync(newFilePath, request.ContentType!);
                }

                _mapper.Map(request, entity);
                // entity.UpdatedAt = DateTime.Now;

                _learningMaterialRepo.Update(entity);
                await _uow.SaveChangesAsync();

                // Delete old file from storage
                if (isFileUpdate && !string.IsNullOrEmpty(oldFilePath))
                {
                    try
                    {
                        await _fileStorageService.DeleteFileAsync(oldFilePath);
                    }
                    catch (Exception ex)
                    {
                      
                        Console.WriteLine($"Warning: Failed to delete old file {oldFilePath}: {ex.Message}");
                    }
                }

              
                return new LearningMaterialUploadResponse
                    {
                        Id = entity.Id,
                        UploadUrl = uploadUrl,
                        FilePath = entity.StoreUrl!,
                        Title = entity.Title,
                    };
            });
        }

        public async Task DeleteLearningMaterialAsync(Guid id)
        {
            await _uow.ExecuteInTransactionAsync(async () =>
            {
                var entity = await _learningMaterialRepo.GetByIdAsync(id);
                if (entity == null)
                    throw new KeyNotFoundException("Learning material not found");

                if (entity.IsDeleted)
                    return; 

                entity.IsDeleted = true;

                _learningMaterialRepo.Update(entity);
                await _uow.SaveChangesAsync();

                // Delete file from storage
                //if (!string.IsNullOrEmpty(entity.StoreUrl))
                //{
                //    await _fileStorageService.DeleteFileAsync(entity.StoreUrl);
                //}
            });
        }

        public async Task<LearningMaterialResponse?> GetLearningMaterialByIdAsync(Guid id)
        {
            var entity = await _learningMaterialRepo.FindFirstAsync(lm => lm.Id == id && !lm.IsDeleted);
            if (entity == null)
                return null;

            return _mapper.Map<LearningMaterialResponse>(entity);
        }

      
        public async Task<IEnumerable<LearningMaterialResponse>> GetLearningMaterialsByClassMeetingAsync(Guid classMeetingId)
        {
            var entities = await _learningMaterialRepo.FindAsync(lm => lm.ClassMeetingID == classMeetingId && !lm.IsDeleted);
            return _mapper.Map<IEnumerable<LearningMaterialResponse>>(entities).OrderByDescending(x => x.CreatedAt);
        }

        public async Task<IEnumerable<LearningMaterialResponse>> GetLearningMaterialsByUploaderAsync(Guid uploaderId)
        {
            var entities = await _learningMaterialRepo.FindAsync(lm => lm.CreatedBy == uploaderId && !lm.IsDeleted);
            return _mapper.Map<IEnumerable<LearningMaterialResponse>>(entities).OrderByDescending(x => x.CreatedAt);
        }

        public async Task<string> GetDownloadUrlAsync(Guid id)
        {
            var entity = await _learningMaterialRepo.FindFirstAsync(lm => lm.Id == id && !lm.IsDeleted);
            if (entity == null)
                throw new KeyNotFoundException("Learning material not found");

            if (string.IsNullOrEmpty(entity.StoreUrl))
                throw new InvalidOperationException("Learning material has no associated file");

            var fileExists = await _fileStorageService.FileExistsAsync(entity.StoreUrl);
            if (!fileExists)
                throw new InvalidOperationException($"File not found in storage: {entity.StoreUrl}");

            return await _fileStorageService.GetPresignedGetUrlAsync(entity.StoreUrl);
        }

      

    }
}
