using Application.Interfaces.ACAD;
using Application.Interfaces.COM;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using Domain.Interfaces.FIN;
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
        private readonly IFIN_InvoiceItemRepository _invoiceItemRepository;
        private readonly IACAD_EnrollmentRepository _enrollmentRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ACAD_ClassService(
            IACAD_ClassRepository classRepo,
            IACAD_ClassMeetingRepository classMeetingRepo,
            IACAD_SyllabusItemRepository sysllabusItemRepo,
            ICOM_NotificationService notificationService,
            IACAD_CourseTeacherAssignmentRepository courseTeacherAssignmentService,
            IACAD_EnrollmentRepository enrollmentRepo,
            IFIN_InvoiceItemRepository invoiceItemRepository,
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
            _enrollmentRepo = enrollmentRepo;
            _invoiceItemRepository = invoiceItemRepository;
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
            // [CONSTANTS] ID của trạng thái "Enrolled / Đã xếp lớp"
            // Tốt nhất nên đưa vào file Constant chung (VD: EnrollmentStatus.Enrolled)
            var STATUS_ENROLLED = Guid.Parse("148fdc3d-fecc-457d-a539-cc28fd5df900");

            return await _uow.ExecuteInTransactionAsync(async () =>
            {
                // ==================================================================
                // 1. TẠO CLASS (LỚP HỌC)
                // ==================================================================
                var classEntity = _mapper.Map<ACAD_Class>(request);            
                // Cập nhật sĩ số hiện tại dựa trên danh sách học sinh gửi lên
                classEntity.EnrolledCount = request.Enrollments?.Count ?? 0;
                classEntity.IsActive = true;
                classEntity.IsDeleted = false;
                classEntity.CreatedAt = DateTime.UtcNow;        
                _classRepo.Add(classEntity);

                // ==================================================================
                // 2. TẠO MEETINGS (LỊCH HỌC)
                // ==================================================================
                if (request.Schedules != null && request.Schedules.Any())
                {
                    var meetings = request.Schedules.Select(item => new ACAD_ClassMeeting
                    {                      
                        ClassID = classEntity.Id, // Link với Class vừa tạo
                        SlotID = item.SlotID,
                        Date = item.Date,
                        RoomID = item.RoomID ?? Guid.Empty,
                        CoveredTopicID = item.SyllabusItemID,
                        TeacherAssignmentID = request.TeacherAssignmentID,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedAt = DateTime.UtcNow
                    }).ToList();

                    _classMeetingRepo.AddRange(meetings);
                }

                // ==================================================================
                // 3. CẬP NHẬT ENROLLMENT (XẾP HỌC SINH VÀO LỚP)
                // ==================================================================
                if (request.Enrollments != null && request.Enrollments.Any())
                {
                    foreach (var studentItem in request.Enrollments)
                    {
                        // Tìm Enrollment cũ (đang ở trạng thái Waiting) bằng EnrollmentId
                        var enrollment = await _enrollmentRepo.GetByIdAsync(studentItem.EnrollmentId);

                        if (enrollment != null)
                        {
                            enrollment.ClassID = classEntity.Id;          // Gán vào lớp mới
                            enrollment.EnrollmentStatusID = STATUS_ENROLLED; // Đổi trạng thái
                            enrollment.UpdatedAt = DateTime.UtcNow;
                            enrollment.UpdatedBy = request.CreatedBy;

                            _enrollmentRepo.Update(enrollment);
                            var invoiceItems = await _invoiceItemRepository.GetByInvoiceIdAsync(enrollment.InvoiceID.Value); 
                            var invoiceItem = invoiceItems.Where(x => x.PaymentSequence == 2).FirstOrDefault();
                            if (invoiceItem != null)
                            {
                                invoiceItem.DueDate = classEntity.StartDate.AddDays(30); 
                                _invoiceItemRepository.Update(invoiceItem);
                            }
                        }                      
                    }
                }

                // ==================================================================
                // 4. COMMIT DATABASE (Lưu tất cả thay đổi vào DB)
                // ==================================================================
                await _uow.SaveChangesAsync();

                // ---> Dữ liệu đã an toàn. Các bước sau là side-effect (Notification) <---

                // ==================================================================
                // 5. GỬI THÔNG BÁO (Fire-and-forget)
                // ==================================================================
                try
                {
                    var notifications = new List<CreateNotificationRequest>();

                    // --- 5.1 Thông báo cho GIÁO VIÊN ---
                    if (request.TeacherAssignmentID.HasValue)
                    {
                        var teacherAssign = await _courseTeacherAssignmentService.GetByIdAsync(request.TeacherAssignmentID.Value);
                        // Giả sử cấu trúc: TeacherAssignment -> Teacher -> Account
                        if (teacherAssign?.Teacher?.Account != null)
                        {
                            notifications.Add(new CreateNotificationRequest
                            {
                                UserId = teacherAssign.Teacher.Account.Id.ToString().ToUpperInvariant(),
                                Title = "New Class Assignment",
                                Message = $"You have been assigned to teach class: {classEntity.ClassName} starting from {classEntity.StartDate:dd/MM/yyyy}.",
                                Type = "system",
                                IsRead = false
                            });
                        }
                    }

                    // --- 5.2 Thông báo cho HỌC SINH (TỐI ƯU: KHÔNG QUERY DB) ---
                    if (request.Enrollments != null && request.Enrollments.Any())
                    {
                        // Vì StudentId == AccountId, ta map trực tiếp luôn
                        var studentNotifs = request.Enrollments.Select(item => new CreateNotificationRequest
                        {
                            UserId = item.StudentId.ToString().ToUpperInvariant(), // Dùng luôn StudentId làm UserId
                            Title = "Class Placement Success",
                            Message = $"You have been placed in class {classEntity.ClassName}. Please check your schedule!",
                            Type = "system",
                            IsRead = false
                        });

                        notifications.AddRange(studentNotifs);
                    }

                    // --- 5.3 Gửi tất cả thông báo cùng lúc ---
                    if (notifications.Any())
                    {
                        await _notificationService.CreateManyAsync(notifications);
                    }
                }
                catch (Exception ex)
                {
                    // Chỉ log lỗi, không throw exception để tránh rollback transaction đã thành công
                    Console.WriteLine($"Warning: Failed to send notifications. ClassID: {classEntity.Id}. Error: {ex.Message}");
                }

                return classEntity.Id;
            });
        }


    }
}
