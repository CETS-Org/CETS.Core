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

        private enum FacetDimension
        {
            Level,
            Category,
            Skill,
            DayOfWeek,
            TimeSlot
        }


        public async Task<IEnumerable<ACAD_CoursePackage>> GetActivePackagesAsync()
        {
            return await _context.ACAD_CoursePackages
                .Where(p => p.IsActive)
                .Include(p => p.ACAD_CoursePackageItems)
                    .ThenInclude(i => i.Course)
                        .ThenInclude(c => c.ACAD_Syllabi)
                            .ThenInclude(s => s.ACAD_SyllabusItems)
                .Include(p => p.ACAD_CoursePackageItems)
                    .ThenInclude(i => i.Course)
                        .ThenInclude(c => c.Category)
                .Include(p => p.ACAD_CoursePackageItems)
                    .ThenInclude(i => i.Course)
                        .ThenInclude(c => c.CourseLevel)
                .Include(p => p.ACAD_CoursePackageItems)
                    .ThenInclude(i => i.Course)
                        .ThenInclude(c => c.ACAD_Enrollments)
                .ToListAsync();
        }

        public async Task<ACAD_CoursePackage?> GetDetailAsync(Guid packageId)
        {
            return await _context.ACAD_CoursePackages
                .Include(p => p.ACAD_CoursePackageItems)
                    .ThenInclude(i => i.Course)
                        .ThenInclude(c => c.ACAD_Syllabi)
                            .ThenInclude(s => s.ACAD_SyllabusItems)
                .Include(p => p.ACAD_CoursePackageItems)
                    .ThenInclude(i => i.Course)
                        .ThenInclude(c => c.Category)
                .Include(p => p.ACAD_CoursePackageItems)
                    .ThenInclude(i => i.Course)
                        .ThenInclude(c => c.CourseLevel)
                .Include(p => p.ACAD_CoursePackageItems)
                    .ThenInclude(i => i.Course)
                        .ThenInclude(c => c.ACAD_Enrollments)
                .FirstOrDefaultAsync(p => p.Id == packageId && p.IsActive);
        }

        public async Task<CoursePackageSearchResult> SearchBasicAsync(CoursePackageSearchQuery q, CancellationToken ct)
        {
            var baseQ = BuildBaseQuery();
            baseQ = ApplySearchKeyword(baseQ, q);
            baseQ = ApplyFilters(baseQ, q);
            baseQ = ApplySorting(baseQ, q);

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

            // Facets
            result.Facets["levels"] = await BuildLevelFacetsAsync(q, ct);
            result.Facets["categories"] = await BuildCategoryFacetsAsync(q, ct);
            result.Facets["skills"] = await BuildSkillFacetsAsync(q, ct);
            result.Facets["daysOfWeek"] = await BuildDayOfWeekFacetsAsync(q, ct);
            result.Facets["timeSlots"] = await BuildTimeSlotFacetsAsync(q, ct);

            return result;
        }

  
        private IQueryable<ACAD_CoursePackage> GetFacetBaseQuery(CoursePackageSearchQuery q, FacetDimension facet)
        {
            var query = _context.Set<ACAD_CoursePackage>()
                .Where(p => !p.IsDeleted);

            // Text search
            if (!string.IsNullOrWhiteSpace(q.Q))
            {
                var keyword = q.Q.Trim();
                query = query.Where(p =>
                    EF.Functions.Like(p.Name, $"%{keyword}%") ||
                    EF.Functions.Like(p.Description!, $"%{keyword}%") ||
                    p.ACAD_CoursePackageItems.Any(i => !i.IsDeleted &&
                        (EF.Functions.Like(i.Course.CourseName, $"%{keyword}%") ||
                         i.Course.ACAD_CourseSkills.Any(cs => EF.Functions.Like(cs.Skill.Name, $"%{keyword}%"))))
                );
            }

            // IsActive
            if (q.IsActive.HasValue)
                query = query.Where(p => p.IsActive == q.IsActive.Value);

            if (facet != FacetDimension.Level && q.LevelIds.Count > 0)
                query = query.Where(p => p.ACAD_CoursePackageItems.Any(i => !i.IsDeleted && q.LevelIds.Contains(i.Course.CourseLevelID)));

            if (facet != FacetDimension.Category && q.CategoryIds.Count > 0)
                query = query.Where(p => p.ACAD_CoursePackageItems.Any(i => !i.IsDeleted && q.CategoryIds.Contains(i.Course.CategoryID)));

            if (facet != FacetDimension.Skill && q.SkillIds.Count > 0)
                query = query.Where(p => p.ACAD_CoursePackageItems.Any(i => !i.IsDeleted &&
                    i.Course.ACAD_CourseSkills.Any(cs => q.SkillIds.Contains(cs.SkillID))));


            // Price and package-size filters always apply
            if (q.PriceMin.HasValue)
                query = query.Where(p => p.TotalPrice >= q.PriceMin.Value);
            if (q.PriceMax.HasValue)
                query = query.Where(p => p.TotalPrice <= q.PriceMax.Value);
            if (q.MinCourseCount.HasValue)
                query = query.Where(p => p.ACAD_CoursePackageItems.Count(i => !i.IsDeleted) >= q.MinCourseCount.Value);
            if (q.MaxCourseCount.HasValue)
                query = query.Where(p => p.ACAD_CoursePackageItems.Count(i => !i.IsDeleted) <= q.MaxCourseCount.Value);

            return query;
        }

        private IQueryable<ACAD_CoursePackage> BuildBaseQuery()
        {
            return _context.Set<ACAD_CoursePackage>()
                .Where(p => !p.IsDeleted)
                .AsSplitQuery()
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
        }

        private static IQueryable<ACAD_CoursePackage> ApplySearchKeyword(IQueryable<ACAD_CoursePackage> query, CoursePackageSearchQuery q)
        {
            if (string.IsNullOrWhiteSpace(q.Q)) return query;

            var keyword = q.Q.Trim();
            return query.Where(p =>
                EF.Functions.Like(p.Name, $"%{keyword}%") ||
                EF.Functions.Like(p.Description!, $"%{keyword}%") ||
                p.ACAD_CoursePackageItems.Any(i => !i.IsDeleted &&
                    (EF.Functions.Like(i.Course.CourseName, $"%{keyword}%") ||
                     i.Course.ACAD_CourseSkills.Any(cs => EF.Functions.Like(cs.Skill.Name, $"%{keyword}%"))))
            );
        }

        private static IQueryable<ACAD_CoursePackage> ApplyFilters(IQueryable<ACAD_CoursePackage> query, CoursePackageSearchQuery q)
        {
            if (q.IsActive.HasValue)
                query = query.Where(p => p.IsActive == q.IsActive.Value);

            if (q.LevelIds.Count > 0)
                query = query.Where(p => p.ACAD_CoursePackageItems.Any(i => !i.IsDeleted && q.LevelIds.Contains(i.Course.CourseLevelID)));

            if (q.CategoryIds.Count > 0)
                query = query.Where(p => p.ACAD_CoursePackageItems.Any(i => !i.IsDeleted && q.CategoryIds.Contains(i.Course.CategoryID)));

            if (q.SkillIds.Count > 0)
                query = query.Where(p => p.ACAD_CoursePackageItems.Any(i => !i.IsDeleted &&
                    i.Course.ACAD_CourseSkills.Any(cs => q.SkillIds.Contains(cs.SkillID))));

            if (q.DaysOfWeek.Count > 0)
                query = query.Where(p => p.ACAD_CoursePackageItems.Any(i => !i.IsDeleted &&
                    i.Course.ACAD_CourseSchedules.Any(s => q.DaysOfWeek.Contains(s.DayOfWeek))));

            if (q.TimeSlotIds.Count > 0)
                query = query.Where(p => p.ACAD_CoursePackageItems.Any(i => !i.IsDeleted &&
                    i.Course.ACAD_CourseSchedules.Any(s => q.TimeSlotIds.Contains(s.TimeSlotID))));

            if (q.TimeSlotNames.Count > 0)
                query = query.Where(p => p.ACAD_CoursePackageItems.Any(i => !i.IsDeleted &&
                    i.Course.ACAD_CourseSchedules.Any(s => q.TimeSlotNames.Contains(s.TimeSlot.Name))));

            if (q.PriceMin.HasValue)
                query = query.Where(p => p.TotalPrice >= q.PriceMin.Value);

            if (q.PriceMax.HasValue)
                query = query.Where(p => p.TotalPrice <= q.PriceMax.Value);

            if (q.MinCourseCount.HasValue)
                query = query.Where(p => p.ACAD_CoursePackageItems.Count(i => !i.IsDeleted) >= q.MinCourseCount.Value);

            if (q.MaxCourseCount.HasValue)
                query = query.Where(p => p.ACAD_CoursePackageItems.Count(i => !i.IsDeleted) <= q.MaxCourseCount.Value);

            return query;
        }

        private static IQueryable<ACAD_CoursePackage> ApplySorting(IQueryable<ACAD_CoursePackage> query, CoursePackageSearchQuery q)
        {
            return q.Sort switch
            {
                "Created.desc" => query.OrderByDescending(p => p.CreatedAt),
                "Price.asc" => query.OrderBy(p => p.TotalPrice),
                "Price.desc" => query.OrderByDescending(p => p.TotalPrice),
                _ => query.OrderByDescending(p => p.IsActive)
                          .ThenByDescending(p => p.CreatedAt)
            };
        }

        private async Task<List<CoursePackageSearchResult.CoursePackageFacetItem>> BuildLevelFacetsAsync(CoursePackageSearchQuery q, CancellationToken ct)
        {
            var facetBase = GetFacetBaseQuery(q, FacetDimension.Level);
            var levelCounts = await facetBase
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

            return levelCounts
                .Select(x => new CoursePackageSearchResult.CoursePackageFacetItem
                {
                    Key = x.Id.ToString(),
                    Label = levelLabels.FirstOrDefault(l => l.Id == x.Id)?.Name,
                    Count = x.Count,
                    Selected = q.LevelIds.Contains(x.Id)
                })
                .OrderByDescending(f => f.Count)
                .ToList();
        }

        private async Task<List<CoursePackageSearchResult.CoursePackageFacetItem>> BuildCategoryFacetsAsync(CoursePackageSearchQuery q, CancellationToken ct)
        {
            var facetBase = GetFacetBaseQuery(q, FacetDimension.Category);
            var categoryCounts = await facetBase
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

            return categoryCounts
                .Select(x => new CoursePackageSearchResult.CoursePackageFacetItem
                {
                    Key = x.Id.ToString(),
                    Label = categoryLabels.FirstOrDefault(l => l.Id == x.Id)?.Name,
                    Count = x.Count,
                    Selected = q.CategoryIds.Contains(x.Id)
                })
                .OrderByDescending(f => f.Count)
                .ToList();
        }

        private async Task<List<CoursePackageSearchResult.CoursePackageFacetItem>> BuildSkillFacetsAsync(CoursePackageSearchQuery q, CancellationToken ct)
        {
            var facetBase = GetFacetBaseQuery(q, FacetDimension.Skill);
            var skillCounts = await facetBase
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

            return skillCounts
                .Select(x => new CoursePackageSearchResult.CoursePackageFacetItem
                {
                    Key = x.Id.ToString(),
                    Label = skillLabels.FirstOrDefault(s => s.Id == x.Id)?.Name,
                    Count = x.Count,
                    Selected = q.SkillIds.Contains(x.Id)
                })
                .OrderByDescending(f => f.Count)
                .ToList();
        }

        private async Task<List<CoursePackageSearchResult.CoursePackageFacetItem>> BuildDayOfWeekFacetsAsync(CoursePackageSearchQuery q, CancellationToken ct)
        {
            var facetBase = GetFacetBaseQuery(q, FacetDimension.DayOfWeek);
            var dayCounts = await facetBase
                .SelectMany(p => p.ACAD_CoursePackageItems.Where(i => !i.IsDeleted)
                    .SelectMany(i => i.Course.ACAD_CourseSchedules.Select(s => s.DayOfWeek)))
                .Where(day => !string.IsNullOrEmpty(day))
                .GroupBy(day => day)
                .Select(g => new { Day = g.Key, Count = g.Count() })
                .ToListAsync(ct);

            return dayCounts
                .Select(x => new CoursePackageSearchResult.CoursePackageFacetItem
                {
                    Key = x.Day,
                    Label = x.Day,
                    Count = x.Count,
                    Selected = q.DaysOfWeek.Contains(x.Day)
                })
                .OrderBy(f => Array.IndexOf(new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" }, f.Key))
                .ToList();
        }

        private async Task<List<CoursePackageSearchResult.CoursePackageFacetItem>> BuildTimeSlotFacetsAsync(CoursePackageSearchQuery q, CancellationToken ct)
        {
            var facetBase = GetFacetBaseQuery(q, FacetDimension.TimeSlot);
            var timeSlotCounts = await facetBase
                .SelectMany(p => p.ACAD_CoursePackageItems.Where(i => !i.IsDeleted)
                    .SelectMany(i => i.Course.ACAD_CourseSchedules
                        .Where(s => s.TimeSlot != null)
                        .Select(s => new { s.TimeSlot.Id, s.TimeSlot.Name })))
                .GroupBy(ts => new { ts.Id, ts.Name })
                .Select(g => new { g.Key.Id, g.Key.Name, Count = g.Count() })
                .ToListAsync(ct);

            return timeSlotCounts
                .Select(x => new CoursePackageSearchResult.CoursePackageFacetItem
                {
                    Key = x.Name,
                    Label = x.Name,
                    Count = x.Count,
                    Selected = q.TimeSlotNames.Contains(x.Name)
                })
                .OrderByDescending(f => f.Count)
                .ToList();
        }
    }
}


