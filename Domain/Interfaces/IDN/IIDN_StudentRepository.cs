using Domain.Entities;

namespace Domain.Interfaces.IDN
{
    public interface IIDN_StudentRepository : IBaseRepository<IDN_Student>
    {
        Task<IDN_Student?> GetStudentWithAccountAsync(Guid accountId);
        Task<IDN_Student?> GetStudentWithStudentCode(string studentCode);
    }
}


