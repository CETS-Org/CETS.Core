using Application.Interfaces.ACAD;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_Assignment.Responses;
using DTOs.ACAD.ACAD_Course.Responses;
using DTOs.ACAD.ACAD_Enrollment.Requests;
using DTOs.ACAD.ACAD_Enrollment.Responses;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ACAD_EnrollmentService(
            IACAD_EnrollmentRepository enrollmentRepo,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _enrollmentRepo = enrollmentRepo;
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

            result.Assignments = BuildAssignmentList(enrollment, studentId);

            return result;
        }

        private static List<StudentAssignmentResponse> BuildAssignmentList(ACAD_Enrollment enrollment, Guid studentId)
        {
            var assignments = new List<StudentAssignmentResponse>();

            if (enrollment.Class?.ACAD_ClassMeetings == null)
                return assignments;

            foreach (var meeting in enrollment.Class.ACAD_ClassMeetings)
            {
                if (meeting.ACAD_Assignments == null) continue;

                foreach (var a in meeting.ACAD_Assignments.Where(x => !x.IsDeleted))
                {
                    var sub = a.ACAD_Submissions
                        .Where(s => s.StudentID == studentId && !s.IsDeleted)
                        .OrderByDescending(s => s.UpdatedAt ?? s.CreatedAt)
                        .FirstOrDefault();

                    string status;
                    if (sub == null)
                        status = "NOT_SUBMITTED";
                    else if (a.DueAt.HasValue && sub.CreatedAt > a.DueAt)
                        status = "LATE_SUBMITTED";
                    else if (sub.Score.HasValue)
                        status = "GRADED";
                    else
                        status = "SUBMITTED";

                    assignments.Add(new StudentAssignmentResponse
                    {
                        AssignmentId = a.Id,
                        Title = a.Title,
                        Description = a.Description,
                        DueAt = a.DueAt,
                        SubmittedAt = sub?.UpdatedAt ?? sub?.CreatedAt,
                        Score = sub?.Score,
                        Feedback = sub?.Feedback,
                        SubmissionStatus = status
                    });
                }
            }

            return assignments.OrderBy(x => x.DueAt).ToList();
        }
    }

}

