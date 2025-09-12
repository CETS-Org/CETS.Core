using DTOs.IDN.IDN_Student.Requests;
using DTOs.IDN.IDN_Student.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IDN
{
    public interface IIDN_StudentService
    {
        Task<IReadOnlyList<StudentResponse>> GetAllStudentsAsync();
        Task<StudentResponse?> GetStudentByIdAsync(Guid id);
        Task<StudentResponse?> GetStudentByCodeAsync(string code);
        Task<StudentResponse?> UpdateStudentAsync(Guid id, UpdateStudentRequest dto);
        Task<StudentProfileResponse?> UpdateStudentProfileAsync(Guid accountId, UpdateStudentProfileRequest dto, ClaimsPrincipal user);
        Task<StudentResponse> CreateStudentAsync(CreateStudentRequest dto);

        Task<StudentResponse> RestoreStudentAsync(Guid id);
        Task<StudentResponse> SoftDeleteStudentAsync(Guid id);
    }
}
