using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Implementations.Repositories.ACAD
{
    public class ACAD_CourseWishlistRepository : BaseRepository<ACAD_CourseWishlist>, IACAD_CourseWishlistRepository
    {
        public ACAD_CourseWishlistRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ACAD_CourseWishlist>> GetWishlistByStudentIdAsync(Guid studentId)
        {
            return await _context.Set<ACAD_CourseWishlist>()
                .Where(w => w.StudentId == studentId)
                .Include(w => w.Course)
                    .ThenInclude(c => c.CourseLevel)
                .Include(w => w.Course)
                    .ThenInclude(c => c.CourseFormat)
                .Include(w => w.Course)
                    .ThenInclude(c => c.Category)
                .Include(w => w.Course)
                    .ThenInclude(c => c.ACAD_CourseTeacherAssignments)
                    .ThenInclude(a => a.Teacher)
                    .ThenInclude(t => t.Account)
                .OrderByDescending(w => w.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> IsCourseInWishlistAsync(Guid studentId, Guid courseId)
        {
            return await _context.Set<ACAD_CourseWishlist>()
                .AnyAsync(w => w.StudentId == studentId && w.CourseId == courseId);
        }

        public async Task<ACAD_CourseWishlist?> GetWishlistItemAsync(Guid studentId, Guid courseId)
        {
            return await _context.Set<ACAD_CourseWishlist>()
                .Include(w => w.Course)
                    .ThenInclude(c => c.CourseLevel)
                .Include(w => w.Course)
                    .ThenInclude(c => c.CourseFormat)
                .Include(w => w.Course)
                    .ThenInclude(c => c.ACAD_CourseTeacherAssignments)
                    .ThenInclude(a => a.Teacher)
                    .ThenInclude(t => t.Account)
                .Include(w => w.Course)
                    .ThenInclude(c => c.ACAD_Syllabi.Where(s => !s.IsDeleted))
                    .ThenInclude(s => s.ACAD_SyllabusItems.Where(i => !i.IsDeleted))
                .Include(w => w.Course)
                    .ThenInclude(c => c.ACAD_Enrollments)
                .Include(w => w.Student)
                .FirstOrDefaultAsync(w => w.StudentId == studentId && w.CourseId == courseId);
        }
    }
}

