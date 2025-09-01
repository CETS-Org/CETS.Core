using Domain.Entities;
using DTOs.IDN_TeacherCredential.Requests;
using DTOs.IDN_TeacherCredential.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IDN
{
    public interface IIDN_TeacherCredentialService : IBaseService<IDN_TeacherCredential, TeacherCredentialResponse, UpdateTeacherCredentialRequest, CreateTeacherCredentialRequest>
    {
        Task<IReadOnlyList<TeacherCredentialResponse>> GetCredentialsByTeacherIdAsync(Guid teacherId);
        Task<IReadOnlyList<TeacherCredentialResponse>> GetCredentialsByTeacherCodeAsync(string teacherCode);
        Task<IReadOnlyList<CredentialTypeResponse>> GetCredentialTypesAsync();
    }
}
