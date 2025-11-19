using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.ACAD
{
    public class ACAD_PlacementQuestionRepository : BaseRepository<ACAD_PlacementQuestion>, IACAD_PlacementQuestionRepository
    {
        public ACAD_PlacementQuestionRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<ACAD_PlacementQuestion?> GetByIdAsync(Guid id)
        {
            return await _context.ACAD_PlacementQuestions
                .Include(q => q.Skill)
                .Include(q => q.QuestionType)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<IEnumerable<ACAD_PlacementQuestion>> GetQuestionsByCriteriaAsync(
            Guid questionTypeId, 
            int difficulty, 
            Guid? skillTypeId = null)
        {
            var query = _context.ACAD_PlacementQuestions
                .Where(q => !q.IsDeleted 
                    && q.QuestionTypeID == questionTypeId 
                    && q.Difficulty == difficulty);

            if (skillTypeId.HasValue)
            {
                query = query.Where(q => q.SkillTypeID == skillTypeId.Value);
            }

            return await query
                .Include(q => q.Skill)
                .Include(q => q.QuestionType)
                .ToListAsync();
        }

        public async Task<IEnumerable<ACAD_PlacementQuestion>> GetRandomQuestionsByCriteriaAsync(
            Guid questionTypeId, 
            int difficulty, 
            int count,
            Guid? skillTypeId = null)
        {
            var query = _context.ACAD_PlacementQuestions
                .Where(q => !q.IsDeleted 
                    && q.QuestionTypeID == questionTypeId 
                    && q.Difficulty == difficulty);

            if (skillTypeId.HasValue)
            {
                query = query.Where(q => q.SkillTypeID == skillTypeId.Value);
            }

            var allQuestions = await query
                .Include(q => q.Skill)
                .Include(q => q.QuestionType)
                .ToListAsync();

            // Random và lấy số lượng cần thiết
            var random = new Random();
            return allQuestions.OrderBy(x => random.Next()).Take(count);
        }
    }
}

