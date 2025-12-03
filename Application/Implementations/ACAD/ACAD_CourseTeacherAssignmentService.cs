using Application.Interfaces.ACAD;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_Course.Responses;
using DTOs.ACAD.ACAD_CourseTeacherAssignment.Requests;
using DTOs.ACAD.ACAD_CourseTeacherAssignment.Responses;
using DTOs.IDN.IDN_Teacher.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<TeacherResponse>> GetTeachersByCourseAsync(Guid courseId)
        {
            var teachers = await _courseAssignmentRepository.GetTeachersByCourseAsync(courseId);
            return _mapper.Map<IEnumerable<TeacherResponse>>(teachers);
        }

        public async Task<IEnumerable<CourseTeacherAssignmentResponse>> GetTeacherAssignmentByCourseAsync(Guid courseId)
        {
            var teacherAssignments = await _courseAssignmentRepository.GetTeacherAssignmentByCourseAsync(courseId);
            return _mapper.Map<IEnumerable<CourseTeacherAssignmentResponse>>(teacherAssignments);
        }

        public async Task<IEnumerable<TeacherOptionResponse>> GetAvailableTeachersAsync(GetAvailableTeachersRequest request)
        {
            // 1. Lấy danh sách giáo viên có chuyên môn dạy khóa học này
            var rawTeacherList = await GetTeacherAssignmentByCourseAsync(request.CourseId);

            // 2. (Optional) BƯỚC QUAN TRỌNG: Lọc giáo viên rảnh (Available)
            // Tại đây bạn nên lọc bỏ những giáo viên đã có lịch dạy trùng với 
            // thời gian trong 'request' (nếu request có gửi lên StartTime/EndTime/Schedule).
            // Ví dụ: rawTeacherList = rawTeacherList.Where(t => !IsTeacherBusy(t.Id, request)).ToList();

            // 3. Map sang TeacherOptionResponse để trả về cho Client
            var result = rawTeacherList.Select(t => new TeacherOptionResponse
            {
                Id = t.Id,
                FullName = t.FullName,
                Email = t.Email,
                Phone = t.Phone,
                AvatarUrl = t.AvatarUrl,            
                YearsExperience = t.YearsExperience,      
                CanTeachOnline = true, 
                CanTeachOffline = true
            }).ToList();

            return result;
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

        public async Task<IReadOnlyList<TeacherOptionDto>> GetAvailableTeachersForClassAsync(GetAvailableTeachersRequest request)
        {
            if (request.EndDate < request.StartDate)
                throw new ArgumentException("EndDate must be greater than or equal to StartDate.");

            // 1. Lấy tất cả teacher assignment của course
            var assignments = await _courseAssignmentRepository.GetByCourseAsync(request.CourseId);
            if (!assignments.Any())
                return Array.Empty<TeacherOptionDto>();

            // 2. Không có schedule => coi như teacher nào cũng rảnh trong course này
            if (request.Schedules == null || request.Schedules.Count == 0)
            {
                return assignments
                    .Select(a => new TeacherOptionDto
                    {
                        Id = a.Id, // TeacherAssignmentID
                        FullName = a.Teacher?.Account?.FullName ?? "Unknown",
                        Email = a.Teacher?.Account?.Email
                    })
                    .OrderBy(t => t.FullName)
                    .ToList();
            }

            // 3. Map DayOfWeek -> list TimeSlotID
            var scheduleLookup = request.Schedules
                .GroupBy(s => s.DayOfWeek)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.TimeSlotID).Distinct().ToList()
                );

            // 4. Generate (Date, TimeSlotID) buổi học của lớp mới
            var requestedPairs = new List<(DateOnly Date, Guid TimeSlotID)>();
            var cursor = request.StartDate;

            while (cursor <= request.EndDate)
            {
                var dow = cursor.DayOfWeek;
                if (scheduleLookup.TryGetValue(dow, out var slotList))
                {
                    foreach (var slotId in slotList)
                    {
                        requestedPairs.Add((cursor, slotId));
                    }
                }

                cursor = cursor.AddDays(1);
            }

            if (!requestedPairs.Any())
            {
                // Không generate được buổi nào -> coi như không trùng lịch
                return assignments
                    .Select(a => new TeacherOptionDto
                    {
                        Id = a.Id,
                        FullName = a.Teacher?.Account?.FullName ?? "Unknown",
                        Email = a.Teacher?.Account?.Email
                    })
                    .OrderBy(t => t.FullName)
                    .ToList();
            }

            // 5. SlotIDs cần quan tâm
            var requestedSlotIds = request.Schedules
                .Select(s => s.TimeSlotID)
                .Distinct()
                .ToList();

            // 6. Lấy các buổi học của teacher trong khoảng để check trùng
            var teacherAssignmentIds = assignments.Select(a => a.Id).ToList();
            var candidateMeetings = await _classMeetingRepository.GetMeetingsForTeacherOverlapAsync(
                request.StartDate,
                request.EndDate,
                requestedSlotIds,
                teacherAssignmentIds);

            // 7. HashSet "yyyy-MM-dd|slotGuid" cho lịch lớp mới
            var requestedKeySet = requestedPairs
                .Select(p => $"{p.Date:yyyy-MM-dd}|{p.TimeSlotID}")
                .ToHashSet();

            // 8. TeacherAssignment nào có (Date, SlotID) trùng => bị bận
            var busyAssignmentIds = candidateMeetings
                .Where(m => m.TeacherAssignmentID.HasValue &&
                            requestedKeySet.Contains($"{m.Date:yyyy-MM-dd}|{m.SlotID}"))
                .Select(m => m.TeacherAssignmentID!.Value)
                .Distinct()
                .ToHashSet();

            // 9. Các assignment còn lại là available
            var availableAssignments = assignments
                .Where(a => !busyAssignmentIds.Contains(a.Id))
                .ToList();

            return availableAssignments
                .Select(a => new TeacherOptionDto
                {
                    Id = a.Id,
                    FullName = a.Teacher?.Account?.FullName ?? "Unknown",
                    Email = a.Teacher?.Account?.Email
                })
                .OrderBy(t => t.FullName)
                .ToList();
        }
    }
}