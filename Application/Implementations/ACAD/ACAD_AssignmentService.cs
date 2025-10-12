using Application.Interfaces.ACAD;
using Application.Interfaces.Common.Storage;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_Assignment.Requests;
using DTOs.ACAD.ACAD_Assignment.Responses;
using DTOs.ACAD.ACAD_Submission.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations.ACAD
{
    public class AssignmentService : IACAD_AssignmentService
    {
        private readonly IACAD_AssignmentRepository _assignmentRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AssignmentService(
            IACAD_AssignmentRepository assignmentRepository,
            IFileStorageService fileStorageService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _assignmentRepository = assignmentRepository;
            _fileStorageService = fileStorageService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AssignmentResponse> CreateAssignmentAsync(CreateAssignmentRequest request)
        {
            var entity = _mapper.Map<ACAD_Assignment>(request);

            _assignmentRepository.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AssignmentResponse>(entity);
        }

        public async Task<AssignmentUploadResponse> CreateAssignmentWithFileAsync(CreateAssignmentWithFileRequest request)
        {
            return await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                // Generate unique file path
                var fileExtension = Path.GetExtension(request.FileName);
                var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = $"assignments/{DateTime.Now:yyyy/MM/dd}/{uniqueFileName}";

                // Create entity using AutoMapper
                var entity = _mapper.Map<ACAD_Assignment>(request);
                entity.StoreUrl = filePath;
                entity.IsDeleted = false;
                entity.CreatedAt = DateTime.Now;
                entity.CreatedBy = request.TeacherId;


                _assignmentRepository.Add(entity);
                await _unitOfWork.SaveChangesAsync();

                // Get presigned upload URL
                var uploadUrl = await _fileStorageService.GetPresignedPutUrlAsync(filePath, request.ContentType);

                return new AssignmentUploadResponse
                {
                    Id = entity.Id,
                    UploadUrl = uploadUrl,
                    FilePath = filePath,
                    Title = entity.Title!,
                    DueDate = entity.DueAt ?? DateTime.Now,
                };
            });
        }

        public async Task<IEnumerable<AssignmentResponse>> GetAssignmentsByClassMeetingAsync(Guid classMeetingId)
        {
            var assignments = await _assignmentRepository.GetByClassMeetingAsync(classMeetingId);
            return _mapper.Map<IEnumerable<AssignmentResponse>>(assignments);
        }

        public async Task<IEnumerable<AssignmentResponse>> GetAssignmentsByTeacherAsync(Guid teacherId)
        {
            var assignments = await _assignmentRepository.GetByTeacherAsync(teacherId);
            return _mapper.Map<IEnumerable<AssignmentResponse>>(assignments);
        }

        public async Task<AssignmentResponse?> GetAssignmentByIdAsync(Guid id)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(id);
            return _mapper.Map<AssignmentResponse?>(assignment);
        }

        public async Task<AssignmentResponse> UpdateAssignmentAsync(UpdateAssignmentRequest request)
        {
            var entity = await _assignmentRepository.GetByIdAsync(request.Id)
                         ?? throw new KeyNotFoundException("Assignment not found");

            _mapper.Map(request, entity);
            _assignmentRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AssignmentResponse>(entity);
        }

        public async Task DeleteAssignmentAsync(Guid id)
        {
            await _assignmentRepository.RemoveByIdAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<AssignmentResponse>> GetAssignmentsWithSubmissions(Guid classMeetingId, Guid studentId)
        {
            var assignments = await _assignmentRepository.GetAssignmentsWithSubmissions(classMeetingId, studentId);
            return _mapper.Map<IEnumerable<AssignmentResponse>>(assignments);
        }

        public async Task<IEnumerable<AssignmentWithSubmissionCountResponse>> GetAssignmentsWithSubmissionCountAsync(Guid classMeetingId)
        {
            return await _assignmentRepository.GetAssignmentsWithSubmissionCountAsync(classMeetingId);
        }

        public async Task<string> GetDownloadUrlAsync(Guid id)
        {
            var entity = await _assignmentRepository.FindFirstAsync(a => a.Id == id && !a.IsDeleted);
            if (entity == null)
                throw new KeyNotFoundException("Assignment not found");

            if (string.IsNullOrEmpty(entity.StoreUrl))
                throw new InvalidOperationException("Assignment has no associated file");

            var fileExists = await _fileStorageService.FileExistsAsync(entity.StoreUrl);
            if (!fileExists)
                throw new InvalidOperationException($"File not found in storage: {entity.StoreUrl}");

            return await _fileStorageService.GetPresignedGetUrlAsync(entity.StoreUrl);
        }

    }
}
