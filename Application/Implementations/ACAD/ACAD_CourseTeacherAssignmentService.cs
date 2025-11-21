using Application.Interfaces.ACAD;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_Course.Responses;
using DTOs.ACAD.ACAD_CourseTeacherAssignment.Requests;
using DTOs.ACAD.ACAD_CourseTeacherAssignment.Responses;

namespace Application.Implementations.ACAD
{
    public class ACAD_CourseTeacherAssignmentService : IACAD_CourseTeacherAssignmentService
    {
        private readonly IACAD_CourseTeacherAssignmentRepository _courseAssignmentRepository;
        private readonly IACAD_ClassMeetingRepository _classMeetingRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ACAD_CourseTeacherAssignmentService(
            IACAD_CourseTeacherAssignmentRepository courseAssignmentRepository,
            IMapper mapper,
            IACAD_ClassMeetingRepository classMeetingRepository,
            IUnitOfWork unitOfWork)
        {
            _courseAssignmentRepository = courseAssignmentRepository;
            _mapper = mapper;
            _classMeetingRepository = classMeetingRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CourseListAssignmentResponse>> GetCoursesByTeacherIdAsync(Guid teacherId)
        {
            var courses = await _courseAssignmentRepository.GetCoursesByTeacherIdAsync(teacherId);
            return _mapper.Map<IEnumerable<CourseListAssignmentResponse>>(courses);
        }
        public async Task<IEnumerable<ClassTeachingListResponse>?> GetTeachingClassesByTeacherIdAndCourseIdAsync(Guid teacherId, Guid courseId)
        {
            var courseTeacherAssignment =await _courseAssignmentRepository.GetCourseTeacherAssignmentsByTeacherIdAndCourseIdAsync(teacherId, courseId);
            var result = new List<ClassTeachingListResponse>();
            if (courseTeacherAssignment != null)
            {
                foreach (var item in courseTeacherAssignment)
                {
                    var classes = item.ACAD_Classes;
                    foreach(var classItem in classes)
                    {
                        var classMeetings = await _classMeetingRepository.GetClassMeetingTodayByClassId(classItem.Id);
                        ClassSession? classSession = null;
                        if (classMeetings != null)
                        {
                            classSession = new ClassSession
                            {
                                ClassMeetingsId = classMeetings.Id,
                                slot = classMeetings.Slot.Name,
                                RoomCode = classMeetings.Room.RoomCode,
                                TopicName = classMeetings.CoveredTopic.TopicTitle,
                                Date = classMeetings.Date,                              
                                isStudyingDay = classMeetings.IsStudy
                            };
                        }
                        
                        var classTeachingListResponseItem = new ClassTeachingListResponse
                        {                           
                            ClassId = classItem.Id,
                            Capacity = classItem.Capacity,
                            EnrolledCount = classItem.EnrolledCount,
                            IsActive = classItem.IsActive,
                            classFormatName = classItem.CourseFormat.Name,
                            StatusName = classItem.ClassStatus.Name,
                            classSession = classSession,
                            className = classItem.ClassName,
                            classNumber= classItem.ClassNum,
                            EndDate = classItem.EndDate
                        };
                        result.Add(classTeachingListResponseItem);
                    }
                    
                }
            }
            return result;
        }

        public async Task<IEnumerable<TeachingCourseResponse>> GetAllTeachingCourses(Guid teacherId)
        {
            var courseAssignments = await _courseAssignmentRepository.GetCoursesByTeacherIdAsync(teacherId);
            return _mapper.Map<IEnumerable<TeachingCourseResponse>>(courseAssignments);
        }

        public async Task<IEnumerable<CourseTeacherAssignmentResponse>> GetAssignmentsByCourseIdAsync(Guid courseId)
        {
            var assignments = await _courseAssignmentRepository.GetCourseTeacherAssignmentsByCourseIdAsync(courseId);
            return _mapper.Map<IEnumerable<CourseTeacherAssignmentResponse>>(assignments);
        }

        public async Task<CourseTeacherAssignmentResponse> CreateAssignmentAsync(CreateCourseTeacherAssignmentRequest request)
        {
            return await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var exists = await _courseAssignmentRepository.ExistsAsync(cta =>
                    cta.CourseID == request.CourseID && cta.TeacherID == request.TeacherID);

                if (exists)
                {
                    throw new InvalidOperationException("Teacher is already assigned to this course.");
                }

                var entity = new ACAD_CourseTeacherAssignment
                {
                    CourseID = request.CourseID,
                    TeacherID = request.TeacherID,
                    AssignedAt = DateTime.UtcNow
                };

                _courseAssignmentRepository.Add(entity);
                await _unitOfWork.SaveChangesAsync();

                var createdEntity = await _courseAssignmentRepository.GetAssignmentWithDetailsAsync(entity.Id)
                    ?? entity;

                return _mapper.Map<CourseTeacherAssignmentResponse>(createdEntity);
            });
        }

        public async Task DeleteAssignmentAsync(Guid assignmentId)
        {
            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var entity = await _courseAssignmentRepository.GetByIdAsync(assignmentId);
                if (entity == null)
                {
                    throw new KeyNotFoundException("Course teacher assignment not found.");
                }

                _courseAssignmentRepository.Remove(entity);
                await _unitOfWork.SaveChangesAsync();
            });
        }
    }
}