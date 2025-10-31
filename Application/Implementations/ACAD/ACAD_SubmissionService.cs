using Application.Interfaces.ACAD;
using Application.Interfaces.Common.Storage;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_Submission.Requests;
using DTOs.ACAD.ACAD_Submission.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations.ACAD
{
    public class ACAD_SubmissionService : IACAD_SubmissionService
    {
        private readonly IACAD_SubmissionRepository _submissionRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ACAD_SubmissionService(
            IACAD_SubmissionRepository submissionRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IFileStorageService fileStorageService)
        {
            _submissionRepository = submissionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }

        public async Task<SubmissionResponse> SubmitAssignmentAsync(SubmitAssignmentRequest request)
        {
            return await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var existing = (await _submissionRepository.FindAsync(x =>
                    x.AssignmentID == request.AssignmentID &&
                    x.StudentID == request.StudentID &&
                    !x.IsDeleted)).FirstOrDefault();

                var fileExtension = Path.GetExtension(request.FileName);
                var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = $"submissions/{DateTime.UtcNow:yyyy/MM/dd}/{uniqueFileName}";

                //var uploadUrl = await _fileStorageService.GetPresignedPutUrlAsync(filePath, request.ContentType);
                var directory = "submissions";
                var fileName = Path.GetFileName(filePath);
                var (uploadUrl, _) = await _fileStorageService.GetPresignedPutUrlAsync(directory, fileName, request.ContentType);


                ACAD_Submission entity;

                if (existing == null)
                {
                    entity = _mapper.Map<ACAD_Submission>(request);
                    entity.Id = Guid.NewGuid();
                    entity.StoreUrl = filePath;
                    entity.CreatedAt = DateTime.UtcNow;
                    entity.UpdatedAt = entity.CreatedAt;
                    entity.IsDeleted = false;

                    _submissionRepository.Add(entity);
                }
                else
                {
                    var oldPath = existing.StoreUrl;
                    existing.StoreUrl = filePath;
                    existing.UpdatedAt = DateTime.UtcNow;

                    _submissionRepository.Update(existing);

                    if (!string.IsNullOrEmpty(oldPath))
                    {
                        try { await _fileStorageService.DeleteFileAsync(oldPath); }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Warning: Could not delete old submission file {oldPath}: {ex.Message}");
                        }
                    }

                    entity = existing;
                }

                await _unitOfWork.SaveChangesAsync();


                var response = _mapper.Map<SubmissionResponse>(entity);
                response.StoreUrl = filePath;
                response.UploadUrl = uploadUrl;
                return response;
            });
        }


        public async Task<IEnumerable<SubmissionResponse>> GetSubmissionsByAssignmentAsync(Guid assignmentId)
        {
            var submissions = await _submissionRepository.GetByAssignmentAsync(assignmentId);
            return _mapper.Map<IEnumerable<SubmissionResponse>>(submissions);
        }

        public async Task<IEnumerable<SubmissionResponse>> GetSubmissionsByStudentAsync(Guid studentId)
        {
            var submissions = await _submissionRepository.GetByStudentAsync(studentId);
            return _mapper.Map<IEnumerable<SubmissionResponse>>(submissions);
        }

        public async Task<SubmissionResponse> GradeSubmissionAsync(GradeSubmissionRequest request)
        {
            var entity = await _submissionRepository.GetByIdAsync(request.SubmissionID)
                         ?? throw new KeyNotFoundException("Submission not found");

            entity.Score = request.Score;
            entity.Feedback = request.Feedback;

            _submissionRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<SubmissionResponse>(entity);
        }
        public async Task<(int submitted, int total)> GetAssignmentsSubmittedSummaryAsync(Guid studentId, Guid courseId)
        {
            return await _submissionRepository.GetSubmissionSummaryAsync(studentId, courseId);
        }

        public async Task<SubmissionResponse> UpdateScoreAsync(UpdateSubmissionScoreRequest request)
        {
            var submission = await _submissionRepository.GetByIdAsync(request.SubmissionId)
                         ?? throw new KeyNotFoundException($"Submission with ID {request.SubmissionId} not found");

            submission.Score = request.Score;
            submission.UpdatedAt = DateTime.UtcNow;

            _submissionRepository.Update(submission);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<SubmissionResponse>(submission);
        }

        public async Task<SubmissionResponse> UpdateFeedbackAsync(UpdateSubmissionFeedbackRequest request)
        {
            var submission = await _submissionRepository.GetByIdAsync(request.SubmissionId)
                         ?? throw new KeyNotFoundException($"Submission with ID {request.SubmissionId} not found");

            submission.Feedback = request.Feedback;
            submission.UpdatedAt = DateTime.UtcNow;

            _submissionRepository.Update(submission);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<SubmissionResponse>(submission);
        }

        public async Task<string> GetDownloadUrlAsync(Guid id)
        {
            var entity = await _submissionRepository.FindFirstAsync(a => a.Id == id && !a.IsDeleted);
            if (entity == null)
                throw new KeyNotFoundException("Submission not found");

            if (string.IsNullOrEmpty(entity.StoreUrl))
                throw new InvalidOperationException("Submission has no associated file");

            var fileExists = await _fileStorageService.FileExistsAsync(entity.StoreUrl);
            if (!fileExists)
                throw new InvalidOperationException($"File not found in storage: {entity.StoreUrl}");

            return await _fileStorageService.GetPresignedGetUrlAsync(entity.StoreUrl);
        }

        public async Task<SubmissionResponse> GetSubmissionByIdAsync(Guid id)
        {
            var submission = await _submissionRepository.GetByIdAsync(id);
            if (submission == null)
                throw new KeyNotFoundException("Submission not found");
            return _mapper.Map<SubmissionResponse>(submission);
        }

        public async Task<AssignmentSubmissionsResponse> GetSubmissionsWithDownloadUrlsAsync(Guid assignmentId)
        {
            var submissions = await _submissionRepository.GetByAssignmentAsync(assignmentId);
            
            if (!submissions.Any())
                throw new KeyNotFoundException("No submissions found for this assignment");

            var firstSubmission = submissions.First();
            var assignment = firstSubmission.Assignment;
            if (assignment == null)
                throw new KeyNotFoundException("Assignment not found");

            var downloadUrls = new List<SubmissionDownloadInfo>();

            foreach (var submission in submissions)
            {
                try
                {
                    var downloadUrl = await _fileStorageService.GetPresignedGetUrlAsync(submission.StoreUrl!);
                    var fileName = Path.GetFileName(submission.StoreUrl!) ?? "submission.pdf";

                    downloadUrls.Add(new SubmissionDownloadInfo
                    {
                        SubmissionId = submission.Id,
                        StudentCode = submission.Student?.StudentCode ?? "N/A",
                        StudentName = submission.Student?.Account?.FullName ?? "N/A",
                        DownloadUrl = downloadUrl,
                        FileName = fileName
                    });
                }
                catch (Exception ex)
                {
                    // Log error but continue with other submissions
                    Console.WriteLine($"Error getting download URL for submission {submission.Id}: {ex.Message}");
                    
                    downloadUrls.Add(new SubmissionDownloadInfo
                    {
                        SubmissionId = submission.Id,
                        StudentCode = submission.Student?.StudentCode ?? "N/A",
                        StudentName = submission.Student?.Account?.FullName ?? "N/A",
                        DownloadUrl = "Error: File not available",
                        FileName = "N/A"
                    });
                }
            }

            return new AssignmentSubmissionsResponse
            {
                AssignmentInfo = new AssignmentInfo
                {
                    Id = assignment.Id,
                    Title = assignment.Title ?? "Assignment"
                },
                DownloadUrls = downloadUrls
            };
        }

        public async Task<BulkUpdateSubmissionsResponse> BulkUpdateSubmissionsAsync(BulkUpdateSubmissionsRequest request)
        {
            var response = new BulkUpdateSubmissionsResponse
            {
                Success = true,
                Data = new BulkUpdateData
                {
                    Results = new List<SubmissionUpdateResult>()
                }
            };

            // Process each submission update
            foreach (var submissionUpdate in request.Submissions)
            {
                try
                {
                    // Retrieve the submission from database
                    var submission = await _submissionRepository.GetByIdAsync(submissionUpdate.SubmissionId);

                    if (submission == null)
                    {
                        // Submission not found
                        response.Data.Results.Add(new SubmissionUpdateResult
                        {
                            SubmissionId = submissionUpdate.SubmissionId,
                            Status = "failed",
                            Error = "Submission not found"
                        });
                        response.Data.FailedCount++;
                        continue;
                    }

                    // Store previous values
                    var previousScore = submission.Score;
                    var previousFeedback = submission.Feedback;

                    // Update fields if provided (non-null)
                    if (submissionUpdate.Score.HasValue)
                    {
                        submission.Score = submissionUpdate.Score.Value;
                    }

                    if (submissionUpdate.Feedback != null)
                    {
                        submission.Feedback = submissionUpdate.Feedback;
                    }

                    // Update timestamp
                    submission.UpdatedAt = DateTime.UtcNow;

                    // Save changes to database
                    _submissionRepository.Update(submission);
                    await _unitOfWork.SaveChangesAsync();

                    // Add successful result with previous and new values
                    response.Data.Results.Add(new SubmissionUpdateResult
                    {
                        SubmissionId = submissionUpdate.SubmissionId,
                        Status = "success",
                        Updates = new UpdateDetails
                        {
                            Score = new FieldUpdate<decimal?>
                            {
                                Previous = previousScore,
                                New = submission.Score
                            },
                            Feedback = new FieldUpdate<string?>
                            {
                                Previous = previousFeedback,
                                New = submission.Feedback
                            }
                        }
                    });
                    response.Data.UpdatedCount++;
                }
                catch (Exception ex)
                {
                    // Handle any unexpected errors
                    response.Data.Results.Add(new SubmissionUpdateResult
                    {
                        SubmissionId = submissionUpdate.SubmissionId,
                        Status = "failed",
                        Error = $"Error updating submission: {ex.Message}"
                    });
                    response.Data.FailedCount++;
                }
            }

            // Set appropriate message based on results
            if (response.Data.FailedCount == 0)
            {
                response.Message = $"Successfully updated {response.Data.UpdatedCount} submissions";
            }
            else if (response.Data.UpdatedCount == 0)
            {
                response.Success = false;
                response.Message = $"Failed to update all {response.Data.FailedCount} submissions";
            }
            else
            {
                response.Message = $"Partially updated submissions: {response.Data.UpdatedCount} succeeded, {response.Data.FailedCount} failed";
            }

            return response;
        }

    }
}
