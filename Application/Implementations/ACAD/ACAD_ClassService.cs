using Application.Interfaces.ACAD;
using Application.Interfaces.COM;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_Class.Requests;
using DTOs.ACAD.ACAD_Class.Responses;
using DTOs.COM.COM_Notification.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations.ACAD
{
    public class ACAD_ClassService : IACAD_ClassService
    {
        private readonly IACAD_ClassRepository _classRepo;
        private readonly IACAD_ClassMeetingRepository _classMeetingRepo;
        private readonly IACAD_SyllabusItemRepository _sysllabusItemRepo;
        private readonly ICOM_NotificationService _notificationService;
        private readonly IACAD_CourseTeacherAssignmentRepository _courseTeacherAssignmentService;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ACAD_ClassService(
            IACAD_ClassRepository classRepo,
            IACAD_ClassMeetingRepository classMeetingRepo,
            IACAD_SyllabusItemRepository sysllabusItemRepo,
            ICOM_NotificationService notificationService,
            IACAD_CourseTeacherAssignmentRepository courseTeacherAssignmentService,
            IUnitOfWork uow,
            IMapper mapper)
        {
            _classRepo = classRepo;
            _uow = uow;
            _mapper = mapper;
            _classMeetingRepo = classMeetingRepo;
            _sysllabusItemRepo = sysllabusItemRepo;
            _notificationService = notificationService;
            _courseTeacherAssignmentService = courseTeacherAssignmentService;
        }

        public async Task<Guid> CreateClassAsync(CreateClassRequest request)
        {
            return await _uow.ExecuteInTransactionAsync(async () =>
            {
                var entity = _mapper.Map<ACAD_Class>(request);
                entity.Id = Guid.NewGuid();
                entity.EnrolledCount = 0;
                entity.IsActive = true;
                entity.IsDeleted = false;
                entity.CreatedAt = DateTime.UtcNow;

                _classRepo.Add(entity);
                await _uow.SaveChangesAsync();

                return entity.Id;
            });
        }

        public async Task UpdateClassAsync(UpdateClassRequest request)
        {
            await _uow.ExecuteInTransactionAsync(async () =>
            {
                var entity = await _classRepo.GetByIdAsync(request.Id);
                if (entity == null) throw new Exception("Class not found");

                _mapper.Map(request, entity); 
                entity.UpdatedAt = DateTime.UtcNow;

                _classRepo.Update(entity);
                await _uow.SaveChangesAsync();
            });
        }

        public async Task DeleteClassAsync(Guid id)
        {
            await _uow.ExecuteInTransactionAsync(async () =>
            {
                await _classRepo.RemoveByIdAsync(id);
                await _uow.SaveChangesAsync();
            });
        }

        public async Task SoftDeleteClassAsync(Guid id)
        {
            await _uow.ExecuteInTransactionAsync(async () =>
            {
                var entity = await _classRepo.GetByIdAsync(id);
                if (entity == null)
                    throw new KeyNotFoundException("Class not found");

                if (entity.IsDeleted)
                    return; // idempotent

                entity.IsDeleted = true;
                entity.IsActive = false;                 
                entity.UpdatedAt = DateTime.UtcNow;
               

                _classRepo.Update(entity);
                await _uow.SaveChangesAsync();
            });
        }

        public async Task<ClassResponse?> GetClassByIdAsync(Guid id)
        {
            var entity = await _classRepo.GetByIdAsync(id);
            return _mapper.Map<ClassResponse?>(entity);
        }

        public async Task<IEnumerable<ClassResponse>> GetAllClassesAsync()
        {
            var entities = await _classRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<ClassResponse>>(entities);
        }

        public async Task<IEnumerable<ClassResponse>> GetClassesByCourseIdAsync(Guid courseId)
        {
            return await _classRepo.GetClassesByCourseIdAsync(courseId);
        }

        public async Task<List<LearningClassResponse>> GetLearningClassByStudentId(Guid studentId)
        {
            return await _classRepo.GetLearningClassByStudentId(studentId);
        }

        public async Task<ClassDetailResponse?> GetClassDetailAsync(Guid classId)
        {
            return await _classRepo.GetClassDetailAsync(classId);
        }

        public async Task<List<ClassRowResponse>> GetAllClassRowsAsync()
        {
            return await _classRepo.GetAllClassRowsAsync();
        }

        public async Task<ClassStaffViewResponse?> GetClassByIdStaffView(Guid id)
        {
            var entity = await _classRepo.GetClassStaffViewById(id);
            return _mapper.Map<ClassStaffViewResponse?>(entity);
        }

        public async Task<List<ClassStaffViewResponse>> GetAllClassStaffView()
        {
            var entities = await _classRepo.GetAllClassStaffView();
            return _mapper.Map<List<ClassStaffViewResponse>>(entities);
        }

        public async Task<List<FeedbackClassResponse>> GetFeedbackClassesByStudentId(Guid studentId)
        {
            return await _classRepo.GetFeedbackClassesByStudentId(studentId);
        }
        public async Task<Guid> CreateClassWithScheduleAsync(CreateClassWithScheduleRequest request)
        {
            return await _uow.ExecuteInTransactionAsync(async () =>
            {
                // 1. Tạo Class
                var classEntity = _mapper.Map<ACAD_Class>(request);
                classEntity.Id = Guid.NewGuid();
                classEntity.EnrolledCount = 0;
                classEntity.IsActive = true;
                classEntity.IsDeleted = false;
                classEntity.CreatedAt = DateTime.UtcNow;
                // ... gán các prop khác

                _classRepo.Add(classEntity);

                // 2. Tạo Meetings từ list Schedules gửi kèm
                if (request.Schedules != null && request.Schedules.Any())
                {
                    var meetings = new List<ACAD_ClassMeeting>();
                    foreach (var item in request.Schedules)
                    {
                        meetings.Add(new ACAD_ClassMeeting
                        {
                            Id = Guid.NewGuid(),
                            ClassID = classEntity.Id, // Link ngay với ID vừa tạo
                            SlotID = item.SlotID,
                            Date = item.Date,
                            RoomID = item.RoomID ?? Guid.Empty, // Hoặc lấy room mặc định
                            CoveredTopicID = item.SyllabusItemID,
                            TeacherAssignmentID = request.TeacherAssignmentID,
                            IsActive = true,
                            IsDeleted = false,
                            CreatedAt = DateTime.UtcNow
                        });
                    }

                    // Gọi Repo Meeting để add range (Cần inject IACAD_ClassMeetingRepository vào ClassService)
                     _classMeetingRepo.AddRange(meetings);
                }

                // 3. Commit cả 2 bảng cùng lúc
                await _uow.SaveChangesAsync();
                try
                {
                    // Bước 4.1: Lấy thông tin User ID của giáo viên để gửi
                    // Lưu ý: Notification Service cần UserId (Guid của bảng Account), không phải TeacherId
                    // Bạn cần truy vấn Teacher để lấy AccountId.

                    // Ví dụ giả định: request.TeacherAssignmentID chính là TeacherID
                    // Nếu request.TeacherAssignmentID là ID của bảng phân công, bạn cần query sâu hơn để tìm ra Teacher.
                    if (request.TeacherAssignmentID.HasValue)
                    {
                        // 2. Dùng .Value để lấy giá trị Guid thật ra (chuyển từ Guid? sang Guid)
                        var teacher = await _courseTeacherAssignmentService.GetByIdAsync(request.TeacherAssignmentID.Value);

                        if (teacher != null && teacher.Teacher != null)
                        {
                            var notificationRequest = new CreateNotificationRequest
                            {
                                UserId = teacher.TeacherID.ToString(),
                                Title = "New Class Assign",
                                Message = $"You have to assign for class: {classEntity.ClassName}.", // Sửa lại message cho gọn hoặc lấy thêm info tùy ý
                                Type = "system",
                                IsRead = false
                            };

                            await _notificationService.CreateAsync(notificationRequest);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log lỗi nhưng KHÔNG throw exception.
                    // Việc tạo lớp đã thành công, không nên rollback chỉ vì lỗi gửi thông báo.
                    // _logger.LogError(ex, "Lỗi gửi thông báo khi tạo lớp {ClassId}", classEntity.Id);
                }

                return classEntity.Id;
            });
        }


    }
}
