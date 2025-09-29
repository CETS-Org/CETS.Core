using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.ACAD
{
    public class ACAD_CourseSkillRepository : BaseRepository<ACAD_CourseSkill>, IACAD_CourseSkillRepository
    {
        public ACAD_CourseSkillRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ACAD_CourseSkill>> GetByCourseAsync(Guid courseId)
        {
            return await _context.ACAD_CourseSkills
                .Include(cs => cs.Course)
                .Include(cs => cs.Skill)
                .Where(cs => cs.CourseID == courseId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ACAD_CourseSkill>> GetBySkillAsync(Guid skillId)
        {
            return await _context.ACAD_CourseSkills
                .Include(cs => cs.Course)
                .Include(cs => cs.Skill)
                .Where(cs => cs.SkillID == skillId)
                .ToListAsync();
        }

        public async Task<ACAD_CourseSkill?> GetByCourseAndSkillAsync(Guid courseId, Guid skillId)
        {
            return await _context.ACAD_CourseSkills
                .Include(cs => cs.Course)
                .Include(cs => cs.Skill)
                .FirstOrDefaultAsync(cs => cs.CourseID == courseId && cs.SkillID == skillId);
        }
    }
}
