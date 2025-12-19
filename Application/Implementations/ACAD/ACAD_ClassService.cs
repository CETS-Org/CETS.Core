using Application.Interfaces.ACAD;
using Application.Interfaces.COM;
using Application.Interfaces.CORE;
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
using DTOs.IDN.IDN_Student.Responses;
using Microsoft.EntityFrameworkCore;
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
        private readonly ICORE_LookUpService _lookUpService;
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
            ICORE_LookUpService lookUpService,
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
            _lookUpService = lookUpService;
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
                entity.CreatedAt = DateTime.Now;

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
                entity.UpdatedAt = DateTime.Now;

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
                entity.UpdatedAt = DateTime.Now;
               

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
            // ==================================================================
            // PHASE 1: PREPARE DATA 
            // (Đọc dữ liệu TUẦN TỰ để tránh lỗi DbContext concurrency)
            // ==================================================================

            // 1. Lấy Constant Status
            var statusEnrolled = await _lookUpService.GetByCodeAsync("EnrollmentStatus", "Enrolled");

            // 2. Lấy thông tin tài khoản Giáo viên (Chạy tuần tự)
            var teacherAccountId = await GetTeacherAccountAsync(request.TeacherAssignmentID);
            var subTeacherAccountId = await GetTeacherAccountAsync(request.SubTeacherAssignmentID);

            // 3. Bulk Read Enrollments (Lấy 1 lần danh sách Enrollment)
            var enrollmentIds = request.Enrollments?.Select(x => x.EnrollmentId).Distinct().ToList() ?? new List<Guid>();
            var enrollments = await _enrollmentRepo.GetByIdsAsync(enrollmentIds);

            // 4. Bulk Read Invoice Items (Lấy 1 lần danh sách InvoiceItem liên quan)
            // Lọc ra các Enrollment có InvoiceID
            var invoiceIds = enrollments
                            .Where(x => x.InvoiceID.HasValue)
                            .Select(x => x.InvoiceID.Value)
                            .Distinct()
                            .ToList();

            var allInvoiceItems = await _invoiceItemRepository.GetByInvoiceIdsAsync(invoiceIds);

            // ==================================================================
            // PHASE 2: EXECUTE TRANSACTION (Chỉ thực hiện Ghi/Sửa DB)
            // ==================================================================

            var classId = await _uow.ExecuteInTransactionAsync(async () =>
            {
                // 1. TẠO CLASS
                var classEntity = _mapper.Map<ACAD_Class>(request);
                classEntity.EnrolledCount = request.Enrollments?.Count ?? 0;
                classEntity.IsActive = true;
                classEntity.IsDeleted = false;
                classEntity.CreatedAt = DateTime.Now;
                _classRepo.Add(classEntity);

                // 2. TẠO MEETINGS
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
                        SubTeacherAssignmentID = request.SubTeacherAssignmentID,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedAt = DateTime.Now
                    }).ToList();

                    _classMeetingRepo.AddRange(meetings);
                }

                // 3. CẬP NHẬT ENROLLMENT & INVOICE (Xử lý trên List RAM đã load ở Phase 1)
                if (enrollments.Any())
                {
                    foreach (var enrollment in enrollments)
                    {
                        // Cập nhật Enrollment
                        enrollment.ClassID = classEntity.Id;
                        if (statusEnrolled != null)
                        {
                            enrollment.EnrollmentStatusID = statusEnrolled.LookUpId;
                        }
                        enrollment.UpdatedAt = DateTime.Now;
                        enrollment.UpdatedBy = request.CreatedBy;

                        // Cập nhật Invoice Item (Tìm trong list đã load sẵn)
                        if (enrollment.InvoiceID.HasValue)
                        {
                            // Tìm InvoiceItem tương ứng trong list memory (Không query lại DB)
                            var targetItem = allInvoiceItems.FirstOrDefault(x =>
                                x.InvoiceID == enrollment.InvoiceID.Value && // So sánh Guid
                                x.PaymentSequence == 2);

                            if (targetItem != null)
                            {
                                targetItem.DueDate = classEntity.StartDate.AddDays(30);
                                // Đánh dấu update InvoiceItem
                                _invoiceItemRepository.Update(targetItem);
                            }
                        }
                    }

                    // Update Bulk Enrollments
                    _enrollmentRepo.UpdateRange(enrollments);
                }

                // 4. COMMIT DATABASE
                await _uow.SaveChangesAsync();

                return classEntity.Id;
            });

            // ==================================================================
            // PHASE 3: SIDE EFFECTS (Chạy nền sau khi Transaction thành công)
            // ==================================================================

            // Fire-and-forget: Gọi hàm async mà không cần await để trả về kết quả ngay cho User
            _ = HandleSideEffectsAsync(classId, request, teacherAccountId, subTeacherAccountId);

            return classId;
        }

        // ==================================================================
        // CÁC HÀM PHỤ TRỢ (HELPER METHODS)
        // ==================================================================

        // 1. Hàm lấy Teacher Account ID (Đã sửa lỗi Guid?)
        private async Task<string?> GetTeacherAccountAsync(Guid? assignmentId)
        {
            if (!assignmentId.HasValue) return null;

            var assign = await _courseTeacherAssignmentService.GetByIdAsync(assignmentId.Value);
            if (assign == null) return null;

            var account = await _accountService.GetAccountByIdAsync(assign.TeacherID);

            // Sửa lỗi: account.AccountId là Guid (không null), nên không dùng account.AccountId?
            return account?.AccountId.ToString().ToUpperInvariant();
        }

        // 2. Hàm xử lý Notification và Chat (Chạy ngầm)
        private async Task HandleSideEffectsAsync(Guid classId, CreateClassWithScheduleRequest request, string? teacherAccountId, string? subTeacherAccountId)
        {
            try
            {
                // Query nhẹ lại thông tin lớp để lấy ClassName và StartDate chính xác nhất
                // (Hoặc có thể truyền từ hàm chính vào nếu muốn tiết kiệm 1 query)
                var classInfo = await _classRepo.GetByIdAsync(classId);
                if (classInfo == null) return;

                var notifications = new List<CreateNotificationRequest>();

                // --- Tạo Notification ---

                // Cho Giáo viên
                if (!string.IsNullOrEmpty(teacherAccountId))
                {
                    notifications.Add(new CreateNotificationRequest
                    {
                        UserId = teacherAccountId,
                        Title = "New Class Assignment",
                        Message = $"You have been assigned to teach class: {classInfo.ClassName} starting from {classInfo.StartDate:dd/MM/yyyy}.",
                        Type = "system",
                        IsRead = false
                    });
                }

                // Cho Sub Teacher
                if (!string.IsNullOrEmpty(subTeacherAccountId))
                {
                    notifications.Add(new CreateNotificationRequest
                    {
                        UserId = subTeacherAccountId,
                        Title = "New Class Assignment",
                        Message = $"You have been assigned to teach class: {classInfo.ClassName} starting from {classInfo.StartDate:dd/MM/yyyy}.",
                        Type = "system",
                        IsRead = false
                    });
                }

                // Cho Học sinh
                if (request.Enrollments != null && request.Enrollments.Any())
                {
                    notifications.AddRange(request.Enrollments.Select(item => new CreateNotificationRequest
                    {
                        UserId = item.StudentId.ToString().ToUpperInvariant(),
                        Title = "Class Placement Success",
                        Message = $"You have been placed in class {classInfo.ClassName}. Please check your schedule!",
                        Type = "system",
                        IsRead = false
                    }));
                }

                if (notifications.Any())
                {
                    await _notificationService.CreateManyAsync(notifications);
                }

                // --- Tạo Group Chat ---

                var chatMemberIds = new List<string>();
                if (!string.IsNullOrEmpty(teacherAccountId)) chatMemberIds.Add(teacherAccountId);
                if (!string.IsNullOrEmpty(subTeacherAccountId)) chatMemberIds.Add(subTeacherAccountId);
                if (request.Enrollments != null)
                {
                    chatMemberIds.AddRange(request.Enrollments.Select(e => e.StudentId.ToString()));
                }

                // Chỉ tạo chat nếu có thành viên
                if (chatMemberIds.Any())
                {
                    await _chatService.CreateRoomAsync(new CreateChatRoomRequest
                    {
                        Name = classInfo.ClassName,
                        Type = "group",
                        MemberIds = chatMemberIds.Distinct().ToList()
                    });
                }
            }
            catch (Exception ex)
            {
                // Log lỗi để debug, không throw exception để tránh ảnh hưởng luồng chính (dù luồng chính đã return rồi)
                Console.WriteLine($"[Background Job Error] ClassID: {classId}. Details: {ex.Message}");
            }
        }

        public async Task<ClassDetailForEditResponse> GetClassDetailForEditAsync(Guid classId)
        {
            // Gọi Repository đã viết ở trên
            var classEntity = await _classRepo.GetClassWithDetailForEditAsync(classId);
    

            if (classEntity == null)
                throw new KeyNotFoundException("Class not found or deleted.");

            // Map Entity -> DTO (Thủ công hoặc dùng AutoMapper nếu đã config)
            return new ClassDetailForEditResponse
            {
                Id = classEntity.Id,
                CourseId = classEntity.TeacherAssignment.CourseID,
                ClassName = classEntity.ClassName,
                TeacherAssignmentID = classEntity.TeacherAssignment.Id,
                TeacherName = classEntity.TeacherAssignment.Teacher.Account.FullName,
                // Lấy phòng từ buổi học đầu tiên (nếu có)
                RoomId = classEntity.ACAD_ClassMeetings.FirstOrDefault()?.RoomID,
                StartDate = classEntity.StartDate,
                EndDate = classEntity.EndDate,
                Capacity = classEntity.Capacity,
                Status = classEntity.IsActive ? "active" : "inactive",

                // Map lịch học
                Schedules = classEntity.ACAD_ClassMeetings
                    .Where(m => !m.IsDeleted)
                    .Select(m => new ClassMeetingScheduleDto
                    {
                        SlotID = m.SlotID,
                        Date = m.Date,
                        RoomID = m.RoomID,
                        SyllabusItemID = m.CoveredTopicID
                    }).ToList(),

                // Map danh sách học sinh đang Active trong lớp
                Enrollments = classEntity.ACAD_Enrollments
                    .Where(e => !e.IsDeleted && e.ClassID == classId)
                    .Select(e => new WaitingStudentResponse
                    {
                        EnrollmentId = e.Id,
                        StudentId = e.StudentID,
                        StudentCode = e.Student.StudentCode,
                        FullName = e.Student.Account.FullName,
                        Email = e.Student.Account.Email ?? "",
                        Phone = e.Student.Account.PhoneNumber ?? ""
                    }).ToList()
            };
        }


            public async Task UpdateClassCompositeAsync(Guid classId, UpdateClassCompositeRequest request)
            {
                // [1] Lấy các Constant trạng thái từ Lookup Service (để tránh hardcode ID)
                var STATUS_ENROLLED = await _lookUpService.GetByCodeAsync("EnrollmentStatus", "Enrolled");
                var STATUS_WAITING = await _lookUpService.GetByCodeAsync("EnrollmentStatus", "Pending"); // Hoặc "Waiting" tùy DB của bạn

                await _uow.ExecuteInTransactionAsync(async () =>
                {
                    // ==================================================================
                    // 1. UPDATE CLASS INFO (Cập nhật thông tin lớp)
                    // ==================================================================
                    var classEntity = await _classRepo.GetByIdAsync(classId);
                    if (classEntity == null) throw new KeyNotFoundException("Class not found.");

                

                    // ==================================================================
                    // 2. SYNC ENROLLMENTS (Đồng bộ danh sách học sinh)
                    // ==================================================================
                    if (request.EnrollmentIds != null)
                    {
                        // A. Lấy danh sách EnrollmentId hiện tại của lớp
                        var currentClassEnrollments = await _enrollmentRepo.GetQueryable()
                            .Where(e => e.ClassID == classId && !e.IsDeleted)
                            .ToListAsync();

                        var currentEnrollmentIds = currentClassEnrollments.Select(e => e.Id).ToList();
                        var newEnrollmentIds = request.EnrollmentIds;

                        // B. Phân loại: Cần Thêm và Cần Xóa
                        var enrollmentsToAdd = newEnrollmentIds.Except(currentEnrollmentIds).ToList();
                        var enrollmentsToRemove = currentEnrollmentIds.Except(newEnrollmentIds).ToList();

                        // --- B1. Xử lý THÊM (Waiting -> Enrolled) ---
                        if (enrollmentsToAdd.Any())
                        {
                            var waitingEnrollments = await _enrollmentRepo.GetQueryable()
                                .Where(e => enrollmentsToAdd.Contains(e.Id)
                                            && e.ClassID == null // Chỉ lấy nếu chưa có lớp
                                            && !e.IsDeleted)
                                .ToListAsync();

                            foreach (var enrollment in waitingEnrollments)
                            {
                                enrollment.ClassID = classId;
                                enrollment.EnrollmentStatusID = STATUS_ENROLLED.LookUpId;
                                enrollment.UpdatedBy = request.UpdatedBy;
                                enrollment.UpdatedAt = DateTime.Now;

                                _enrollmentRepo.Update(enrollment);
                            }
                        }

                        // --- B2. Xử lý XÓA (Enrolled -> Waiting) ---
                        if (enrollmentsToRemove.Any())
                        {
                            var enrollmentsToKick = currentClassEnrollments
                                .Where(e => enrollmentsToRemove.Contains(e.Id))
                                .ToList();

                            foreach (var enrollment in enrollmentsToKick)
                            {
                                enrollment.ClassID = null; // Gỡ khỏi lớp
                                enrollment.EnrollmentStatusID = STATUS_WAITING.LookUpId; // Quay về hàng chờ
                                enrollment.UpdatedBy = request.UpdatedBy;
                                enrollment.UpdatedAt = DateTime.Now;

                                _enrollmentRepo.Update(enrollment);
                            }
                        }
                    }

                    // ==================================================================
                    // 3. COMMIT DATABASE (Lưu SQL trước)
                    // ==================================================================
                    await _uow.SaveChangesAsync();

                    // ==================================================================
                    // 4. SIDE-EFFECTS: Đồng bộ nhóm Chat (MongoDB)
                    // ==================================================================
                    try
                    {
                        // A. Tạo danh sách thành viên CHUẨN (Giáo viên + Học sinh đang Active)
                        var chatMemberIds = new List<string>();

                        // 4.1 Lấy ID Giáo viên (nếu có)
                        if (request.TeacherAssignmentID.HasValue)
                        {
                            //This line is now not get teacheAssign
                            var teacherAssign = await _courseTeacherAssignmentService.GetByIdAsync(request.TeacherAssignmentID.Value);
                            if (teacherAssign?.Teacher?.Account != null)
                            {
                                chatMemberIds.Add(teacherAssign.Teacher.Account.Id.ToString().ToUpperInvariant());
                            }
                        }

                        if (request.SubTeacherAssignmentID.HasValue)
                        {
                            //This line is now not get teacheAssign
                            var subTeacherAssign = await _courseTeacherAssignmentService.GetByIdAsync(request.SubTeacherAssignmentID.Value);
                            if (subTeacherAssign?.Teacher?.Account != null)
                            {
                                chatMemberIds.Add(subTeacherAssign.Teacher.Account.Id.ToString().ToUpperInvariant());
                            }
                        }

                        // 4.2 Lấy ID Học sinh (Query lại DB để lấy danh sách "Sạch" nhất sau khi Add/Remove)
                        // Chỉ lấy những người có Status là ENROLLED
                        var currentStudents = await _enrollmentRepo.GetQueryable()
                            .Where(e => e.ClassID == classId
                                        && !e.IsDeleted
                                        && e.EnrollmentStatusID == STATUS_ENROLLED.LookUpId)
                            .Select(e => e.StudentID.ToString().ToUpperInvariant())
                            .ToListAsync();

                        chatMemberIds.AddRange(currentStudents);

                        // B. Gọi Chat Service để cập nhật (Ghi đè danh sách mới vào Mongo)
                        // Giả định Tên nhóm chat == Tên lớp (ClassName)
                        if (chatMemberIds.Any())
                        {
                            await _chatService.UpdateGroupMembersByRoomNameAsync(classEntity.ClassName, chatMemberIds);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log warning để không làm rollback transaction chính
                        Console.WriteLine($"[Warning] Failed to sync chat members for ClassID: {classId}. Error: {ex.Message}");
                    }

                    return true;
                });
            }





    }
}
