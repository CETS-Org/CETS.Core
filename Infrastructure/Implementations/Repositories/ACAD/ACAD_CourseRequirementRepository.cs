using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.ACAD
{
    public class ACAD_CourseRequirementRepository : BaseRepository<ACAD_CourseRequirement>, IACAD_CourseRequirementRepository
    {
        public ACAD_CourseRequirementRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ACAD_CourseRequirement>> GetRequirementsByCourseIdAsync(Guid courseId)
        {
            return await _context.ACAD_CourseRequirements
                .Include(cr => cr.Course)
                .Include(cr => cr.Requirement)
                .Where(cr => cr.CourseID == courseId)
                .ToListAsync();
        }

        public async Task<ACAD_CourseRequirement?> GetCourseRequirementAsync(Guid courseId, Guid requirementId)
        {
            return await _context.ACAD_CourseRequirements
                .Include(cr => cr.Course)
                .Include(cr => cr.Requirement)
                .FirstOrDefaultAsync(cr => cr.CourseID == courseId && cr.RequirementID == requirementId);
        }

        public async Task<bool> ExistsAsync(Guid courseId, Guid requirementId)
        {
            return await _context.ACAD_CourseRequirements
                .AnyAsync(cr => cr.CourseID == courseId && cr.RequirementID == requirementId);
        }

        public override async Task<ACAD_CourseRequirement?> GetByIdAsync(Guid id)
        {
            return await _context.ACAD_CourseRequirements
                .Include(cr => cr.Course)
                .Include(cr => cr.Requirement)
                .FirstOrDefaultAsync(cr => cr.Id == id);
        }

        public override async Task<IReadOnlyList<ACAD_CourseRequirement>> GetAllAsync()
        {
            return await _context.ACAD_CourseRequirements
                .Include(cr => cr.Course)
                .Include(cr => cr.Requirement)
                .ToListAsync();
        }
    }
}
