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
                // Get presigned upload URL and generated file path
                var (uploadUrl, filePath) = await _fileStorageService.GetPresignedPutUrlAsync("assignments", request.FileName, request.ContentType);

                // Create entity using AutoMapper
                var entity = _mapper.Map<ACAD_Assignment>(request);
                entity.StoreUrl = filePath;

                _assignmentRepository.Add(entity);
                await _unitOfWork.SaveChangesAsync();

                // Map entity to response and set computed properties
                var response = _mapper.Map<AssignmentUploadResponse>(entity);
                response.UploadUrl = uploadUrl;
                response.FilePath = filePath;

                return response;
            });
        }

        public async Task<QuizAssignmentResponse> CreateQuizAssignmentAsync(CreateQuizAssignmentRequest request)
        {
            return await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                // Get presigned URL for JSON file upload
                var jsonFileName = $"quiz-assignment-{Guid.NewGuid()}.json";
                var (uploadUrl, jsonFilePath) = await _fileStorageService.GetPresignedPutUrlAsync("assignments/questions", jsonFileName, "application/json");

                // Create entity using AutoMapper
                var entity = _mapper.Map<ACAD_Assignment>(request);
                entity.QuestionUrl = jsonFilePath; // Store file path

                _assignmentRepository.Add(entity);
                await _unitOfWork.SaveChangesAsync();

                var response = _mapper.Map<QuizAssignmentResponse>(entity);
                response.UploadUrl = uploadUrl; // Return presigned URL for frontend to upload JSON
                response.QuestionJson = request.QuestionJson; // Return JSON content for frontend to upload
                response.QuestionFilePath = jsonFilePath; // Return file path for updates

                return response;
            });
        }

        public async Task<SpeakingAssignmentResponse> CreateSpeakingAssignmentAsync(CreateSpeakingAssignmentRequest request)
        {
            return await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                // Get presigned URL for JSON file upload
                var jsonFileName = $"speaking-assignment-{Guid.NewGuid()}.json";
                var (jsonUploadUrl, jsonFilePath) = await _fileStorageService.GetPresignedPutUrlAsync("assignments/questions", jsonFileName, "application/json");

                // Create entity using AutoMapper
                var entity = _mapper.Map<ACAD_Assignment>(request);
                entity.QuestionUrl = jsonFilePath; // Store JSON file path

                _assignmentRepository.Add(entity);
                await _unitOfWork.SaveChangesAsync();

                var response = _mapper.Map<SpeakingAssignmentResponse>(entity);
                response.UploadUrl = jsonUploadUrl; // Return presigned URL for JSON upload
                response.QuestionJson = request.QuestionJson; // Return JSON content for frontend to upload
                response.QuestionJsonUrl = null; // Will be set when fetching

                return response;
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
            if (assignment == null)
                return null;

            var response = _mapper.Map<AssignmentResponse>(assignment);
            response.QuestionUrl = assignment.QuestionUrl;

            return response;
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
            var entity = await _assignmentRepository.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("Assignment not found");

            if (entity.IsDeleted)
                return; // Already deleted,

            entity.IsDeleted = true;
            
            _assignmentRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<AssignmentResponse>> GetAssignmentsWithSubmissions(Guid classMeetingId, Guid studentId)
        {
            var assignments = await _assignmentRepository.GetAssignmentsWithSubmissions(classMeetingId, studentId);
            var responses = _mapper.Map<IEnumerable<AssignmentResponse>>(assignments).ToList();
            
           
            foreach (var assignment in assignments)
            {
                var response = responses.FirstOrDefault(r => r.Id == assignment.Id);
                if (response != null)
                {
                    response.QuestionUrl = assignment.QuestionUrl;
                }
            }
            
            return responses;
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
        


        public async Task<string> GetQuestionDataUrlAsync(Guid id)
        {
            var entity = await _assignmentRepository.FindFirstAsync(a => a.Id == id && !a.IsDeleted);
            if (entity == null)
                throw new KeyNotFoundException("Assignment not found");

            if (string.IsNullOrEmpty(entity.QuestionUrl))
                throw new InvalidOperationException("Assignment has no question data");

            // Check if QuestionUrl is a file path (cloud storage)
            if (entity.QuestionUrl.StartsWith("assignments/"))
            {
                // Get presigned URL for frontend to download JSON
                return await _fileStorageService.GetPresignedGetUrlAsync(entity.QuestionUrl);
            }
            else
            {
                // Already a URL, return as is
                return entity.QuestionUrl;
            }
        }

    }
}