using AutoMapper;
using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_CoursePackage.Responses;
using DTOs.ACAD.ACAD_CoursePackage.Search;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.ACAD
{
    public class ACAD_CoursePackageRepository : BaseRepository<ACAD_CoursePackage>, IACAD_CoursePackageRepository
    {
        private readonly IMapper _mapper;

        public ACAD_CoursePackageRepository(AppDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }
            public async Task<IEnumerable<ACAD_CoursePackage>> GetActivePackagesAsync()
        {
            return await _context.ACAD_CoursePackages
                .Where(p => p.IsActive)
                .Include(p => p.ACAD_CoursePackageItems)
                    .ThenInclude(i => i.Course)
                .ToListAsync();
        }

        public async Task<ACAD_CoursePackage?> GetDetailAsync(Guid packageId)
        {
            return await _context.ACAD_CoursePackages
                .Include(p => p.ACAD_CoursePackageItems)
                    .ThenInclude(i => i.Course)
                .FirstOrDefaultAsync(p => p.Id == packageId && p.IsActive);
        }

        public async Task<CoursePackageSearchResult> SearchBasicAsync(CoursePackageSearchQuery q, CancellationToken ct)
        {
            var baseQ = _context.Set<ACAD_CoursePackage>()
                .Where(p => !p.IsDeleted)
                .Include(p => p.ACAD_CoursePackageItems.Where(i => !i.IsDeleted))
                    .ThenInclude(i => i.Course)
                        .ThenInclude(c => c.Category)
                .Include(p => p.ACAD_CoursePackageItems.Where(i => !i.IsDeleted))
                    .ThenInclude(i => i.Course)
                        .ThenInclude(c => c.CourseLevel)
                .Include(p => p.ACAD_CoursePackageItems.Where(i => !i.IsDeleted))
                    .ThenInclude(i => i.Course)
                        .ThenInclude(c => c.ACAD_CourseSkills)
                            .ThenInclude(cs => cs.Skill)
                .Include(p => p.ACAD_CoursePackageItems.Where(i => !i.IsDeleted))
                    .ThenInclude(i => i.Course)
                        .ThenInclude(c => c.ACAD_CourseSchedules)
                .AsQueryable();

            // Search keyword
            if (!string.IsNullOrWhiteSpace(q.Q))
            {
                var keyword = q.Q.Trim();
                baseQ = baseQ.Where(p =>
                    EF.Functions.Like(p.Name, $"%{keyword}%") ||
                    EF.Functions.Like(p.Description!, $"%{keyword}%") ||
                    p.ACAD_CoursePackageItems.Any(i => !i.IsDeleted && 
                        (EF.Functions.Like(i.Course.CourseName, $"%{keyword}%") ||
                         i.Course.ACAD_CourseSkills.Any(cs => EF.Functions.Like(cs.Skill.Name, $"%{keyword}%"))))
                );
            }

            // Filters
            if (q.IsActive.HasValue) 
                baseQ = baseQ.Where(p => p.IsActive == q.IsActive.Value);

            if (q.LevelIds.Count > 0) 
                baseQ = baseQ.Where(p => p.ACAD_CoursePackageItems.Any(i => !i.IsDeleted && q.LevelIds.Contains(i.Course.CourseLevelID)));

            if (q.CategoryIds.Count > 0) 
                baseQ = baseQ.Where(p => p.ACAD_CoursePackageItems.Any(i => !i.IsDeleted && q.CategoryIds.Contains(i.Course.CategoryID)));

            if (q.SkillIds.Count > 0) 
                baseQ = baseQ.Where(p => p.ACAD_CoursePackageItems.Any(i => !i.IsDeleted && 
                    i.Course.ACAD_CourseSkills.Any(cs => q.SkillIds.Contains(cs.SkillID))));

            // Schedule filters
            if (q.DaysOfWeek.Count > 0)
                baseQ = baseQ.Where(p => p.ACAD_CoursePackageItems.Any(i => !i.IsDeleted &&
                    i.Course.ACAD_CourseSchedules.Any(s => q.DaysOfWeek.Contains(s.DayOfWeek))));

            if (q.TimeSlotIds.Count > 0)
                baseQ = baseQ.Where(p => p.ACAD_CoursePackageItems.Any(i => !i.IsDeleted &&
                    i.Course.ACAD_CourseSchedules.Any(s => q.TimeSlotIds.Contains(s.TimeSlotID))));
            if (q.TimeSlotNames.Count > 0)
                baseQ = baseQ.Where(p => p.ACAD_CoursePackageItems.Any(i => !i.IsDeleted &&
                    i.Course.ACAD_CourseSchedules.Any(s => q.TimeSlotNames.Contains(s.TimeSlot.Name))));

            if (q.PriceMin.HasValue) 
                baseQ = baseQ.Where(p => p.TotalPrice >= q.PriceMin.Value);

            if (q.PriceMax.HasValue) 
                baseQ = baseQ.Where(p => p.TotalPrice <= q.PriceMax.Value);

            if (q.MinCourseCount.HasValue)
                baseQ = baseQ.Where(p => p.ACAD_CoursePackageItems.Count(i => !i.IsDeleted) >= q.MinCourseCount.Value);

            if (q.MaxCourseCount.HasValue)
                baseQ = baseQ.Where(p => p.ACAD_CoursePackageItems.Count(i => !i.IsDeleted) <= q.MaxCourseCount.Value);

            // Sorting
            baseQ = q.Sort switch
            {
                "Created.desc" => baseQ.OrderByDescending(p => p.CreatedAt),
                "Price.asc" => baseQ.OrderBy(p => p.TotalPrice),
                "Price.desc" => baseQ.OrderByDescending(p => p.TotalPrice),
                _ => baseQ.OrderByDescending(p => p.IsActive)
                          .ThenByDescending(p => p.CreatedAt)
            };

            // Paging
            var total = await baseQ.CountAsync(ct);
            var entities = await baseQ
                .Skip((q.Page - 1) * q.PageSize)
                .Take(q.PageSize)
                .AsNoTracking()
                .ToListAsync(ct);

            var result = new CoursePackageSearchResult
            {
                Page = q.Page,
                PageSize = q.PageSize,
                Total = total,
                Items = _mapper.Map<List<CoursePackageResponse>>(entities),
                Facets = new Dictionary<string, List<CoursePackageSearchResult.CoursePackageFacetItem>>()
            };

            // ========= Facet Levels =========
            var levelCounts = await _context.Set<ACAD_CoursePackage>()
                .Where(p => !p.IsDeleted)
                .Where(p =>
                    (string.IsNullOrWhiteSpace(q.Q) ||
                        EF.Functions.Like(p.Name, $"%{q.Q}%") ||
                        EF.Functions.Like(p.Description!, $"%{q.Q}%") ||
                        p.ACAD_CoursePackageItems.Any(i => !i.IsDeleted && 
                            (EF.Functions.Like(i.Course.CourseName, $"%{q.Q}%") ||
                             i.Course.ACAD_CourseSkills.Any(cs => EF.Functions.Like(cs.Skill.Name, $"%{q.Q}%"))))) &&
                    (!q.IsActive.HasValue || p.IsActive == q.IsActive.Value) &&
                    (q.CategoryIds.Count == 0 || p.ACAD_CoursePackageItems.Any(i => !i.IsDeleted && q.CategoryIds.Contains(i.Course.CategoryID))) &&
                    (q.SkillIds.Count == 0 || p.ACAD_CoursePackageItems.Any(i => !i.IsDeleted && 
                        i.Course.ACAD_CourseSkills.Any(cs => q.SkillIds.Contains(cs.SkillID)))) &&
                    (!q.PriceMin.HasValue || p.TotalPrice >= q.PriceMin.Value) &&
                    (!q.PriceMax.HasValue || p.TotalPrice <= q.PriceMax.Value) &&
                    (!q.MinCourseCount.HasValue || p.ACAD_CoursePackageItems.Count(i => !i.IsDeleted) >= q.MinCourseCount.Value) &&
                    (!q.MaxCourseCount.HasValue || p.ACAD_CoursePackageItems.Count(i => !i.IsDeleted) <= q.MaxCourseCount.Value)
                )
                .SelectMany(p => p.ACAD_CoursePackageItems.Where(i => !i.IsDeleted && 
                    (q.CategoryIds.Count == 0 || q.CategoryIds.Contains(i.Course.CategoryID)))
                    .Select(i => i.Course.CourseLevelID))
                .GroupBy(levelId => levelId)
                .Select(g => new { Id = g.Key, Count = g.Count() })
                .ToListAsync(ct);

            var levelIds = levelCounts.Select(x => x.Id).ToList();
            var levelLabels = await _context.Set<CORE_LookUp>()
                .Where(l => levelIds.Contains(l.Id))
                .Select(l => new { l.Id, l.Name })
                .ToListAsync(ct);

            result.Facets["levels"] = levelCounts
                .Select(x => new CoursePackageSearchResult.CoursePackageFacetItem
                {
                    Key = x.Id.ToString(),
                    Label = levelLabels.FirstOrDefault(l => l.Id == x.Id)?.Name,
                    Count = x.Count,
                    Selected = q.LevelIds.Contains(x.Id)
                })
                .OrderByDescending(f => f.Count)
                .ToList();

            // ========= Facet Categories =========
            var categoryCounts = await _context.Set<ACAD_CoursePackage>()
                .Where(p => !p.IsDeleted)
                .Where(p =>
                    (string.IsNullOrWhiteSpace(q.Q) ||
                        EF.Functions.Like(p.Name, $"%{q.Q}%") ||
                        EF.Functions.Like(p.Description!, $"%{q.Q}%") ||
                        p.ACAD_CoursePackageItems.Any(i => !i.IsDeleted && 
                            (EF.Functions.Like(i.Course.CourseName, $"%{q.Q}%") ||
                             i.Course.ACAD_CourseSkills.Any(cs => EF.Functions.Like(cs.Skill.Name, $"%{q.Q}%"))))) &&
                    (!q.IsActive.HasValue || p.IsActive == q.IsActive.Value) &&
                    (q.LevelIds.Count == 0 || p.ACAD_CoursePackageItems.Any(i => !i.IsDeleted && q.LevelIds.Contains(i.Course.CourseLevelID))) &&
                    (q.SkillIds.Count == 0 || p.ACAD_CoursePackageItems.Any(i => !i.IsDeleted && 
                        i.Course.ACAD_CourseSkills.Any(cs => q.SkillIds.Contains(cs.SkillID)))) &&
                    (!q.PriceMin.HasValue || p.TotalPrice >= q.PriceMin.Value) &&
                    (!q.PriceMax.HasValue || p.TotalPrice <= q.PriceMax.Value) &&
                    (!q.MinCourseCount.HasValue || p.ACAD_CoursePackageItems.Count(i => !i.IsDeleted) >= q.MinCourseCount.Value) &&
                    (!q.MaxCourseCount.HasValue || p.ACAD_CoursePackageItems.Count(i => !i.IsDeleted) <= q.MaxCourseCount.Value)
                    // NOTE: Intentionally excluding CategoryIds filter from category facets
                )
                .SelectMany(p => p.ACAD_CoursePackageItems.Where(i => !i.IsDeleted)
                    .Select(i => i.Course.CategoryID))
                .GroupBy(categoryId => categoryId)
                .Select(g => new { Id = g.Key, Count = g.Count() })
                .ToListAsync(ct);

            var categoryIds = categoryCounts.Select(x => x.Id).ToList();
            var categoryLabels = await _context.Set<ACAD_CourseCategory>()
                .Where(l => categoryIds.Contains(l.Id))
                .Select(l => new { l.Id, l.Name })
                .ToListAsync(ct);

            result.Facets["categories"] = categoryCounts
                .Select(x => new CoursePackageSearchResult.CoursePackageFacetItem
                {
                    Key = x.Id.ToString(),
                    Label = categoryLabels.FirstOrDefault(l => l.Id == x.Id)?.Name,
                    Count = x.Count,
                    Selected = q.CategoryIds.Contains(x.Id)
                })
                .OrderByDescending(f => f.Count)
                .ToList();

            // ========= Facet Skills =========
            var skillCounts = await _context.Set<ACAD_CoursePackage>()
                .Where(p => !p.IsDeleted)
                .Where(p =>
                    (string.IsNullOrWhiteSpace(q.Q) ||
                        EF.Functions.Like(p.Name, $"%{q.Q}%") ||
                        EF.Functions.Like(p.Description!, $"%{q.Q}%") ||
                        p.ACAD_CoursePackageItems.Any(i => !i.IsDeleted &&
                            (EF.Functions.Like(i.Course.CourseName, $"%{q.Q}%") ||
                             i.Course.ACAD_CourseSkills.Any(cs => EF.Functions.Like(cs.Skill.Name, $"%{q.Q}%"))))) &&
                    (!q.IsActive.HasValue || p.IsActive == q.IsActive.Value) &&
                    (q.LevelIds.Count == 0 || p.ACAD_CoursePackageItems.Any(i => !i.IsDeleted && q.LevelIds.Contains(i.Course.CourseLevelID))) &&
                    (q.CategoryIds.Count == 0 || p.ACAD_CoursePackageItems.Any(i => !i.IsDeleted && q.CategoryIds.Contains(i.Course.CategoryID))) &&
                    (!q.PriceMin.HasValue || p.TotalPrice >= q.PriceMin.Value) &&
                    (!q.PriceMax.HasValue || p.TotalPrice <= q.PriceMax.Value) &&
                    (!q.MinCourseCount.HasValue || p.ACAD_CoursePackageItems.Count(i => !i.IsDeleted) >= q.MinCourseCount.Value) &&
                    (!q.MaxCourseCount.HasValue || p.ACAD_CoursePackageItems.Count(i => !i.IsDeleted) <= q.MaxCourseCount.Value)
                )
                .SelectMany(p => p.ACAD_CoursePackageItems.Where(i => !i.IsDeleted)
                    .SelectMany(i => i.Course.ACAD_CourseSkills.Select(cs => cs.SkillID)))
                .GroupBy(skillId => skillId)
                .Select(g => new { Id = g.Key, Count = g.Count() })
                .ToListAsync(ct);

            var skillIds = skillCounts.Select(x => x.Id).ToList();
            var skillLabels = await _context.Set<CORE_LookUp>()
                .Where(s => skillIds.Contains(s.Id))
                .Select(s => new { s.Id, s.Name })
                .ToListAsync(ct);

            result.Facets["skills"] = skillCounts
                .Select(x => new CoursePackageSearchResult.CoursePackageFacetItem
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


