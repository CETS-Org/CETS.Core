using Application.Interfaces.ACAD;
using Application.Interfaces.Common.Storage;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using Domain.Interfaces.CORE;
using DTOs.ACAD.ACAD_Submission.Requests;
using DTOs.ACAD.ACAD_Submission.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Implementations.ACAD
{
    public class ACAD_SubmissionService : IACAD_SubmissionService
    {
        private readonly IACAD_SubmissionRepository _submissionRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICORE_LookUpRepository _lookUpRepository;
        private readonly ICORE_LookUpTypeRepository _lookUpTypeRepository;
        private readonly IACAD_AssignmentRepository _assignmentRepository;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public ACAD_SubmissionService(
            IACAD_SubmissionRepository submissionRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IFileStorageService fileStorageService,
            ICORE_LookUpRepository lookUpRepository,
            IACAD_AssignmentRepository assignmentRepository,
            ICORE_LookUpTypeRepository lookUpTypeRepository,
            IConfiguration configuration)
        {
            _submissionRepository = submissionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
            _lookUpRepository = lookUpRepository;
            _assignmentRepository = assignmentRepository;
            _lookUpTypeRepository = lookUpTypeRepository;
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public async Task<SubmissionResponse> SubmitAssignmentAsync(SubmitAssignmentRequest request)
        {
            return await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var existing = (await _submissionRepository.FindAsync(x =>
                    x.AssignmentID == request.AssignmentID &&
                    x.StudentID == request.StudentID &&
                    !x.IsDeleted)).FirstOrDefault();
                var uploadUrl = "";
                var filePath = "";

                if (request.FileName != null)
                {
                    var fileExtension = Path.GetExtension(request.FileName);
                    var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

                    var directory = "submissions";
                    var fileName = uniqueFileName;

                    (uploadUrl, filePath) = await _fileStorageService.GetPresignedPutUrlAsync(directory, fileName, request.ContentType);

                }

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
            entity.IsAiScore = false; // Teacher graded

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
            submission.IsAiScore = false; // Teacher updated
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
            submission.IsAiScore = false; // Teacher updated
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
                    var previousIsAiScore = submission.IsAiScore;

                    // Update fields if provided (non-null)
                    if (submissionUpdate.Score.HasValue)
                    {
                        submission.Score = submissionUpdate.Score.Value;
                    }

                    if (submissionUpdate.Feedback != null)
                    {
                        submission.Feedback = submissionUpdate.Feedback;
                    }

                    // Mark as teacher graded when score or feedback is updated
                    if (submissionUpdate.Score.HasValue || submissionUpdate.Feedback != null)
                    {
                        submission.IsAiScore = false; // Teacher updated
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
                            },
                            IsAiScore = new FieldUpdate<bool>
                            {
                                Previous = previousIsAiScore,
                                New = submission.IsAiScore
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

        public async Task<IEnumerable<SubmissionResponse>> GetSubmissionsByAssignmentAndSkillAsync(Guid assignmentId, string? assignmentSkill)
        {
            // Lấy assignment để kiểm tra SkillID
            var assignment = await _assignmentRepository.GetByIdAsync(assignmentId);
            if (assignment == null || assignment.IsDeleted)
            {
                return Enumerable.Empty<SubmissionResponse>();
            }

            // Nếu có assignmentSkill, kiểm tra assignment.SkillID có khớp không
            if (!string.IsNullOrWhiteSpace(assignmentSkill))
            {
                // Get skill lookup by code (reading, writing, speaking, listening)
                var skill = await _lookUpRepository.GetByCodeAsync(LookUpTypes.CourseSkill, assignmentSkill);
                if (skill == null)
                {
                    // Skill code không tồn tại, trả về empty list
                    return Enumerable.Empty<SubmissionResponse>();
                }

                // Kiểm tra assignment.SkillID có khớp với skillId không
                if (assignment.SkillID != skill.Id)
                {
                    // Assignment không có skill này, trả về empty list
                    return Enumerable.Empty<SubmissionResponse>();
                }
            }

            // Lấy tất cả submissions của assignment
            var submissions = await _submissionRepository.GetByAssignmentAsync(assignmentId);
            return _mapper.Map<IEnumerable<SubmissionResponse>>(submissions);
        }

        //Gemini Score and feeback
        public async Task<(double Score, string Feedback)> GradeEssayByAiAsync(IFormFile file)
        {
            var ApiKey = _configuration["GeminiApi:ApiKey"];
            string uploadUrl = $"https://generativelanguage.googleapis.com/upload/v1beta/files?key={ApiKey}";
            string metadata = JsonSerializer.Serialize(new
            {
                file = new
                {
                    displayName = file.FileName
                }
            });

            var boundary = "boundary_" + Guid.NewGuid().ToString("N");
            var content = new MultipartContent("related", boundary);

            var metadataPart = new StringContent(metadata, Encoding.UTF8, "application/json");
            content.Add(metadataPart);

            var fileStream = file.OpenReadStream();
            var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent);

            var uploadRequest = new HttpRequestMessage(HttpMethod.Post, uploadUrl)
            {
                Content = content
            };
            uploadRequest.Headers.ExpectContinue = false;

            var uploadResponse = await _httpClient.SendAsync(uploadRequest);
            var uploadBody = await uploadResponse.Content.ReadAsStringAsync();

            if (!uploadResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Gemini upload failed: {uploadResponse.StatusCode} - {uploadBody}");
            }

            var uploadJson = JsonDocument.Parse(uploadBody);
            var fileUri = uploadJson.RootElement.GetProperty("file").GetProperty("uri").GetString();
            var mimeType = uploadJson.RootElement.GetProperty("file").GetProperty("mimeType").GetString();



            string modelUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={ApiKey}";

            var prompt = @"You are an IELTS Writing examiner.
                    Read the uploaded essay file and provide:
                    1. A band score (0–9)
                    2. Short feedback (3–5 sentences)
                    Output JSON: { ""score"": number, ""feedback"": string }";

            var requestJson = new
            {
                contents = new[]
                {
                new {
                    role = "user",
                    parts = new object[]
                    {
                        new { text = prompt },
                        new { fileData = new { mimeType = mimeType, fileUri = fileUri } }
                    }
                }
            }
            };

            var jsonBody = JsonSerializer.Serialize(requestJson);
            var request = new HttpRequestMessage(HttpMethod.Post, modelUrl)
            {
                Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Gemini grading failed: {response.StatusCode}");
            }



            try
            {
                var doc = JsonDocument.Parse(responseBody);
                var textResponse = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                var resultJson = JsonDocument.Parse(textResponse);
                double score = resultJson.RootElement.GetProperty("score").GetDouble();
                string feedback = resultJson.RootElement.GetProperty("feedback").GetString();

                return (score, feedback);
            }
            catch (Exception ex)
            {
                return (0, "Could not parse Gemini response");
            }
        }

        // New method: Grade essay by text (instead of file upload)
        public async Task<(double Score, string Feedback)> GradeEssayByTextAsync(string essayText)
        {
            var ApiKey = _configuration["GeminiApi:ApiKey"];
            string modelUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";

            var prompt = $@"You are an IELTS Writing examiner.
                        Read the following essay and provide:
                        1. A band score (0–9)
                        2. Short feedback (3–5 sentences)
                        Output ONLY valid JSON in this exact format: {{""score"": number, ""feedback"": string}}

                        Essay:
                        {essayText}";

            var requestJson = new
            {
                contents = new[]
                {
                    new {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var jsonBody = JsonSerializer.Serialize(requestJson);
            var request = new HttpRequestMessage(HttpMethod.Post, modelUrl)
            {
                Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
            };
            
            // Add API key as header instead of query parameter
            request.Headers.Add("X-goog-api-key", ApiKey);

            var response = await _httpClient.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Gemini grading failed: {response.StatusCode} - {responseBody}");
            }

            try
            {
                var doc = JsonDocument.Parse(responseBody);
                var textResponse = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                // Clean up the response (remove markdown code blocks if present)
                textResponse = textResponse?.Trim();
                if (textResponse?.StartsWith("```json") == true)
                {
                    textResponse = textResponse.Substring(7);
                }
                if (textResponse?.StartsWith("```") == true)
                {
                    textResponse = textResponse.Substring(3);
                }
                if (textResponse?.EndsWith("```") == true)
                {
                    textResponse = textResponse.Substring(0, textResponse.Length - 3);
                }
                textResponse = textResponse?.Trim();

                var resultJson = JsonDocument.Parse(textResponse);
                double score = resultJson.RootElement.GetProperty("score").GetDouble();
                string feedback = resultJson.RootElement.GetProperty("feedback").GetString() ?? "";

                return (score, feedback);
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not parse Gemini response: {ex.Message}. Response: {responseBody}");
            }
        }

        public async Task<SubmissionResponse> SubmitWritingAssignmentAsync(SubmitWritingSubmissionRequest request)
        {
            return await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                // Validate file
                if (request.File == null)
                {
                    throw new ArgumentException("File is required");
                }

                // Extract text from document
                string essayText = await Application.Helpers.DocumentTextExtractor.ExtractTextFromFileAsync(request.File);

                if (string.IsNullOrWhiteSpace(essayText))
                {
                    throw new ArgumentException("Could not extract text from the document or document is empty");
                }

                // Grade essay by AI using extracted text
                var (score, feedback) = await GradeEssayByTextAsync(essayText);

                // Check if submission already exists
                var existing = (await _submissionRepository.FindAsync(x =>
                    x.AssignmentID == request.AssignmentId &&
                    x.StudentID == request.StudentId &&
                    !x.IsDeleted)).FirstOrDefault();

                // Get presigned upload URL and generated file path (similar to assignment)
                var (uploadUrl, filePath) = await _fileStorageService.GetPresignedPutUrlAsync(
                    "submissions",
                    request.FileName,
                    request.ContentType
                );

                ACAD_Submission entity;

                if (existing == null)
                {
                    // Create new submission
                    entity = new ACAD_Submission
                    {
                        Id = Guid.NewGuid(),
                        AssignmentID = request.AssignmentId,
                        StudentID = request.StudentId,
                        StoreUrl = filePath,
                        Score = (decimal)score,
                        Feedback = feedback,
                        IsAiScore = true, // AI graded
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    };

                    _submissionRepository.Add(entity);
                }
                else
                {
                    // Update existing submission
                    var oldPath = existing.StoreUrl;
                    existing.StoreUrl = filePath;
                    existing.Score = (decimal)score;
                    existing.Feedback = feedback;
                    existing.IsAiScore = true; // AI graded
                    existing.UpdatedAt = DateTime.UtcNow;

                    _submissionRepository.Update(existing);

                    // Delete old file if exists
                    if (!string.IsNullOrEmpty(oldPath))
                    {
                        try
                        {
                            await _fileStorageService.DeleteFileAsync(oldPath);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Warning: Could not delete old submission file {oldPath}: {ex.Message}");
                        }
                    }

                    entity = existing;
                }

                await _unitOfWork.SaveChangesAsync();

                // Map to response (similar to assignment)
                var response = _mapper.Map<SubmissionResponse>(entity);
                response.StoreUrl = filePath;
                response.UploadUrl = uploadUrl;
                response.Score = (decimal)score;
                response.Feedback = feedback;

                return response;
            });
        }
    }
}
