using DTOs.ACAD.ACAD_PlacementTest.Requests;
using DTOs.ACAD.ACAD_PlacementTest.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.ACAD
{
    public interface IACAD_PlacementTestService
    {
        // PlacementQuestion methods
        Task<PlacementQuestionResponse> CreatePlacementQuestionAsync(CreatePlacementQuestionRequest request);
        Task<IEnumerable<PlacementQuestionResponse>> CreatePlacementQuestionsAsync(List<CreatePlacementQuestionRequest> requests);
        Task<PlacementQuestionResponse> UpdatePlacementQuestionAsync(UpdatePlacementQuestionRequest request);
        Task DeletePlacementQuestionAsync(Guid id);
        Task<PlacementQuestionResponse?> GetPlacementQuestionByIdAsync(Guid id);
        Task<IEnumerable<PlacementQuestionResponse>> GetAllPlacementQuestionsAsync();
        Task<IEnumerable<PlacementQuestionResponse>> GetPlacementQuestionsByCriteriaAsync(Guid questionTypeId, int difficulty, Guid? skillTypeId = null);
        
        // PlacementTest methods
        Task<PlacementTestResponse> RandomPlacementTestAsync(); // Random đề theo tiêu chí (Staff)
        Task<PlacementTestResponse> CreatePlacementTestWithQuestionsAsync(CreatePlacementTestWithQuestionsRequest request); // Tạo đề với danh sách câu hỏi được chọn
        Task<PlacementTestResponse> UpdatePlacementTestAsync(Guid id, UpdatePlacementTestRequest request);
        Task<PlacementTestResponse?> GetPlacementTestByIdAsync(Guid id);
        Task<IEnumerable<PlacementTestResponse>> GetAllPlacementTestsAsync();
        Task DeletePlacementTestAsync(Guid id);
        Task<PlacementTestResponse> TogglePlacementTestStatusAsync(Guid id, bool isDisabled); // true = disable (IsDeleted = true), false = enable (IsDeleted = false)
        Task SubmitPlacementTestAsync(SubmitPlacementTestRequest request);
        Task<PlacementTestResponse> GetRandomPlacementTestForStudentAsync(); // Học sinh random 1 đề để làm
        Task<string> GetQuestionDataUrlAsync(Guid id);
        
        // Lookup methods
        Task<IEnumerable<QuestionTypeResponse>> GetQuestionTypesAsync(); // Lấy danh sách QuestionType từ LookUp
    }
}
