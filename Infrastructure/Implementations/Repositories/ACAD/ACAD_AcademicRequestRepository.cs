using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.ACAD
{
    public class ACAD_AcademicRequestRepository : BaseRepository<ACAD_AcademicRequest>, IACAD_AcademicRequestRepository
    {
        public ACAD_AcademicRequestRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<ACAD_AcademicRequest>> GetByStudentAsync(Guid studentId)
        {
            return await _context.ACAD_AcademicRequests
                .Include(r => r.Student)
                .Where(r => r.StudentID == studentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ACAD_AcademicRequest>> GetByStatusAsync(Guid statusId)
        {
            return await _context.ACAD_AcademicRequests
                .Include(r => r.Student)
                .Where(r => r.AcademicRequestStatusID == statusId)
                .ToListAsync();
        }
    }
}


