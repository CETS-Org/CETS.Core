using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ACAD
{
    public interface IACAD_AssignmentService
    {

        Task<ACAD_Assignment> CreateAssignmentAsync(ACAD_Assignment assignment);
        Task<IEnumerable<ACAD_Assignment>> GetAssignmentsByClassMeetingAsync(Guid classMeetingId);
        Task<IEnumerable<ACAD_Assignment>> GetAssignmentsByTeacherAsync(Guid teacherId);
        Task<ACAD_Assignment?> GetAssignmentByIdAsync(Guid id);
        Task UpdateAssignmentAsync(ACAD_Assignment assignment);
        Task DeleteAssignmentAsync(Guid id);
    }
}
