using Domain.Entities;
using DTOs.ACAD.ACAD_Assignment.Requests;
using DTOs.ACAD.ACAD_Assignment.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ACAD
{
    public interface IACAD_AssignmentService
    {
        Task<AssignmentResponse> CreateAssignmentAsync(CreateAssignmentRequest request);

        Task<IEnumerable<AssignmentResponse>> GetAssignmentsByClassMeetingAsync(Guid classMeetingId);

        Task<IEnumerable<AssignmentResponse>> GetAssignmentsByTeacherAsync(Guid teacherId);

        Task<AssignmentResponse?> GetAssignmentByIdAsync(Guid id);

        Task<AssignmentResponse> UpdateAssignmentAsync(UpdateAssignmentRequest request);

        Task DeleteAssignmentAsync(Guid id);
        Task<IEnumerable<AssignmentResponse>> GetAssignmentsWithSubmissions(Guid classMeetingId, Guid studentId);
    }
}
