using Application.Interfaces.ACAD;
using Application.Interfaces.COM;
using Application.Interfaces.IDN;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using Domain.Interfaces.FIN;
using DTOs.ACAD.ACAD_Class.Requests;
using DTOs.ACAD.ACAD_Class.Responses;
using DTOs.COM.COM_Chat.Requests;
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
        private readonly IIDN_AccountService _accountService;
        private readonly ICOM_ChatService _chatService;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ACAD_ClassService(
            IACAD_ClassRepository classRepo,
            IACAD_ClassMeetingRepository classMeetingRepo,
            ICOM_ChatService chatService,
            IACAD_SyllabusItemRepository sysllabusItemRepo,
            ICOM_NotificationService notificationService,
            IACAD_CourseTeacherAssignmentRepository courseTeacherAssignmentService,
            IACAD_EnrollmentRepository enrollmentRepo,
            IFIN_InvoiceItemRepository invoiceItemRepository,
            IIDN_AccountService accountService,
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
            _chatService = chatService;
            _accountService = accountService;
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

        public async Task<IEnumerable<ClassResponse>> GetClassesByCourseIdAsync2(Guid courseId)
        {
            return await _classRepo.GetClassesByCourseIdAsync2(courseId);
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

        public async Task<List<ClassStaffViewResponse>> GetClassByCourseStaffView(Guid courseId )
        {
            var entities = await _classRepo.GetClassByCourseStaffView(courseId);
            return _mapper.Map<List<ClassStaffViewResponse>>(entities);
        }

        public async Task<List<FeedbackClassResponse>> GetFeedbackClassesByStudentId(Guid studentId)
        {
            return await _classRepo.GetFeedbackClassesByStudentId(studentId);
        }

        public async Task<Guid> CreateClassWithScheduleAsync(CreateClassWithScheduleRequest request)
        {
            // [CONSTANTS]
            var STATUS_ENROLLED = Guid.Parse("148fdc3d-fecc-457d-a539-cc28fd5df900");

            return await _uow.ExecuteInTransactionAsync(async () =>
            {
                // ==================================================================
                // 1. TẠO CLASS (LỚP HỌC)
                // ==================================================================
                var classEntity = _mapper.Map<ACAD_Class>(request);
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
                        ClassID = classEntity.Id,
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
                        var enrollment = await _enrollmentRepo.GetByIdAsync(studentItem.EnrollmentId);

                        if (enrollment != null)
                        {
                            enrollment.ClassID = classEntity.Id;
                            enrollment.EnrollmentStatusID = STATUS_ENROLLED;
                            enrollment.UpdatedAt = DateTime.UtcNow;
                            enrollment.UpdatedBy = request.CreatedBy;

                            _enrollmentRepo.Update(enrollment);

                            // Cập nhật Invoice (Payment Sequence 2)
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
                // 4. COMMIT DATABASE
                // ==================================================================
                await _uow.SaveChangesAsync();

                // ------------------------------------------------------------------
                // CÁC TÁC VỤ SIDE-EFFECT (Notification & Chat)
                // Thực hiện sau khi commit để không block transaction chính
                // ------------------------------------------------------------------

                try
                {
                    // Lấy thông tin Giáo viên để dùng chung cho Notif và Chat
                    string? teacherAccountId = null;
                    if (request.TeacherAssignmentID.HasValue)
                    {
                        var teacherId = await _courseTeacherAssignmentService.GetByIdAsync(request.TeacherAssignmentID.Value);
                        var teacherAssign = await _accountService.GetAccountByIdAsync(teacherId.TeacherID);
                        if (teacherAssign?.AccountId != null)
                        {
                            teacherAccountId = teacherAssign.AccountId.ToString().ToUpperInvariant();
                        }
                    }

                    // ==================================================================
                    // 5. GỬI THÔNG BÁO (NOTIFICATION)
                    // ==================================================================
                    var notifications = new List<CreateNotificationRequest>();

                    // 5.1 Thông báo cho GIÁO VIÊN
                    if (!string.IsNullOrEmpty(teacherAccountId))
                    {
                        notifications.Add(new CreateNotificationRequest
                        {
                            UserId = teacherAccountId,
                            Title = "New Class Assignment",
                            Message = $"You have been assigned to teach class: {classEntity.ClassName} starting from {classEntity.StartDate:dd/MM/yyyy}.",
                            Type = "system",
                            IsRead = false
                        });
                    }

                    // 5.2 Thông báo cho HỌC SINH
                    if (request.Enrollments != null && request.Enrollments.Any())
                    {
                        var studentNotifs = request.Enrollments.Select(item => new CreateNotificationRequest
                        {
                            UserId = item.StudentId.ToString().ToUpperInvariant(),
                            Title = "Class Placement Success",
                            Message = $"You have been placed in class {classEntity.ClassName}. Please check your schedule!",
                            Type = "system",
                            IsRead = false
                        });
                        notifications.AddRange(studentNotifs);
                    }

                    if (notifications.Any())
                    {
                        await _notificationService.CreateManyAsync(notifications);
                    }

                    // ==================================================================
                    // 6. TẠO GROUP CHAT CHO LỚP HỌC [NEW]
                    // ==================================================================

                    // 6.1 Tổng hợp thành viên: Giáo viên + Học sinh
                    var chatMemberIds = new List<string>();

                    // Thêm giáo viên (nếu có)
                    if (!string.IsNullOrEmpty(teacherAccountId))
                    {
                        chatMemberIds.Add(teacherAccountId);
                    }

                    // Thêm học sinh
                    if (request.Enrollments != null)
                    {
                        chatMemberIds.AddRange(request.Enrollments.Select(e => e.StudentId.ToString()));
                    }

                    // 6.2 Gọi Service tạo phòng chat
                    // Chỉ tạo nếu có ít nhất 1 thành viên (hoặc tùy logic business của bạn)
                    if (chatMemberIds.Any())
                    {
                        var createChatRequest = new CreateChatRoomRequest
                        {
                            Name = classEntity.ClassName, // Tên nhóm chat = Tên lớp
                            Type = "group",
                            MemberIds = chatMemberIds.Distinct().ToList() // Loại bỏ ID trùng lặp nếu có
                        };

                        await _chatService.CreateRoomAsync(createChatRequest);
                    }
                }
                catch (Exception ex)
                {
                    // Log lỗi nhưng không throw exception để transaction tạo lớp vẫn thành công
                    // (Vì Chat và Notif là tính năng phụ trợ)
                    Console.WriteLine($"[Warning] Failed to handle side-effects (Notif/Chat) for ClassID: {classEntity.Id}. Error: {ex.Message}");
                }

                return classEntity.Id;
            });
        }


    }
}
