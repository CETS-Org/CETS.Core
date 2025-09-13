using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_Course.Responses;
using DTOs.ACAD.ACAD_Course.Search;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Infrastructure.Repositories.ACAD
{
    public class ACAD_CourseRepository : BaseRepository<ACAD_Course>, IACAD_CourseRepository
    {
        private readonly IMapper _mapper;

        public ACAD_CourseRepository(AppDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<ACAD_Course>> SearchAsync(string keyword)
        {
            return await _context.ACAD_Courses
                .Where(c => c.CourseName.Contains(keyword) || c.CourseCode.Contains(keyword))
                .ToListAsync();
        }

        public async Task<IEnumerable<ACAD_Course>> FilterAsync(Guid? levelId, Guid? formatId, Guid? teacherId)
        {
            var query = _context.ACAD_Courses.AsQueryable();

            if (levelId.HasValue)
                query = query.Where(c => c.CourseLevelID == levelId);

            if (formatId.HasValue)
                query = query.Where(c => c.CourseFormatID == formatId);

            if (teacherId.HasValue)
            {
                query = query.Where(c =>
                    _context.ACAD_CourseTeacherAssignments
                        .Any(a => a.CourseID == c.Id && a.TeacherID == teacherId));
            }

            return await query.ToListAsync();
        }

        public async Task<ACAD_Course?> GetDetailAsync(Guid courseId)
        {
            return await _context.ACAD_Courses
                .Include(c => c.Category)
                .Include(c => c.CourseLevel)
                .Include(c => c.CourseFormat)
                .Include(c => c.ACAD_CourseTeacherAssignments).ThenInclude(a => a.Teacher).ThenInclude(t => t.Account)
                .Include(c => c.ACAD_CourseTeacherAssignments).ThenInclude(a => a.Teacher).ThenInclude(t => t.COM_Feedbacks)
                .Include(c => c.ACAD_Syllabi).ThenInclude(s => s.ACAD_SyllabusItems)
                .Include(c => c.COM_Feedbacks)
                .Include(c => c.ACAD_Enrollments)
                .Include(c => c.ACAD_CourseBenefits).ThenInclude(b => b.Benefit)
                .Include(c => c.ACAD_CourseRequirements).ThenInclude(r => r.Requirement)
                .FirstOrDefaultAsync(c => c.Id == courseId);
        }


        public IQueryable<ACAD_Course> GetAllCoursesForListAsync()
        {
            return _context.ACAD_Courses
                .AsNoTracking()
                .Where(c => !c.IsDeleted && c.IsActive)
                .Include(c => c.Category)
                .Include(c => c.CourseLevel)
                .Include(c => c.ACAD_CourseTeacherAssignments).ThenInclude(a => a.Teacher).ThenInclude(t => t.Account)
                .Include(c => c.ACAD_CourseTeacherAssignments).ThenInclude(a => a.Teacher).ThenInclude(t => t.COM_Feedbacks)
                .Include(c => c.ACAD_Syllabi).ThenInclude(s => s.ACAD_SyllabusItems)
                .Include(c => c.COM_Feedbacks)
                .Include(c => c.ACAD_Enrollments);
        }

        public async Task<CourseSearchResult> SearchBasicAsync(CourseSearchQuery q, CancellationToken ct)
        {
            var baseQ = _context.Set<ACAD_Course>()
                .Where(c => c.IsActive && !c.IsDeleted)
                .Include(c => c.Category)
                .Include(c => c.CourseLevel)
                .Include(c => c.CourseFormat)
                .Include(c => c.ACAD_Enrollments)
                .Include(c => c.ACAD_CourseTeacherAssignments).ThenInclude(a => a.Teacher).ThenInclude(t => t.Account)
                .Include(c => c.ACAD_CourseTeacherAssignments).ThenInclude(a => a.Teacher).ThenInclude(t => t.COM_Feedbacks)
                .Include(c => c.ACAD_Syllabi).ThenInclude(s => s.ACAD_SyllabusItems)
                .Include(c => c.COM_Feedbacks)
                .Include(c => c.ACAD_CourseBenefits).ThenInclude(b => b.Benefit)
                .Include(c => c.ACAD_CourseSkills).ThenInclude(cs => cs.Skill)   // <-- mới thêm
                .AsQueryable();

            // Search keyword
            if (!string.IsNullOrWhiteSpace(q.Q))
            {
                var keyword = q.Q.Trim();
                baseQ = baseQ.Where(c =>
                    EF.Functions.Like(c.CourseName, $"%{keyword}%") ||
                    EF.Functions.Like(c.Description!, $"%{keyword}%") ||
                    c.ACAD_CourseSkills.Any(cs => EF.Functions.Like(cs.Skill.Name, $"%{keyword}%"))  // <-- tìm theo skill
                );
            }

            // Filters
            if (q.LevelIds.Count > 0) baseQ = baseQ.Where(c => q.LevelIds.Contains(c.CourseLevelID));
            if (q.CategoryIds.Count > 0) baseQ = baseQ.Where(c => q.CategoryIds.Contains(c.CategoryID));
            if (q.SkillIds.Count > 0) baseQ = baseQ.Where(c => c.ACAD_CourseSkills.Any(cs => q.SkillIds.Contains(cs.SkillID))); // <-- filter skill
            if (q.PriceMin.HasValue) baseQ = baseQ.Where(c => c.StandardPrice >= q.PriceMin.Value);
            if (q.PriceMax.HasValue) baseQ = baseQ.Where(c => c.StandardPrice <= q.PriceMax.Value);

            // Sorting
            baseQ = q.Sort switch
            {
                "Created.desc" => baseQ.OrderByDescending(c => c.CreatedAt),
                "Price.asc" => baseQ.OrderBy(c => c.StandardPrice),
                "Price.desc" => baseQ.OrderByDescending(c => c.StandardPrice),
                _ => baseQ.OrderByDescending(c => c.COM_Feedbacks.Average(f => (double?)f.Rating) ?? 0)
                          .ThenByDescending(c => c.ACAD_Enrollments.Count())
            };

            // Paging
            var total = await baseQ.CountAsync(ct);
            var entities = await baseQ
                .Skip((q.Page - 1) * q.PageSize)
                .Take(q.PageSize)
                .AsNoTracking()
                .ToListAsync(ct);

            var result = new CourseSearchResult
            {
                Page = q.Page,
                PageSize = q.PageSize,
                Total = total,
                Items = _mapper.Map<List<CourseListItemResponse>>(entities),
                Facets = new Dictionary<string, List<CourseSearchResult.FacetItem>>()
            };

            // ========= Facet Levels =========
            var levelCounts = await _context.Set<ACAD_Course>()
                .Where(c => c.IsActive && !c.IsDeleted)
                .Where(c =>
                    (string.IsNullOrWhiteSpace(q.Q) ||
                        EF.Functions.Like(c.CourseName, $"%{q.Q}%") ||
                        EF.Functions.Like(c.Description!, $"%{q.Q}%")) &&
                    (q.CategoryIds.Count == 0 || q.CategoryIds.Contains(c.CategoryID)) &&
                    (q.SkillIds.Count == 0 || c.ACAD_CourseSkills.Any(cs => q.SkillIds.Contains(cs.SkillID))) &&
                    (!q.PriceMin.HasValue || c.StandardPrice >= q.PriceMin.Value) &&
                    (!q.PriceMax.HasValue || c.StandardPrice <= q.PriceMax.Value)
                )
                .GroupBy(c => c.CourseLevelID)
                .Select(g => new { Id = g.Key, Count = g.Count() })
                .ToListAsync(ct);

            var levelIds = levelCounts.Select(x => x.Id).ToList();
            var levelLabels = await _context.Set<CORE_LookUp>()
                .Where(l => levelIds.Contains(l.Id))
                .Select(l => new { l.Id, l.Name })
                .ToListAsync(ct);

            result.Facets["levels"] = levelCounts
                .Select(x => new CourseSearchResult.FacetItem
                {
                    Key = x.Id.ToString(),
                    Label = levelLabels.FirstOrDefault(l => l.Id == x.Id)?.Name,
                    Count = x.Count,
                    Selected = q.LevelIds.Contains(x.Id)
                })
                .OrderByDescending(f => f.Count)
                .ToList();

            // ========= Facet Categories =========
            var categoryCounts = await _context.Set<ACAD_Course>()
                .Where(c => c.IsActive && !c.IsDeleted)
                .Where(c =>
                    (string.IsNullOrWhiteSpace(q.Q) ||
                        EF.Functions.Like(c.CourseName, $"%{q.Q}%") ||
                        EF.Functions.Like(c.Description!, $"%{q.Q}%")) &&
                    (q.LevelIds.Count == 0 || q.LevelIds.Contains(c.CourseLevelID)) &&
                    (q.SkillIds.Count == 0 || c.ACAD_CourseSkills.Any(cs => q.SkillIds.Contains(cs.SkillID))) &&
                    (!q.PriceMin.HasValue || c.StandardPrice >= q.PriceMin.Value) &&
                    (!q.PriceMax.HasValue || c.StandardPrice <= q.PriceMax.Value)
                )
                .GroupBy(c => c.CategoryID)
                .Select(g => new { Id = g.Key, Count = g.Count() })
                .ToListAsync(ct);

            var catIds = categoryCounts.Select(x => x.Id).ToList();
            var catLabels = await _context.Set<ACAD_CourseCategory>()
                .Where(cat => catIds.Contains(cat.Id))
                .Select(cat => new { cat.Id, cat.Name })
                .ToListAsync(ct);

            result.Facets["categories"] = categoryCounts
                .Select(x => new CourseSearchResult.FacetItem
                {
                    Key = x.Id.ToString(),
                    Label = catLabels.FirstOrDefault(c => c.Id == x.Id)?.Name,
                    Count = x.Count,
                    Selected = q.CategoryIds.Contains(x.Id)
                })
                .OrderByDescending(f => f.Count)
                .ToList();

            // ========= Facet Skills =========
            var skillCounts = await _context.Set<ACAD_CourseSkill>()
                .Where(cs => cs.Course.IsActive && !cs.Course.IsDeleted)
                .Where(cs =>
                    (string.IsNullOrWhiteSpace(q.Q) ||
                        EF.Functions.Like(cs.Course.CourseName, $"%{q.Q}%") ||
                        EF.Functions.Like(cs.Course.Description!, $"%{q.Q}%") ||
                        EF.Functions.Like(cs.Skill.Name, $"%{q.Q}%")) &&
                    (q.LevelIds.Count == 0 || q.LevelIds.Contains(cs.Course.CourseLevelID)) &&
                    (q.CategoryIds.Count == 0 || q.CategoryIds.Contains(cs.Course.CategoryID)) &&
                    (!q.PriceMin.HasValue || cs.Course.StandardPrice >= q.PriceMin.Value) &&
                    (!q.PriceMax.HasValue || cs.Course.StandardPrice <= q.PriceMax.Value)
                )
                .GroupBy(cs => cs.SkillID)
                .Select(g => new { Id = g.Key, Count = g.Count() })
                .ToListAsync(ct);

            var skillIds = skillCounts.Select(x => x.Id).ToList();
            var skillLabels = await _context.Set<CORE_LookUp>() // giả sử Skill lưu trong LookUp
                .Where(s => skillIds.Contains(s.Id))
                .Select(s => new { s.Id, s.Name })
                .ToListAsync(ct);

            result.Facets["skills"] = skillCounts
                .Select(x => new CourseSearchResult.FacetItem
                {
                    Key = x.Id.ToString(),
                    Label = skillLabels.FirstOrDefault(s => s.Id == x.Id)?.Name,
                    Count = x.Count,
                    Selected = q.SkillIds.Contains(x.Id)
                })
                .OrderByDescending(f => f.Count)
                .ToList();

            return result;
        }




    }
}
