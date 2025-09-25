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

        public async Task UpdateLearningMaterialAsync(UpdateLearningMaterialRequest request)
        {
            await _uow.ExecuteInTransactionAsync(async () =>
            {
                var entity = await _learningMaterialRepo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new KeyNotFoundException("Learning material not found");

                if (entity.IsDeleted)
                    throw new InvalidOperationException("Cannot update deleted learning material");

                _mapper.Map(request, entity);

                _learningMaterialRepo.Update(entity);
                await _uow.SaveChangesAsync();
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
                if (!string.IsNullOrEmpty(entity.StoreUrl))
                {
                    await _fileStorageService.DeleteFileAsync(entity.StoreUrl);
                }
            });
        }

        public async Task<LearningMaterialResponse?> GetLearningMaterialByIdAsync(Guid id)
        {
            var entity = await _learningMaterialRepo.FindFirstAsync(lm => lm.Id == id && !lm.IsDeleted);
            if (entity == null)
                return null;

            return _mapper.Map<LearningMaterialResponse>(entity);
        }

        public async Task<IEnumerable<LearningMaterialResponse>> GetLearningMaterialsByClassAsync(Guid classId)
        {
            var entities = await _learningMaterialRepo.FindAsync(lm => lm.ClassID == classId && !lm.IsDeleted);
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

        public async Task<string> GetTestPresignedUrlAsync()
        {
            var testPath = $"test/connection-test-{DateTime.Now:yyyyMMdd-HHmmss}.txt";
            return await _fileStorageService.GetPresignedPutUrlAsync(testPath, "text/plain");
        }

    }
}
