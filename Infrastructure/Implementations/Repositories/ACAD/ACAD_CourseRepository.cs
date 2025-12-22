using AutoMapper;
using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_Course.Responses;
using DTOs.ACAD.ACAD_Course.Search;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;
using static DTOs.ACAD.ACAD_Course.Search.CourseSearchResult;

namespace Infrastructure.Implementations.Repositories.ACAD
{
    public class ACAD_CourseRepository : BaseRepository<ACAD_Course>, IACAD_CourseRepository
    {
        private readonly IMapper _mapper;

        public ACAD_CourseRepository(AppDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        private enum FacetDimension
        {
            Level,
            Category,
            Skill,
            Requirement,
            Benefit,
            DayOfWeek,
            TimeSlot
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
            var enrolledStatusId = await _context.Set<CORE_LookUp>()
                .Where(s => s.LookUpType != null && s.LookUpType.Code == "EnrollmentStatus" && s.Code == "Enrolled")
                .Select(s => s.Id)
                .FirstOrDefaultAsync();

            var course = await _context.ACAD_Courses
                .AsNoTracking()
                .AsSplitQuery()
                .Include(c => c.Category)
                .Include(c => c.CourseLevel)
                .Include(c => c.CourseFormat)
                .Include(c => c.ACAD_Syllabi.Where(s => !s.IsDeleted))
                    .ThenInclude(s => s.ACAD_SyllabusItems.Where(i => !i.IsDeleted).OrderBy(i => i.SessionNumber))
                .Include(c => c.ACAD_Enrollments.Where(e => enrolledStatusId != Guid.Empty && e.EnrollmentStatusID == enrolledStatusId))
                    .ThenInclude(e => e.EnrollmentStatus)
                .Include(c => c.ACAD_CourseBenefits)
                    .ThenInclude(b => b.Benefit)
                .Include(c => c.ACAD_CourseRequirements)
                    .ThenInclude(r => r.Requirement)
                .Include(c => c.ACAD_CourseSkills)
                    .ThenInclude(s => s.Skill)
                .Include(c => c.ACAD_CourseSchedules)
                    .ThenInclude(cs => cs.TimeSlot)

                .Include(c => c.COM_Feedbacks.Where(f => f.TeacherID == null && !f.IsDeleted))
                    .ThenInclude(f => f.Submitter)
                        .ThenInclude(s => s.Account)
                .Include(c => c.COM_Feedbacks.Where(f => f.TeacherID == null && !f.IsDeleted))
                    .ThenInclude(f => f.FeedbackType)
                .Include(c => c.CreatedByNavigation)
                .Include(c => c.UpdatedByNavigation)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            return course;
        }

        public async Task<IEnumerable<ACAD_Course>> GetAllCourse()
        {
            return await _context.ACAD_Courses
                .Include(c => c.Category)
                .Include(c => c.ACAD_Enrollments)
                .Include(c => c.CreatedByNavigation)
                .Include(c => c.UpdatedByNavigation)
                .ToListAsync();
        }


        public IQueryable<ACAD_Course> GetAllCoursesForListAsync()
        {
            
            return _context.ACAD_Courses
                .AsNoTracking()
                .AsSplitQuery()
                .Where(c => !c.IsDeleted)
                .Include(c => c.Category)
                .Include(c => c.CourseLevel)
                .Include(c => c.ACAD_CourseTeacherAssignments)
                    .ThenInclude(a => a.Teacher)
                    .ThenInclude(t => t.Account)
                .Include(c => c.ACAD_Enrollments);
        }

        private IQueryable<ACAD_Course> CreateFacetBaseQuery()
        {
            return _context.ACAD_Courses
                .Where(c => c.IsActive && !c.IsDeleted);
        }

        
        private async Task<List<FacetItem>> BuildLevelFacetAsync(
    CourseSearchQuery q,
    CancellationToken ct)
        {
            var query = CreateFacetBaseQuery();
            query = ApplyFacetFilters(query, q, FacetDimension.Level);

            var counts = await query
                .GroupBy(c => c.CourseLevelID)
                .Select(g => new
                {
                    LevelId = g.Key,
                    Count = g.Count()
                })
                .ToListAsync(ct);

            var levelIds = counts.Select(x => x.LevelId).ToList();

            var labels = await _context.CORE_LookUps
                .Where(l => levelIds.Contains(l.Id))
                .ToDictionaryAsync(l => l.Id, l => l.Name, ct);
            return counts
                .Select(x => new FacetItem
                {
                    Key = x.LevelId.ToString(),
                    Label = labels.GetValueOrDefault(x.LevelId),
                    Count = x.Count,
                    Selected = q.LevelIds.Contains(x.LevelId)
                })
                .OrderByDescending(x => x.Count)
                .ToList();
        }

        private async Task BuildFacetsAsync(
            CourseSearchResult result,
            CourseSearchQuery q,
            CancellationToken ct)
        {
                    result.Facets["levels"] = await BuildLevelFacetAsync(q, ct);
        }

        public async Task<CourseSearchResult> SearchBasicAsync(CourseSearchQuery q, CancellationToken ct)
        {
            var baseQ = BuildBaseQuery();
            baseQ = ApplySearchKeyword(baseQ, q);
            baseQ = ApplyFilters(baseQ, q);
            baseQ = ApplySorting(baseQ, q);

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

            // Facets
            var levelCounts = await GetFacetBaseQuery(q, FacetDimension.Level)
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

            var categoryCounts = await GetFacetBaseQuery(q, FacetDimension.Category)
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

            var skillCounts = await GetFacetBaseQueryWithRelatedTextSearch(q, FacetDimension.Skill)
                .SelectMany(c => c.ACAD_CourseSkills)
                .GroupBy(cs => cs.SkillID)
                .Select(g => new { Id = g.Key, Count = g.Count() })
                .ToListAsync(ct);

            //var skillCounts = await GetFacetBaseQuery(q, FacetDimension.Skill)
            //    .SelectMany(c => c.ACAD_CourseSkills)
            //    .GroupBy(cs => cs.SkillID)
            //    .Select(g => new
            //    {
            //        Id = g.Key,
            //        Count = g.Select(x => x.CourseID).Distinct().Count()
            //    })
            //    .ToListAsync(ct);


            var skillIds = skillCounts.Select(x => x.Id).ToList();
            var skillLabels = await _context.Set<CORE_LookUp>()
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

            var requirementCounts = await GetFacetBaseQueryWithRelatedTextSearch(q, FacetDimension.Requirement)
                .SelectMany(c => c.ACAD_CourseRequirements)
                .GroupBy(cr => cr.RequirementID)
                .Select(g => new { Id = g.Key, Count = g.Count() })
                .ToListAsync(ct);

            var requirementIds = requirementCounts.Select(x => x.Id).ToList();
            var requirementLabels = await _context.Set<CORE_LookUp>()
                .Where(r => requirementIds.Contains(r.Id))
                .Select(r => new { r.Id, r.Name })
                .ToListAsync(ct);

            result.Facets["requirements"] = requirementCounts
                .Select(x => new CourseSearchResult.FacetItem
                {
                    Key = x.Id.ToString(),
                    Label = requirementLabels.FirstOrDefault(r => r.Id == x.Id)?.Name,
                    Count = x.Count,
                    Selected = q.RequirementIds.Contains(x.Id)
                })
                .OrderByDescending(f => f.Count)
                .ToList();

            var benefitCounts = await GetFacetBaseQueryWithRelatedTextSearch(q, FacetDimension.Benefit)
                .SelectMany(c => c.ACAD_CourseBenefits)
                .GroupBy(cb => cb.BenefitID)
                .Select(g => new { Id = g.Key, Count = g.Count() })
                .ToListAsync(ct);

            var benefitIds = benefitCounts.Select(x => x.Id).ToList();
            var benefitLabels = await _context.Set<CORE_LookUp>()
                .Where(b => benefitIds.Contains(b.Id))
                .Select(b => new { b.Id, b.Name })
                .ToListAsync(ct);

            result.Facets["benefits"] = benefitCounts
                .Select(x => new CourseSearchResult.FacetItem
                {
                    Key = x.Id.ToString(),
                    Label = benefitLabels.FirstOrDefault(b => b.Id == x.Id)?.Name,
                    Count = x.Count,
                    Selected = q.BenefitIds.Contains(x.Id)
                })
                .OrderByDescending(f => f.Count)
                .ToList();

            var dayCounts = await GetFacetBaseQuery(q, FacetDimension.DayOfWeek)
                .SelectMany(c => c.ACAD_CourseSchedules)
                .GroupBy(s => s.DayOfWeek)
                .Select(g => new { Day = g.Key, Count = g.Select(s => s.CourseID).Distinct().Count() })
                .ToListAsync(ct);

            var dayOrder = new[] { System.DayOfWeek.Monday, System.DayOfWeek.Tuesday, System.DayOfWeek.Wednesday, System.DayOfWeek.Thursday, System.DayOfWeek.Friday, System.DayOfWeek.Saturday, System.DayOfWeek.Sunday };

            result.Facets["daysOfWeek"] = dayCounts
                .OrderBy(x => Array.IndexOf(dayOrder, x.Day))
                .Select(x => new CourseSearchResult.FacetItem
                {
                    Key = x.Day.ToString(),
                    Label = x.Day.ToString(),
                    Count = x.Count,
                    Selected = q.DaysOfWeek.Contains(x.Day)
                })
                .ToList();

            var timeSlotCounts = await GetFacetBaseQuery(q, FacetDimension.TimeSlot)
                .SelectMany(c => c.ACAD_CourseSchedules)
                .Where(s => s.TimeSlot != null)
                .GroupBy(s => new { s.TimeSlot.Id, s.TimeSlot.Name })
                .Select(g => new { g.Key.Id, g.Key.Name, Count = g.Select(s => s.CourseID).Distinct().Count() })
                .ToListAsync(ct);

            result.Facets["timeSlots"] = timeSlotCounts
                .Select(x => new CourseSearchResult.FacetItem
                {
                    Key = x.Name,
                    Label = x.Name,
                    Count = x.Count,
                    Selected = q.TimeSlotNames.Contains(x.Name)
                })
                .OrderByDescending(f => f.Count)
                .ToList();

            return result;
        }

  
        private IQueryable<ACAD_Course> BuildBaseQuery()
        {
            return _context.Set<ACAD_Course>()
                .Where(c => c.IsActive && !c.IsDeleted)
                .AsSplitQuery()
                .Include(c => c.Category)
                .Include(c => c.CourseLevel)
                .Include(c => c.CourseFormat)
                .Include(c => c.ACAD_Enrollments)
                    .ThenInclude(e => e.EnrollmentStatus)
                .Include(c => c.ACAD_CourseTeacherAssignments).ThenInclude(a => a.Teacher).ThenInclude(t => t.Account)
                .Include(c => c.ACAD_Syllabi.Where(s => !s.IsDeleted))
                    .ThenInclude(s => s.ACAD_SyllabusItems.Where(i => !i.IsDeleted).OrderBy(i => i.SessionNumber))
                .Include(c => c.ACAD_CourseBenefits).ThenInclude(b => b.Benefit)
                .Include(c => c.ACAD_CourseRequirements).ThenInclude(r => r.Requirement)
                .Include(c => c.ACAD_CourseSkills).ThenInclude(cs => cs.Skill)
                .Include(c => c.ACAD_CourseSchedules).ThenInclude(s => s.TimeSlot)
                .AsQueryable();
        }

        private static IQueryable<ACAD_Course> ApplySearchKeyword(IQueryable<ACAD_Course> query, CourseSearchQuery q)
        {
            if (string.IsNullOrWhiteSpace(q.Q)) return query;
            var keyword = q.Q.Trim();
            return query.Where(c =>
                EF.Functions.Like(c.CourseName, $"%{keyword}%") ||
                EF.Functions.Like(c.Description!, $"%{keyword}%") ||
                c.ACAD_CourseSkills.Any(cs => EF.Functions.Like(cs.Skill.Name, $"%{keyword}%"))
            );
        }

        private static IQueryable<ACAD_Course> ApplyFilters(IQueryable<ACAD_Course> query, CourseSearchQuery q)
        {
            //if (q.LevelIds.Count > 0) query = query.Where(c => q.LevelIds.Contains(c.CourseLevelID));
            query = query.Where(c =>
                q.SkillIds.All(skillId =>
                    c.ACAD_CourseSkills.Any(cs => cs.SkillID == skillId)));

            if (q.CategoryIds.Count > 0) query = query.Where(c => q.CategoryIds.Contains(c.CategoryID));
            if (q.SkillIds.Count > 0) query = query.Where(c => c.ACAD_CourseSkills.Any(cs => q.SkillIds.Contains(cs.SkillID)));
            if (q.RequirementIds.Count > 0) query = query.Where(c => c.ACAD_CourseRequirements.Any(cr => q.RequirementIds.Contains(cr.RequirementID)));
            if (q.BenefitIds.Count > 0) query = query.Where(c => c.ACAD_CourseBenefits.Any(cb => q.BenefitIds.Contains(cb.BenefitID)));
            if (q.DaysOfWeek.Count > 0) query = query.Where(c => c.ACAD_CourseSchedules.Any(s => q.DaysOfWeek.Contains(s.DayOfWeek)));
            if (q.TimeSlotIds.Count > 0) query = query.Where(c => c.ACAD_CourseSchedules.Any(s => q.TimeSlotIds.Contains(s.TimeSlotID)));
            if (q.TimeSlotNames.Count > 0) query = query.Where(c => c.ACAD_CourseSchedules.Any(s => q.TimeSlotNames.Contains(s.TimeSlot.Name)));
            if (q.PriceMin.HasValue) query = query.Where(c => c.StandardPrice >= q.PriceMin.Value);
            if (q.PriceMax.HasValue) query = query.Where(c => c.StandardPrice <= q.PriceMax.Value);
            return query;
        }

        private static IQueryable<ACAD_Course> ApplySorting(IQueryable<ACAD_Course> query, CourseSearchQuery q)
        {
            return q.Sort switch
            {
                "Created.desc" => query.OrderByDescending(c => c.CreatedAt),
                "Price.asc" => query.OrderBy(c => c.StandardPrice),
                "Price.desc" => query.OrderByDescending(c => c.StandardPrice),
                "StandardScore.asc" => query.OrderBy(c => c.StandardScore),
                "StandardScore.desc" => query.OrderByDescending(c => c.StandardScore),
                _ => query.OrderByDescending(c => c.AverageRating ?? 0)
                          .ThenByDescending(c => c.ACAD_Enrollments.Count())
            };
        }

        private IQueryable<ACAD_Course> GetFacetBaseQuery(CourseSearchQuery q, FacetDimension facet)
        {
            var query = _context.Set<ACAD_Course>()
                .Where(c => c.IsActive && !c.IsDeleted);

            query = ApplyFacetTextSearch(query, q, includeRelatedNames: false);
            query = ApplyFacetFilters(query, q, facet);

            return query;
        }

        private IQueryable<ACAD_Course> GetFacetBaseQueryWithRelatedTextSearch(CourseSearchQuery q, FacetDimension facet)
        {
            var query = _context.Set<ACAD_Course>()
                .Where(c => c.IsActive && !c.IsDeleted);

            query = ApplyFacetTextSearch(query, q, includeRelatedNames: true);
            query = ApplyFacetFilters(query, q, facet);

            return query;
        }

        private static IQueryable<ACAD_Course> ApplyFacetTextSearch(IQueryable<ACAD_Course> query, CourseSearchQuery q, bool includeRelatedNames)
        {
            if (!string.IsNullOrWhiteSpace(q.Q))
            {
                var keyword = q.Q.Trim();
                if (includeRelatedNames)
                {
                    query = query.Where(c =>
                        EF.Functions.Like(c.CourseName, $"%{keyword}%") ||
                        EF.Functions.Like(c.Description!, $"%{keyword}%") ||
                        c.ACAD_CourseSkills.Any(cs => EF.Functions.Like(cs.Skill.Name, $"%{keyword}%")));
                }
                else
                {
                    query = query.Where(c =>
                        EF.Functions.Like(c.CourseName, $"%{keyword}%") ||
                        EF.Functions.Like(c.Description!, $"%{keyword}%"));
                }
            }
            return query;
        }

        //private static IQueryable<ACAD_Course> ApplyFacetFilters(IQueryable<ACAD_Course> query, CourseSearchQuery q, FacetDimension facet)
        //{
        //    if (facet != FacetDimension.Level && q.LevelIds.Count > 0)
        //        query = query.Where(c => q.LevelIds.Contains(c.CourseLevelID));
        //    if (facet != FacetDimension.Category && q.CategoryIds.Count > 0)
        //        query = query.Where(c => q.CategoryIds.Contains(c.CategoryID));
        //    if (facet != FacetDimension.Skill && q.SkillIds.Count > 0)
        //        query = query.Where(c => c.ACAD_CourseSkills.Any(cs => q.SkillIds.Contains(cs.SkillID)));
        //    if (facet != FacetDimension.Requirement && q.RequirementIds.Count > 0)
        //        query = query.Where(c => c.ACAD_CourseRequirements.Any(cr => q.RequirementIds.Contains(cr.RequirementID)));
        //    if (facet != FacetDimension.Benefit && q.BenefitIds.Count > 0)
        //        query = query.Where(c => c.ACAD_CourseBenefits.Any(cb => q.BenefitIds.Contains(cb.BenefitID)));

        //    if (facet != FacetDimension.DayOfWeek && facet != FacetDimension.TimeSlot)
        //    {
        //        if (q.DaysOfWeek.Count > 0) query = query.Where(c => c.ACAD_CourseSchedules.Any(s => q.DaysOfWeek.Contains(s.DayOfWeek)));
        //        if (q.TimeSlotIds.Count > 0) query = query.Where(c => c.ACAD_CourseSchedules.Any(s => q.TimeSlotIds.Contains(s.TimeSlotID)));
        //        if (q.TimeSlotNames.Count > 0) query = query.Where(c => c.ACAD_CourseSchedules.Any(s => q.TimeSlotNames.Contains(s.TimeSlot.Name)));
        //    }

        //    if (q.PriceMin.HasValue) query = query.Where(c => c.StandardPrice >= q.PriceMin.Value);
        //    if (q.PriceMax.HasValue) query = query.Where(c => c.StandardPrice <= q.PriceMax.Value);

        //    return query;
        //}


        private IQueryable<ACAD_Course> ApplyFacetFilters(
            IQueryable<ACAD_Course> query,
            CourseSearchQuery q,
            FacetDimension currentFacet)
        {
            if (currentFacet != FacetDimension.Level && q.LevelIds.Any())
                query = query.Where(c => q.LevelIds.Contains(c.CourseLevelID));

            if (currentFacet != FacetDimension.Category && q.CategoryIds.Any())
                query = query.Where(c => q.CategoryIds.Contains(c.CategoryID));

            if (currentFacet != FacetDimension.Skill && q.SkillIds.Any())
                query = query.Where(c =>
                    c.ACAD_CourseSkills.Any(s => q.SkillIds.Contains(s.SkillID)));

            if (currentFacet != FacetDimension.Requirement && q.RequirementIds.Any())
                query = query.Where(c =>
                    c.ACAD_CourseRequirements.Any(r => q.RequirementIds.Contains(r.RequirementID)));

            if (currentFacet != FacetDimension.Benefit && q.BenefitIds.Any())
                query = query.Where(c =>
                    c.ACAD_CourseBenefits.Any(b => q.BenefitIds.Contains(b.BenefitID)));

            return query;
        }

    }
}
