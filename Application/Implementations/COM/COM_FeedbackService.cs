using Application.Interfaces.COM;
using Application.Interfaces.CORE;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.COM;
using DTOs.COM.COM_Feedback.Requests;
using DTOs.COM.COM_Feedback.Responses;

namespace Application.Implementations.COM
{
	public class COM_FeedbackService : BaseService<COM_Feedback, FeedbackResponse, UpdateFeedbackRequest, CreateFeedbackRequest>, ICOM_FeedbackService
	{
		private readonly ICORE_LookUpService _lookUpService;
		private readonly ICOM_FeedbackRepository _feedbackRepository;

		public COM_FeedbackService(
			ICOM_FeedbackRepository repository, 
			IUnitOfWork unitOfWork, 
			IMapper mapper,
			ICORE_LookUpService lookUpService)
			: base(repository, unitOfWork, mapper)
		{
			_lookUpService = lookUpService;
			_feedbackRepository = repository;
		}

		public async Task<FeedbackResponse> SoftDeleteAsync(Guid id)
		{
			var entity = await _repository.GetByIdAsync(id);
			if (entity == null)
			{
				throw new KeyNotFoundException($"COM_Feedback with id {id} not found.");
			}
			entity.IsDeleted = true;
			_repository.Update(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<FeedbackResponse>(entity);
        }

		public async Task<FeedbackResponse> RestoreAsync(Guid id)
		{
			var entity = await _repository.GetByIdAsync(id);
			if (entity == null)
			{
				throw new KeyNotFoundException($"COM_Feedback with id {id} not found.");
			}
			entity.IsDeleted = false;
			_repository.Update(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<FeedbackResponse>(entity);
		}

		public async Task<CombinedFeedbackResponse> CreateCombinedFeedbackAsync(CreateCombinedFeedbackRequest request)
		{
			try
			{
				// Get feedback type IDs from lookup
				var feedbackType = await _lookUpService.GetByTypeCodeAsync(LookUpTypes.FeedbackType);
                var courseFeedbackType = feedbackType.FirstOrDefault(ft => ft.Code == "ForCourse");
                var teacherFeedbackType = feedbackType.FirstOrDefault(ft => ft.Code == "ForTeacher");

                if (courseFeedbackType == null || teacherFeedbackType == null)
				{
					throw new InvalidOperationException("Feedback types not found in lookup table. Please ensure COURSE_FEEDBACK and TEACHER_FEEDBACK exist.");
				}

				FeedbackResponse? courseFeedbackResponse = null;
				FeedbackResponse? teacherFeedbackResponse = null;

				// Create Course Feedback if data provided
				if (request.CourseFeedback != null)
				{
					var courseFeedback = new COM_Feedback
					{
						Id = Guid.NewGuid(),
						SubmitterID = request.SubmitterID,
						FeedbackTypeID = courseFeedbackType.LookUpId,
						CourseID = request.CourseID,
						Rating = request.CourseFeedback.Rating,
						Comment = request.CourseFeedback.Comment,
						ContentClarity = request.CourseFeedback.ContentClarity,
						CourseRelevance = request.CourseFeedback.CourseRelevance,
						MaterialsQuality = request.CourseFeedback.MaterialsQuality,
						CreatedAt = DateTime.UtcNow,
						IsDeleted = false
					};

					_repository.Add(courseFeedback);
					courseFeedbackResponse = _mapper.Map<FeedbackResponse>(courseFeedback);
				}

				// Create Teacher Feedback if data provided
				if (request.TeacherFeedback != null)
				{
					var teacherFeedback = new COM_Feedback
					{
						Id = Guid.NewGuid(),
						SubmitterID = request.SubmitterID,
						FeedbackTypeID = teacherFeedbackType.LookUpId,
						TeacherID = request.TeacherID,
						CourseID = request.CourseID,
						Rating = request.TeacherFeedback.Rating,
						Comment = request.TeacherFeedback.Comment,
						TeachingEffectiveness = request.TeacherFeedback.TeachingEffectiveness,
						CommunicationSkills = request.TeacherFeedback.CommunicationSkills,
						TeacherSupportiveness = request.TeacherFeedback.TeacherSupportiveness,
						CreatedAt = DateTime.UtcNow,
						IsDeleted = false
					};

					_repository.Add(teacherFeedback);
					teacherFeedbackResponse = _mapper.Map<FeedbackResponse>(teacherFeedback);
				}

				// Save all changes
				await _unitOfWork.SaveChangesAsync();

				return new CombinedFeedbackResponse
				{
					CourseFeedback = courseFeedbackResponse,
					TeacherFeedback = teacherFeedbackResponse,
					Success = true,
					Message = "Feedback submitted successfully"
				};
			}
			catch (Exception ex)
			{
				return new CombinedFeedbackResponse
				{
					Success = false,
					Message = $"Failed to submit feedback: {ex.Message}"
				};
			}
		}

		public async Task<List<CourseFeedbackListResponse>> GetFeedbacksByCourseIdAsync(Guid courseId)
		{
			return await _feedbackRepository.GetFeedbacksByCourseIdAsync(courseId);
		}
    }
}



