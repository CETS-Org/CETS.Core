using Application.Interfaces.ACAD;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
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

    }
}
