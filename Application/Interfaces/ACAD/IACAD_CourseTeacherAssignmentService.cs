using DTOs.ACAD.ACAD_CourseTeacherAssignment.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ACAD
{
    public interface IACAD_CourseTeacherAssignmentService
    {
        Task<IEnumerable<CourseListAssignmentResponse>> GetCoursesByTeacherIdAsync(Guid teacherId);
    }
}
