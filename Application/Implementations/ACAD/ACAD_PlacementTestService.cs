using Application.Interfaces;
using Application.Interfaces.ACAD;
using Application.Interfaces.Common.Storage;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using Domain.Interfaces.CORE;
using Domain.Interfaces.IDN;
using DTOs.ACAD.ACAD_PlacementTest.Requests;
using DTOs.ACAD.ACAD_PlacementTest.Responses;
using System.Text;
using System.Text.Json;

namespace Application.Implementations.ACAD
{
    public class ACAD_PlacementTestService : IACAD_PlacementTestService
    {
        private readonly IACAD_PlacementTestRepository _placementTestRepository;
        private readonly IACAD_PlacementQuestionRepository _placementQuestionRepository;
        private readonly IIDN_StudentRepository _studentRepository;
        private readonly ICORE_LookUpRepository _lookUpRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public ACAD_PlacementTestService(
            IACAD_PlacementTestRepository placementTestRepository,
            IACAD_PlacementQuestionRepository placementQuestionRepository,
            IIDN_StudentRepository studentRepository,
            ICORE_LookUpRepository lookUpRepository,
            IFileStorageService fileStorageService,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _placementTestRepository = placementTestRepository;
            _placementQuestionRepository = placementQuestionRepository;
            _studentRepository = studentRepository;
            _lookUpRepository = lookUpRepository;
            _fileStorageService = fileStorageService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        #region PlacementQuestion Methods

        public async Task<PlacementQuestionResponse> CreatePlacementQuestionAsync(CreatePlacementQuestionRequest request)
        {
            return await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                string? questionUrl = null;

                // Nếu có QuestionJson, upload lên cloud storage
                if (!string.IsNullOrEmpty(request.QuestionJson))
                {
                    var jsonFileName = $"placement-question-{Guid.NewGuid()}.json";
                    var (uploadUrl, filePath) = await _fileStorageService.GetPresignedPutUrlAsync("placement-questions", jsonFileName, "application/json");
                    questionUrl = filePath;
                    // Frontend sẽ upload JSON lên uploadUrl
                }

                var entity = new ACAD_PlacementQuestion
                {
                    Id = Guid.NewGuid(),
                    Title = request.Title,
                    QuestionTypeID = request.QuestionTypeID,
                    SkillTypeID = request.SkillTypeID,
                    Difficulty = request.Difficulty,
                    QuestionUrl = questionUrl ?? request.QuestionUrl,
                    CreatedAt = DateTime.Now,
                    CreatedBy = _currentUserService.UserId ?? Guid.Empty,
                    IsDeleted = false
                };

                _placementQuestionRepository.Add(entity);
                await _unitOfWork.SaveChangesAsync();

                var response = _mapper.Map<PlacementQuestionResponse>(entity);
                return response;
            });
        }

        public async Task<IEnumerable<PlacementQuestionResponse>> CreatePlacementQuestionsAsync(List<CreatePlacementQuestionRequest> requests)
        {
            return await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var entities = new List<ACAD_PlacementQuestion>();
                var userId = _currentUserService.UserId ?? Guid.Empty;
                var now = DateTime.Now;

                foreach (var request in requests)
                {
                    string? questionUrl = null;

                    // Nếu có QuestionJson, upload lên cloud storage
                    if (!string.IsNullOrEmpty(request.QuestionJson))
                    {
                        var jsonFileName = $"placement-question-{Guid.NewGuid()}.json";
                        var (uploadUrl, filePath) = await _fileStorageService.GetPresignedPutUrlAsync("placement-questions", jsonFileName, "application/json");
                        questionUrl = filePath;
                        // Frontend sẽ upload JSON lên uploadUrl
                    }

                    var entity = new ACAD_PlacementQuestion
                    {
                        Id = Guid.NewGuid(),
                        Title = request.Title,
                        QuestionTypeID = request.QuestionTypeID,
                        SkillTypeID = request.SkillTypeID,
                        Difficulty = request.Difficulty,
                        QuestionUrl = questionUrl ?? request.QuestionUrl,
                        CreatedAt = now,
                        CreatedBy = userId,
                        IsDeleted = false
                    };

                    entities.Add(entity);
                    _placementQuestionRepository.Add(entity);
                }

                await _unitOfWork.SaveChangesAsync();

                var responses = entities.Select(e => _mapper.Map<PlacementQuestionResponse>(e)).ToList();
                return responses;
            });
        }

        public async Task<PlacementQuestionResponse> UpdatePlacementQuestionAsync(UpdatePlacementQuestionRequest request)
        {
            var entity = await _placementQuestionRepository.GetByIdAsync(request.Id)
                         ?? throw new KeyNotFoundException("Placement question not found");

            entity.Title = request.Title;
            entity.QuestionTypeID = request.QuestionTypeID;
            entity.SkillTypeID = request.SkillTypeID;
            entity.Difficulty = request.Difficulty;
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedBy = _currentUserService.UserId;

            if (!string.IsNullOrEmpty(request.QuestionJson))
            {
                var jsonFileName = $"placement-question-{entity.Id}.json";
                var (uploadUrl, filePath) = await _fileStorageService.GetPresignedPutUrlAsync("placement-questions", jsonFileName, "application/json");
                entity.QuestionUrl = filePath;
            }
            else if (!string.IsNullOrEmpty(request.QuestionUrl))
            {
                entity.QuestionUrl = request.QuestionUrl;
            }

            _placementQuestionRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PlacementQuestionResponse>(entity);
        }

        public async Task DeletePlacementQuestionAsync(Guid id)
        {
            var entity = await _placementQuestionRepository.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("Placement question not found");

            if (entity.IsDeleted)
                return;

            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedBy = _currentUserService.UserId;

            _placementQuestionRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PlacementQuestionResponse?> GetPlacementQuestionByIdAsync(Guid id)
        {
            var entity = await _placementQuestionRepository.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted)
                return null;

            return _mapper.Map<PlacementQuestionResponse>(entity);
        }

        public async Task<IEnumerable<PlacementQuestionResponse>> GetAllPlacementQuestionsAsync()
        {
            // Use FindAsync which returns IReadOnlyList, then get each with includes
            var entities = await _placementQuestionRepository.FindAsync(q => !q.IsDeleted);
            // Get entities with navigation properties included
            var entitiesWithIncludes = new List<ACAD_PlacementQuestion>();
            foreach (var entity in entities)
            {
                var entityWithIncludes = await _placementQuestionRepository.GetByIdAsync(entity.Id);
                if (entityWithIncludes != null)
                    entitiesWithIncludes.Add(entityWithIncludes);
            }
            return _mapper.Map<IEnumerable<PlacementQuestionResponse>>(entitiesWithIncludes);
        }

        public async Task<IEnumerable<PlacementQuestionResponse>> GetPlacementQuestionsByCriteriaAsync(Guid questionTypeId, int difficulty, Guid? skillTypeId = null)
        {
            var entities = await _placementQuestionRepository.GetQuestionsByCriteriaAsync(questionTypeId, difficulty, skillTypeId);
            return _mapper.Map<IEnumerable<PlacementQuestionResponse>>(entities);
        }


        #endregion

        #region Helper Methods

        /*private async Task<string> UploadTestJsonAsync(string testJson, string fileName)
        {
            // Upload JSON directly using UploadFileContentAsync
            var filePath = await _fileStorageService.UploadFileContentAsync("placement-tests", fileName, testJson, "application/json");
            return filePath;
        }*/
        public async Task<string> UploadTestJsonAsync(string json, string fileName)
        {
            var (uploadUrl, filePath) = await _fileStorageService.GetPresignedPutUrlAsync("placement-tests", fileName, "application/json");

            var bytes = Encoding.UTF8.GetBytes(json);

            // Tính Content-MD5 (nhiều S3 buckets yêu cầu khi PUT)
            using var md5 = System.Security.Cryptography.MD5.Create();
            var md5Hash = md5.ComputeHash(bytes);
            string md5Base64 = Convert.ToBase64String(md5Hash);

            using var client = new HttpClient(new HttpClientHandler
            {
                AllowAutoRedirect = false,
                UseCookies = false
            });

            var request = new HttpRequestMessage(HttpMethod.Put, uploadUrl)
            {
                Content = new ByteArrayContent(bytes)
            };

            // BẮT BUỘC – tránh chunked upload
            request.Headers.TransferEncodingChunked = false;

            request.Content.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            request.Content.Headers.ContentLength = bytes.Length;

            // MD5 quan trọng khi bucket bật kiểm tra hash
            request.Content.Headers.ContentMD5 = md5Hash;

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var text = await response.Content.ReadAsStringAsync();
                throw new Exception($"Upload failed {response.StatusCode}: {text}");
            }

            return filePath;
        }


        private async Task<List<object>> GetQuestionsDataAsync(List<ACAD_PlacementQuestion> questions)
        {
            var questionsData = new List<object>();

            foreach (var question in questions)
            {
                if (string.IsNullOrEmpty(question.QuestionUrl))
                    continue;

                try
                {
                    // Download question JSON directly using R2FileStorageService
                    var questionJson = await _fileStorageService.DownloadFileContentAsync(question.QuestionUrl);
                    var questionData = JsonSerializer.Deserialize<object>(questionJson);
                    
                    if (questionData != null)
                    {
                        questionsData.Add(new
                        {
                            questionId = question.Id,
                            title = question.Title,
                            questionType = question.QuestionType != null ? question.QuestionType.Name : "",
                            difficulty = question.Difficulty,
                            data = questionData
                        });
                    }
                }
                catch (Exception ex)
                {
                    // Log error but continue with other questions
                    Console.WriteLine($"Error loading question {question.Id}: {ex.Message}");
                }
            }

            return questionsData;
        }

        #endregion

        #region PlacementTest Methods

        public async Task<PlacementTestResponse> RandomPlacementTestAsync()
        {
            // Random theo tiêu chí:
            // - 2 đoạn văn ngắn (passage, difficulty = 2)
            // - 1 bài văn dài (passage, difficulty = 3)
            // - 2 audio ngắn (audio, difficulty = 2)
            // - 1 audio dài (audio, difficulty = 3)
            // - 5 câu hỏi multiple choice grammar (MCQ, difficulty = 1)
            // 
            // Note: Method này chỉ random questions và return về cho frontend preview/select,
            // KHÔNG tạo PlacementTest entity trong database. Staff sẽ cần click "Create Test" 
            // để thực sự tạo placement test trong DB.

            // Lookup QuestionTypeID từ LookUp table
            var passageLookUp = await _lookUpRepository.GetByCodeAsync("QuestionType", "passage");
            var audioLookUp = await _lookUpRepository.GetByCodeAsync("QuestionType", "audio");
            var mcqLookUp = await _lookUpRepository.GetByCodeAsync("QuestionType", "MCQ");

            if (passageLookUp == null || audioLookUp == null || mcqLookUp == null)
                throw new InvalidOperationException("Required QuestionType lookups not found in database");

            var selectedQuestions = new List<ACAD_PlacementQuestion>();

            // 2 passage ngắn
            var shortPassages = await _placementQuestionRepository.GetRandomQuestionsByCriteriaAsync(passageLookUp.Id, 2, 2);
            selectedQuestions.AddRange(shortPassages);

            // 1 passage dài
            var longPassage = await _placementQuestionRepository.GetRandomQuestionsByCriteriaAsync(passageLookUp.Id, 3, 1);
            selectedQuestions.AddRange(longPassage);

            // 2 audio ngắn
            var shortAudios = await _placementQuestionRepository.GetRandomQuestionsByCriteriaAsync(audioLookUp.Id, 2, 2);
            selectedQuestions.AddRange(shortAudios);

            // 1 audio dài
            var longAudio = await _placementQuestionRepository.GetRandomQuestionsByCriteriaAsync(audioLookUp.Id, 3, 1);
            selectedQuestions.AddRange(longAudio);

            // 5 MCQ grammar
            var mcqQuestions = await _placementQuestionRepository.GetRandomQuestionsByCriteriaAsync(mcqLookUp.Id, 1, 5);
            selectedQuestions.AddRange(mcqQuestions);

            // Map questions to response (không cần download JSON hay tạo test entity)
            var questionResponses = _mapper.Map<List<PlacementQuestionResponse>>(selectedQuestions);

            // Return PlacementTestResponse với questions (không có id vì chưa tạo trong DB)
            return new PlacementTestResponse
            {
                Id = Guid.Empty, // Chưa tạo trong DB
                Title = $"Random Placement Test - {DateTime.Now:yyyy-MM-dd HH:mm}",
                DurationMinutes = 60,
                StoreUrl = null,
                Questions = questionResponses,
                CreatedAt = DateTime.Now,
                UpdatedAt = null,
                IsDeleted = false
            };
        }

        public async Task<PlacementTestResponse> CreatePlacementTestWithQuestionsAsync(CreatePlacementTestWithQuestionsRequest request)
        {
            return await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                // Verify all questions exist
                var questions = new List<ACAD_PlacementQuestion>();
                foreach (var questionId in request.QuestionIds)
                {
                    var question = await _placementQuestionRepository.GetByIdAsync(questionId);
                    if (question == null || question.IsDeleted)
                        throw new KeyNotFoundException($"Placement question {questionId} not found");
                    questions.Add(question);
                }

                // Download và tổng hợp JSON của các câu hỏi
                var questionsData = await GetQuestionsDataAsync(questions);

                // Tạo test JSON
                var testJson = JsonSerializer.Serialize(new
                {
                    title = request.Title,
                    durationMinutes = request.DurationMinutes,
                    questions = questionsData,
                    createdAt = DateTime.Now
                }, new JsonSerializerOptions { WriteIndented = true });

                // Upload test JSON lên cloud
                var fileName = $"placement-test-{Guid.NewGuid()}.json";
                var storeUrl = await UploadTestJsonAsync(testJson, fileName);

                // Create PlacementTest
                var test = new ACAD_PlacementTest
                {
                    Id = Guid.NewGuid(),
                    Title = request.Title,
                    DurationMinutes = request.DurationMinutes,
                    StoreUrl = storeUrl,
                    CreatedAt = DateTime.Now,
                    CreatedBy = _currentUserService.UserId ?? Guid.Empty,
                    IsDeleted = false
                };

                _placementTestRepository.Add(test);
                await _unitOfWork.SaveChangesAsync();

                return await GetPlacementTestByIdAsync(test.Id) ?? throw new InvalidOperationException("Failed to create placement test");
            });
        }

        public async Task<PlacementTestResponse> UpdatePlacementTestAsync(Guid id, UpdatePlacementTestRequest request)
        {
            return await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var test = await _placementTestRepository.GetByIdAsync(id)
                          ?? throw new KeyNotFoundException("Placement test not found");

                test.Title = request.Title;
                test.DurationMinutes = request.DurationMinutes;
                test.UpdatedAt = DateTime.Now;
                test.UpdatedBy = _currentUserService.UserId;
                test.IsDeleted = request.IsDeleted;

                // Update questions if provided
                if (request.QuestionIds != null && request.QuestionIds.Any())
                {
                    // Get questions
                    var questions = new List<ACAD_PlacementQuestion>();
                    foreach (var questionId in request.QuestionIds)
                    {
                        var question = await _placementQuestionRepository.GetByIdAsync(questionId);
                        if (question == null || question.IsDeleted)
                            continue;
                        questions.Add(question);
                    }

                    // Download và tổng hợp JSON của các câu hỏi
                    var questionsData = await GetQuestionsDataAsync(questions);

                    // Tạo test JSON mới
                    var testJson = JsonSerializer.Serialize(new
                    {
                        title = request.Title,
                        durationMinutes = request.DurationMinutes,
                        questions = questionsData,
                        updatedAt = DateTime.Now
                    }, new JsonSerializerOptions { WriteIndented = true });

                    // Upload test JSON mới lên cloud (có thể xóa file cũ nếu cần)
                    var fileName = $"placement-test-{id}.json";
                    var storeUrl = await UploadTestJsonAsync(testJson, fileName);
                    test.StoreUrl = storeUrl;
                }

                _placementTestRepository.Update(test);
                await _unitOfWork.SaveChangesAsync();

                return await GetPlacementTestByIdAsync(id) ?? throw new InvalidOperationException("Failed to update placement test");
            });
        }

        public async Task<PlacementTestResponse?> GetPlacementTestByIdAsync(Guid id)
        {
            var test = await _placementTestRepository.GetByIdAsync(id);
            if (test == null || test.IsDeleted)
                return null;

            // Download và parse JSON từ cloud
            List<PlacementQuestionResponse> questions = new List<PlacementQuestionResponse>();
            
            if (!string.IsNullOrEmpty(test.StoreUrl))
            {
                try
                {
                    // Download JSON directly using R2FileStorageService
                    var testJson = await _fileStorageService.DownloadFileContentAsync(test.StoreUrl);
                    var testData = JsonSerializer.Deserialize<JsonElement>(testJson);

                    if (testData.TryGetProperty("questions", out var questionsElement))
                    {
                        foreach (var questionElement in questionsElement.EnumerateArray())
                        {
                            if (questionElement.TryGetProperty("questionId", out var questionIdElement))
                            {
                                var questionId = Guid.Parse(questionIdElement.GetString() ?? "");
                                var question = await _placementQuestionRepository.GetByIdAsync(questionId);
                                if (question != null && !question.IsDeleted)
                                {
                                    questions.Add(_mapper.Map<PlacementQuestionResponse>(question));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log error but return test without questions
                    Console.WriteLine($"Error loading test questions: {ex.Message}");
                }
            }

            var response = _mapper.Map<PlacementTestResponse>(test);
            response.Questions = questions;
            return response;
        }

        public async Task<IEnumerable<PlacementTestResponse>> GetAllPlacementTestsAsync()
        {
            // Get all tests including deleted ones for staff management
            var tests = await _placementTestRepository.GetAllPlacementTestsForStaffAsync();
            var responses = new List<PlacementTestResponse>();

            foreach (var test in tests)
            {
                // Load questions similar to GetPlacementTestByIdAsync but don't check IsDeleted
                List<PlacementQuestionResponse> questions = new List<PlacementQuestionResponse>();
                
                if (!string.IsNullOrEmpty(test.StoreUrl))
                {
                    try
                    {
                        var testJson = await _fileStorageService.DownloadFileContentAsync(test.StoreUrl);
                        var testData = JsonSerializer.Deserialize<JsonElement>(testJson);

                        if (testData.TryGetProperty("questions", out var questionsElement))
                        {
                            foreach (var questionElement in questionsElement.EnumerateArray())
                            {
                                if (questionElement.TryGetProperty("questionId", out var questionIdElement))
                                {
                                    var questionId = Guid.Parse(questionIdElement.GetString() ?? "");
                                    var question = await _placementQuestionRepository.GetByIdAsync(questionId);
                                    if (question != null && !question.IsDeleted)
                                    {
                                        questions.Add(_mapper.Map<PlacementQuestionResponse>(question));
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log error but continue with other tests
                        Console.WriteLine($"Error loading test questions for {test.Id}: {ex.Message}");
                    }
                }

                var response = _mapper.Map<PlacementTestResponse>(test);
                response.Questions = questions;
                responses.Add(response);
            }

            return responses;
        }

        public async Task DeletePlacementTestAsync(Guid id)
        {
            var entity = await _placementTestRepository.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("Placement test not found");

            if (entity.IsDeleted)
                return;

            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedBy = _currentUserService.UserId;

            _placementTestRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PlacementTestResponse> TogglePlacementTestStatusAsync(Guid id, bool isDisabled)
        {
            var entity = await _placementTestRepository.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("Placement test not found");

            // isDisabled = true means disable (set IsDeleted = true)
            // isDisabled = false means enable (set IsDeleted = false)
            entity.IsDeleted = isDisabled;
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedBy = _currentUserService.UserId;

            _placementTestRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            // Map entity to response (load questions similar to GetPlacementTestByIdAsync but don't check IsDeleted)
            List<PlacementQuestionResponse> questions = new List<PlacementQuestionResponse>();
            
            if (!string.IsNullOrEmpty(entity.StoreUrl))
            {
                try
                {
                    var testJson = await _fileStorageService.DownloadFileContentAsync(entity.StoreUrl);
                    var testData = JsonSerializer.Deserialize<JsonElement>(testJson);

                    if (testData.TryGetProperty("questions", out var questionsElement))
                    {
                        foreach (var questionElement in questionsElement.EnumerateArray())
                        {
                            if (questionElement.TryGetProperty("questionId", out var questionIdElement))
                            {
                                var questionId = Guid.Parse(questionIdElement.GetString() ?? "");
                                var question = await _placementQuestionRepository.GetByIdAsync(questionId);
                                if (question != null && !question.IsDeleted)
                                {
                                    questions.Add(_mapper.Map<PlacementQuestionResponse>(question));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log error but return test without questions
                    Console.WriteLine($"Error loading test questions: {ex.Message}");
                }
            }

            var response = _mapper.Map<PlacementTestResponse>(entity);
            response.Questions = questions;
            return response;
        }

        public async Task<PlacementTestResponse> GetRandomPlacementTestForStudentAsync()
        {
            // Random 1 đề từ các đề đã tạo sẵn
            var allTests = await _placementTestRepository.GetAllActivePlacementTestsAsync();
            if (!allTests.Any())
                throw new InvalidOperationException("No placement tests available");

            var random = new Random();
            var randomTest = allTests.OrderBy(x => random.Next()).First();

            return await GetPlacementTestByIdAsync(randomTest.Id) 
                   ?? throw new InvalidOperationException("Failed to get placement test");
        }

        public async Task SubmitPlacementTestAsync(SubmitPlacementTestRequest request)
        {
           /* await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {*/
                // Validate placement test exists
                var placementTest = await _placementTestRepository.GetByIdAsync(request.PlacementTestId);
                if (placementTest == null || placementTest.IsDeleted)
                    throw new KeyNotFoundException("Placement test not found");

                // Validate student exists
                var student = await _studentRepository.GetStudentWithAccountAsync(request.StudentId);
                if (student == null || student.IsDeleted)
                    throw new KeyNotFoundException("Student not found");

                // Validate score range (placement test now uses 0-900 scale)
                if (request.Score < 0 || request.Score > 900)
                    throw new ArgumentException("Score must be between 0 and 900");

                // Round score to 2 decimal places to match Precision(5,2) in database
                var roundedScore = Math.Round(request.Score, 2);

                // Update student's PlacementTestGrade sau khi học sinh hoàn thành bài test
                student.PlacementTestGrade = roundedScore;
                student.UpdatedAt = DateTime.Now;
                student.UpdatedBy = _currentUserService.UserId;
                _studentRepository.Update(student);

                await _unitOfWork.SaveChangesAsync();
           // });
        }

        public async Task<string> GetQuestionDataUrlAsync(Guid id)
        {
            var entity = await _placementTestRepository.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted)
                throw new KeyNotFoundException("Placement test not found");

            if (string.IsNullOrEmpty(entity.StoreUrl))
                throw new InvalidOperationException("Placement test has no question data");

            return await _fileStorageService.GetPresignedGetUrlAsync(entity.StoreUrl);
        }

        public async Task<IEnumerable<QuestionTypeResponse>> GetQuestionTypesAsync()
        {
            // Lấy tất cả QuestionType từ LookUp table với LookUpType = "QuestionType"
            var questionTypes = await _lookUpRepository.GetByTypeAsync("QuestionType");
            
            return questionTypes
                .Where(lt => lt.IsActive)
                .Select(lt => new QuestionTypeResponse
                {
                    Id = lt.Id,
                    Code = lt.Code,
                    Name = lt.Name,
                    IsActive = lt.IsActive
                })
                .OrderBy(lt => lt.Name)
                .ToList();
        }

        #endregion
    }
}
