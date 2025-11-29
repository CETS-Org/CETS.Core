using Application.Interfaces.ACAD;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_Assignment.Responses;
using DTOs.ACAD.ACAD_ClassMeetings.Responses;
using DTOs.ACAD.ACAD_Course.Responses;
using DTOs.ACAD.ACAD_Enrollment.Requests;
using DTOs.ACAD.ACAD_Enrollment.Responses;
using DTOs.ACAD.ACAD_Submission.Responses;
using DTOs.IDN.IDN_Student.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations.ACAD
{
    public class ACAD_EnrollmentService : IACAD_EnrollmentService
    {
        private readonly IACAD_EnrollmentRepository _enrollmentRepo;
        private readonly IACAD_AttendanceRepository _attendanceRepo;    
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ACAD_EnrollmentService(
            IACAD_EnrollmentRepository enrollmentRepo,
            IACAD_AttendanceRepository attendanceRepo,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _enrollmentRepo = enrollmentRepo;
            _attendanceRepo = attendanceRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EnrollmentResponse> EnrollAsync(CreateEnrollmentRequest request)
        {
            var enrollment = _mapper.Map<ACAD_Enrollment>(request);
            enrollment.Id = Guid.NewGuid();
            enrollment.EnrollmentStatusID = Guid.Empty;
            enrollment.CreatedAt = DateTime.UtcNow;

            _enrollmentRepo.Add(enrollment);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<EnrollmentResponse>(enrollment);
        }

        public async Task<IEnumerable<EnrollmentResponse>> GetStudentEnrollmentsAsync(Guid studentId)
        {
            var enrollments = await _enrollmentRepo.GetByStudentAsync(studentId);
            return _mapper.Map<IEnumerable<EnrollmentResponse>>(enrollments);
        }

        public async Task<IEnumerable<EnrollmentResponse>> GetClassEnrollmentsAsync(Guid classId)
        {
            var enrollments = await _enrollmentRepo.GetByClassAsync(classId);
            return _mapper.Map<IEnumerable<EnrollmentResponse>>(enrollments);
        }

        public async Task<EnrollmentDetailResponse?> GetEnrollmentDetailAsync(Guid enrollmentId)
        {
            var enrollment = await _enrollmentRepo.GetDetailAsync(enrollmentId);
            return _mapper.Map<EnrollmentDetailResponse?>(enrollment);
        }

        public async Task<IEnumerable<CourseEnrollmentListResponse>> GetStudentCoursesEnrollmentAsync(Guid studentId)
        {
            var enrollments = await _enrollmentRepo.GetByStudentAsync(studentId);

            return _mapper.Map<IEnumerable<CourseEnrollmentListResponse>>(enrollments);
        }
        public async Task<AcademicResultResponse> GetStudentAcademicResultsAsync(Guid studentId)
        {
            var enrollments = await _enrollmentRepo.GetStudentAcademicResultsAsync(studentId);

            var items = _mapper.Map<List<CourseItemResponse>>(enrollments);

            int passed = items.Count(i => i.StatusCode == "Passed");
            int failed = items.Count(i => i.StatusCode == "Failed");
            int inProgress = items.Count(i => i.StatusCode == "InProgress");

            return new AcademicResultResponse
            {
                TotalCourses = items.Count,
                PassedCourses = passed,
                FailedCourses = failed,
                InProgressCourses = inProgress,
                Items = items
            };
        }
        public async Task<StudentCourseDetailResponse?> GetStudentCourseDetailAsync(Guid studentId, Guid courseId)
        {
            var enrollment = await _enrollmentRepo.GetEnrollmentDetailByStudentAndCourseAsync(studentId, courseId);
            if (enrollment == null)
                return null;

            var result = _mapper.Map<StudentCourseDetailResponse>(enrollment);

            result.Assignments = BuildClassMeetingAssignments(enrollment, studentId);
            var allAssignments = enrollment.Class?.ACAD_ClassMeetings?
            .SelectMany(m => m.ACAD_Assignments)
            .Where(a => !a.IsDeleted)
            .ToList() ?? new List<ACAD_Assignment>();

                    int totalAssignments = allAssignments.Count;
                    int completedOnTime = 0, completedLate = 0, pendingGrading = 0, notSubmitted = 0;

                    foreach (var a in allAssignments)
                    {
                        var submission = a.ACAD_Submissions?
                            .Where(s => s.StudentID == studentId && !s.IsDeleted)
                            .OrderByDescending(s => s.UpdatedAt ?? s.CreatedAt)
                            .FirstOrDefault();

                        if (submission == null)
                        {
                            notSubmitted++;
                            continue;
                        }

                        // Đã nộp
                        if (a.DueAt.HasValue && submission.CreatedAt > a.DueAt)
                        {
                            completedLate++;
                        }
                        else
                        {
                            completedOnTime++;
                        }

                        // Nếu đã nộp mà chưa có điểm → đang chờ chấm
                        if (!submission.Score.HasValue)
                        {
                            pendingGrading++;
                        }
                    }

                // ✅ Gán vào CompletionStats
                result.CompletionStats = new AssignmentCompletionStatsResponse
                {
                    TotalAssignments = totalAssignments,
                    CompletedOnTime = completedOnTime,
                    CompletedLate = completedLate,
                    PendingGrading = pendingGrading,
                    NotSubmitted = notSubmitted
                };


            // 🔹 Tính điểm submission theo tuần
            var allSubmissions = enrollment.Class?.ACAD_ClassMeetings?
                .SelectMany(m => m.ACAD_Assignments)
                .Where(a => a.ACAD_Submissions != null)
                .SelectMany(a => a.ACAD_Submissions)
                .Where(s => s.StudentID == studentId && !s.IsDeleted)
                .ToList() ?? new List<ACAD_Submission>();

            if (allSubmissions.Any())
            {
                var enrollDate = enrollment.CreatedAt; // ngày sinh viên chính thức enroll lớp
                if (enrollDate == DateTime.MinValue)
                    enrollDate = allSubmissions.Min(s => s.CreatedAt); // fallback nếu enrollDate null

                var groupedByWeek = allSubmissions
                    .Where(s => s.CreatedAt > DateTime.MinValue)
                    .GroupBy(s =>
                    {
                        // 🧮 Tính số tuần kể từ ngày enroll
                        var daysDiff = (s.CreatedAt.Date - enrollDate.Date).TotalDays;
                        return (int)Math.Floor(daysDiff / 7) + 1; // tuần học thứ N
                    })
                    .OrderBy(g => g.Key)
                    .Select(g =>
                    {
                        var weekSubs = g.ToList();
                        var gradedSubs = weekSubs.Where(s => s.Score.HasValue).ToList();
                        var totalScore = gradedSubs.Sum(s => s.Score!.Value);
                        var avgScore = gradedSubs.Any() ? gradedSubs.Average(s => s.Score!.Value) : 0m;

                        string performanceLevel;
                        if (avgScore >= 8.5m) performanceLevel = "Excellent";
                        else if (avgScore >= 7m) performanceLevel = "Good";
                        else if (avgScore >= 5.5m) performanceLevel = "Average";
                        else performanceLevel = "Poor";

                        return new WeeklySubmissionPerformanceResponse
                        {
                            WeekNumber = g.Key,
                            TotalSubmissions = weekSubs.Count,
                            GradedSubmissions = gradedSubs.Count,
                            TotalScore = Math.Round(totalScore, 2),
                            AverageScore = Math.Round(avgScore, 2),
                            PerformanceLevel = performanceLevel
                        };
                    })
                    .ToList();

                result.WeeklyPerformance = groupedByWeek;
            }

            return result;
        }

        private static List<ClassMeetingAssignmentResponse> BuildClassMeetingAssignments(ACAD_Enrollment enrollment, Guid studentId)
        {
            var meetingResponses = new List<ClassMeetingAssignmentResponse>();

            if (enrollment.Class?.ACAD_ClassMeetings == null)
                return meetingResponses;

            foreach (var meeting in enrollment.Class.ACAD_ClassMeetings.OrderBy(m => m.Date))
            {
                var meetingResponse = new ClassMeetingAssignmentResponse
                {
                    MeetingId = meeting.Id,
                    MeetingDate = meeting.Date.ToDateTime(TimeOnly.MinValue),
                    Topic = meeting.CoveredTopic?.TopicTitle ?? "(No topic)",
                    Assignments = new List<StudentAssignmentResponse>()
                };

                if (meeting.ACAD_Assignments != null)
                {
                    foreach (var a in meeting.ACAD_Assignments.Where(x => !x.IsDeleted))
                    {
                        var submission = a.ACAD_Submissions
                            .Where(s => s.StudentID == studentId && !s.IsDeleted)
                            .OrderByDescending(s => s.UpdatedAt ?? s.CreatedAt)
                            .FirstOrDefault();

                        string status;
                        if (submission == null)
                            status = "NOT_SUBMITTED";
                        else if (a.DueAt.HasValue && submission.CreatedAt > a.DueAt)
                            status = "LATE_SUBMITTED";
                        else if (submission.Score.HasValue)
                            status = "GRADED";
                        else
                            status = "SUBMITTED";

                        meetingResponse.Assignments.Add(new StudentAssignmentResponse
                        {
                            AssignmentId = a.Id,
                            Title = a.Title,
                            Description = a.Description,
                            DueAt = a.DueAt,
                            SubmittedAt = submission?.UpdatedAt ?? submission?.CreatedAt,
                            Score = submission?.Score,
                            Feedback = submission?.Feedback,
                            SubmissionStatus = status
                        });
                    }
                }

                meetingResponses.Add(meetingResponse);
            }

            return meetingResponses;
        }



        public async Task<LearningPathOverviewResponse?> GetLearningPathOverviewAsync(Guid studentId)
        {
            var enrollments = await _enrollmentRepo.GetByStudentAsync(studentId);
            if (!enrollments.Any())
                return null;

            var student = enrollments.First().Student;
            var studentName = student?.Account?.FullName ?? "Unknown";

            var courseResponses = new List<CourseOverviewItemResponse>();

            foreach (var e in enrollments)
            {
                var course = e.Course;
                var courseId = course.Id;

                // 1️⃣ Tổng số session trong syllabus
                var totalSessions = await _attendanceRepo.CountTotalMeetingsByCourseAsync(courseId);

                // 2️⃣ Attendance stats
                var attendances = await _attendanceRepo.GetByStudentAndCourseAsync(studentId, courseId);
                var totalAttended = attendances.Count(a => a.AttendanceStatus.Code == "Present");
                var totalAbsent = attendances.Count(a => a.AttendanceStatus.Code == "Absent");
                var attendanceRate = attendances.Count == 0
                    ? 0
                    : Math.Round((double)totalAttended / attendances.Count * 100, 1);

                // 3️⃣ Course progress = (Sessions đã học / tổng sessions)
                // Sessions đã học: có attendance Present hoặc đã qua ngày
                var today = DateOnly.FromDateTime(DateTime.UtcNow);

                var completedSessions = attendances.Count(a =>
                    a.AttendanceStatus.Code == "Present" ||
                    (a.Meeting != null && a.Meeting.Date < today)
                );

                // 4️⃣ Lấy tên giáo viên
                var teacherNames = course.ACAD_CourseTeacherAssignments?
                    .Select(cta => cta.Teacher.Account.FullName)
                    .Distinct()
                    .ToList() ?? new List<string>();

                courseResponses.Add(new CourseOverviewItemResponse
                {
                    CourseId = course.Id,
                    CourseCode = course.CourseCode,
                    CourseName = course.CourseName,
                    TeacherNames = teacherNames,
                    Instructor = teacherNames.FirstOrDefault() ?? "N/A",
                    StatusCode = e.EnrollmentStatus?.Code ?? "InProgress",
                    StatusName = e.EnrollmentStatus?.Name ?? "In Progress",
                    CourseProgress = $"{completedSessions}/{totalSessions}"
                });
            }

            // 5️⃣ Tính overall stats
            var totalCourses = courseResponses.Count;
            var passedCourses = courseResponses.Count(c => c.StatusCode == "Passed");
            var failedCourses = courseResponses.Count(c => c.StatusCode == "Failed");
            var inProgressCourses = courseResponses.Count(c => c.StatusCode == "InProgress");

            var allAttendances = await _attendanceRepo.GetByStudentAsync(studentId);
            var totalSessionsAll = allAttendances.Count();
            var totalAttendedAll = allAttendances.Count(a => a.AttendanceStatus.Code == "Present");
            var totalAbsentAll = allAttendances.Count(a => a.AttendanceStatus.Code == "Absent");

            var overallAttendanceRate = totalSessionsAll == 0 ? 0 :
                Math.Round((double)totalAttendedAll / totalSessionsAll * 100, 1);

            // 6️⃣ Build response
            return new LearningPathOverviewResponse
            {
                StudentId = studentId,
                StudentName = studentName,
                OverallStats = new OverallStatsResponse
                {
                    TotalCourses = totalCourses,
                    PassedCourses = passedCourses,
                    FailedCourses = failedCourses,
                    InProgressCourses = inProgressCourses,
                    TotalSessions = totalSessionsAll,
                    TotalAttended = totalAttendedAll,
                    TotalAbsent = totalAbsentAll,
                    OverallAttendanceRate = overallAttendanceRate
                },
                Courses = courseResponses
            };
        }


        public async Task<WaitingStudentSearchResult> GetStudentWaitListAsync(Guid courseId, string? query, int page, int pageSize)
        {
            // 1. Lấy danh sách Enrollments từ Repo
            // Lưu ý: Nếu dữ liệu lớn (>1000 dòng), bạn nên chuyển logic filter/paging vào trong Repo (IQueryable)
            var allEnrollments = await _enrollmentRepo.GetStudentWaitList(courseId);

            // 2. Xử lý tìm kiếm (In-Memory Filtering)
            if (!string.IsNullOrWhiteSpace(query))
            {
                query = query.Trim().ToLower();

                // Dùng ?. để tránh null
                allEnrollments = allEnrollments.Where(e =>
                    (e.Student?.StudentCode?.ToLower().Contains(query) == true) ||
                    (e.Student?.Account?.FullName?.ToLower().Contains(query) == true) ||
                    (e.Student?.Account?.PhoneNumber?.Contains(query) == true) ||
                    (e.Student?.Account?.Email?.ToLower().Contains(query) == true)
                );
            }

            // 3. Tính toán số liệu
            var totalCount = allEnrollments.Count();

            // Normalize page
            if (page < 1) page = 1;

            // 4. Phân trang & Map sang DTO (Projection)
            // Thực hiện Select ngay đây để lấy đúng các trường cần thiết
            var items = allEnrollments
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new WaitingStudentResponse
                {
                    EnrollmentId = e.Id,                 // Lấy trực tiếp ID Enrollment
                    StudentId = e.StudentID,
                    StudentCode = e.Student?.StudentCode ?? string.Empty,
                    FullName = e.Student?.Account?.FullName ?? "Unknown", // Flatten dữ liệu từ Account
                    Phone = e.Student?.Account?.PhoneNumber,
                    Email = e.Student?.Account?.Email
                })
                .ToList();

            // 5. Trả về kết quả
            return new WaitingStudentSearchResult
            {
                Page = page,
                PageSize = pageSize,
                Total = totalCount,
                HasMore = totalCount > (page * pageSize),
                Items = items
            };
        }

        public async Task<BulkUpdateFinalGradesResponse> BulkUpdateFinalGradesAsync(BulkUpdateFinalGradesRequest request)
        {
            var response = new BulkUpdateFinalGradesResponse
            {
                Success = true,
                Data = new BulkUpdateFinalGradesData
                {
                    Results = new List<FinalGradeUpdateResult>()
                }
            };

            // Process each final grade update
            foreach (var gradeUpdate in request.FinalGrades)
            {
                try
                {
                    // Retrieve the enrollment from database
                    var enrollment = await _enrollmentRepo.GetByIdAsync(gradeUpdate.EnrollmentId);

                    if (enrollment == null)
                    {
                        // Enrollment not found
                        response.Data.Results.Add(new FinalGradeUpdateResult
                        {
                            EnrollmentId = gradeUpdate.EnrollmentId,
                            Status = "failed",
                            Error = "Enrollment not found"
                        });
                        response.Data.FailedCount++;
                        continue;
                    }

                    // Update final grade
                    enrollment.FinalGrade = gradeUpdate.FinalGrade;
                    enrollment.UpdatedAt = DateTime.UtcNow;

                    _enrollmentRepo.Update(enrollment);

                    response.Data.Results.Add(new FinalGradeUpdateResult
                    {
                        EnrollmentId = gradeUpdate.EnrollmentId,
                        Status = "success"
                    });
                    response.Data.UpdatedCount++;
                }
                catch (Exception ex)
                {
                    response.Data.Results.Add(new FinalGradeUpdateResult
                    {
                        EnrollmentId = gradeUpdate.EnrollmentId,
                        Status = "failed",
                        Error = ex.Message
                    });
                    response.Data.FailedCount++;
                }
            }

            // Save all changes to database
            await _unitOfWork.SaveChangesAsync();

            response.Message = response.Data.FailedCount > 0
                ? $"Updated {response.Data.UpdatedCount} record(s). {response.Data.FailedCount} failed."
                : $"Successfully updated {response.Data.UpdatedCount} record(s)!";

            return response;
        }
    }
}

