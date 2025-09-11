using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.ACAD
{
    public class ACAD_CourseBenefitRepository : BaseRepository<ACAD_CourseBenefit>, IACAD_CourseBenefitRepository
    {
        public ACAD_CourseBenefitRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ACAD_CourseBenefit>> GetBenefitsByCourseIdAsync(Guid courseId)
        {
            return await _context.ACAD_CourseBenefits
                .Include(cb => cb.Course)
                .Include(cb => cb.Benefit)
                .Where(cb => cb.CourseID == courseId)
                .ToListAsync();
        }

        public async Task<ACAD_CourseBenefit?> GetCourseBenefitAsync(Guid courseId, Guid benefitId)
        {
            return await _context.ACAD_CourseBenefits
                .Include(cb => cb.Course)
                .Include(cb => cb.Benefit)
                .FirstOrDefaultAsync(cb => cb.CourseID == courseId && cb.BenefitID == benefitId);
        }

        public async Task<bool> ExistsAsync(Guid courseId, Guid benefitId)
        {
            return await _context.ACAD_CourseBenefits
                .AnyAsync(cb => cb.CourseID == courseId && cb.BenefitID == benefitId);
        }

        public override async Task<ACAD_CourseBenefit?> GetByIdAsync(Guid id)
        {
            return await _context.ACAD_CourseBenefits
                .Include(cb => cb.Course)
                .Include(cb => cb.Benefit)
                .FirstOrDefaultAsync(cb => cb.Id == id);
        }

        public override async Task<IReadOnlyList<ACAD_CourseBenefit>> GetAllAsync()
        {
            return await _context.ACAD_CourseBenefits
                .Include(cb => cb.Course)
                .Include(cb => cb.Benefit)
                .ToListAsync();
        }
    }
}
