using Domain.Entities;

namespace Domain.Interfaces.IDN
{
    public interface IIDN_TeacherRepository : IBaseRepository<IDN_Teacher>
    {
        Task<IDN_Teacher?> GetTeacherDetailsByIdAsync(Guid id);
    }
}


