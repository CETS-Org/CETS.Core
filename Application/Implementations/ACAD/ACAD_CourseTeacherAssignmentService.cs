using Application.Interfaces.ACAD;
using AutoMapper;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_Course.Responses;
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
        public async Task<IEnumerable<TeachingClassResponse>> GetTeachingClassesByTeacherIdAsync(Guid teacherId)
        {
            //var classes = await _courseAssignmentRepository.GetCourseTeacherAssignmentsByTeacherIdAsync(teacherId);
            //IEnumerable<ClassSession> classSessions = new List<ClassSession>();
            //foreach (var classItem in classes)
            //{
            //    var classmeetings = classItem.ACAD_ClassMeetings.Where(cm => cm.IsStudy!).ToList();
            //    var classSession = new ClassSession()
            //    {
            //        slot = classmeetings.FirstOrDefault()?.Slot.Name,
            //        Capacity = classItem.ACAD_Classes.,

            //    };
            //}
            return null;
        }

        public async Task<IEnumerable<TeachingCourseResponse>> GetAllTeachingCourses(Guid teacherId)
        {
            var courseAssignments = await _courseAssignmentRepository.GetCoursesByTeacherIdAsync(teacherId);
            return _mapper.Map<IEnumerable<TeachingCourseResponse>>(courseAssignments);
        }
    }
}