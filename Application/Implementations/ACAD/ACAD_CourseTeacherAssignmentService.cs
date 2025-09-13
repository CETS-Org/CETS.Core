using Application.Interfaces.ACAD;
using AutoMapper;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_CourseTeacherAssignment.Responses;
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
        private readonly IMapper _mapper;

        public ACAD_CourseTeacherAssignmentService(IACAD_CourseTeacherAssignmentRepository courseAssignmentRepository, IMapper mapper)
        {
            _courseAssignmentRepository = courseAssignmentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CourseListAssignmentResponse>> GetCoursesByTeacherIdAsync(Guid teacherId)
        {
            var courses = await _courseAssignmentRepository.GetCoursesByTeacherIdAsync(teacherId);
            return _mapper.Map<IEnumerable<CourseListAssignmentResponse>>(courses);
        }

    }
}
