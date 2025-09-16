using DTOs.IDN.IDN_Teacher.Requests;
using DTOs.IDN.IDN_Teacher.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IDN
{
    public interface IIDN_TeacherService
    {
        Task<IReadOnlyList<TeacherResponse>> GetAllTeachersAsync();
        Task<TeacherResponse?> GetTeacherByIdAsync(Guid id);
        Task<TeacherResponse?> GetTeacherByCodeAsync(string teacherCode);
        Task<TeacherResponse?> GetTeacherByEmailAsync(string email);
        Task<TeacherDetailResponse?> GetTeacherDetailsAsync(Guid id);
        Task<TeacherDetailResponse> CreateTeacherWithAccountAsync(CreateTeacherRequest dto);
        Task<TeacherResponse> UpdateTeacherAsync(Guid id, UpdateTeacherRequest dto);
        Task<TeacherDetailResponse?> UpdateTeacherProfileAsync(Guid teacherId, UpdateTeacherProfileRequest dto, ClaimsPrincipal user);

        Task<TeacherResponse> RestoreTeacherAsync(Guid id);
        Task<TeacherResponse> SoftDeleteTeacherAsync(Guid id);
    }
}
