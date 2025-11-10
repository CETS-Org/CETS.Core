using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_Course.Responses;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.ACAD
{
    public class ACAD_CourseTeacherAssignmentRepository : BaseRepository<ACAD_CourseTeacherAssignment>, IACAD_CourseTeacherAssignmentRepository
    {
        public ACAD_CourseTeacherAssignmentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TeachingCourseResponse>> GetCoursesByTeacherIdAsync(Guid teacherId)
        {
            var data = await _context.ACAD_Courses
                .Where(c => c.ACAD_CourseTeacherAssignments.Any(cta => cta.TeacherID == teacherId))
                .Select(c => new TeachingCourseResponse
                {
                    Id = c.Id,
                    CourseCode = c.CourseCode,
                    CourseName = c.CourseName,
                    CourseImageUrl = c.CourseImageUrl,
                    CategoryName = c.Category.Name,
                    CourseLevel = c.CourseLevel.Name,
                    FormatName = c.CourseFormat.Name,                   
                    ActiveClassCount =
                        c.ACAD_CourseTeacherAssignments
                         .Where(cta => cta.TeacherID == teacherId)
                        
                         .SelectMany(cta => cta.ACAD_Classes)
                         .Count(cls => cls.IsActive)
                })
                .ToListAsync();

            return data;
        }
        public async Task<IEnumerable<ACAD_CourseTeacherAssignment>> GetCourseTeacherAssignmentsByTeacherIdAsync(Guid teacherId)
        {
            return await _context.ACAD_CourseTeacherAssignments
                .Where(cta => cta.TeacherID == teacherId)
                .Include(cta => cta.Course)
                    .ThenInclude(c => c.Category)
                .Include(cta => cta.Course)
                    .ThenInclude(c => c.CourseLevel)
                .Include(cta => cta.Course)
                    .ThenInclude(c => c.CourseFormat)
                .Include(cta => cta.ACAD_ClassMeetings)
                    .ThenInclude(cm => cm.Room)
                .Include(cta => cta.ACAD_ClassMeetings)
                    .ThenInclude(cm => cm.Class)
                .ToListAsync();
        }
        public async Task<IEnumerable<ACAD_CourseTeacherAssignment>> GetCourseTeacherAssignmentsByTeacherIdAndCourseIdAsync(Guid teacherId, Guid courseId)
        {
            return await _context.ACAD_CourseTeacherAssignments
                .Where(cta => cta.TeacherID == teacherId && cta.CourseID == courseId)
                .Include (cta => cta.ACAD_Classes)
                .ThenInclude(cta => cta.CourseFormat)
                .Include(cta => cta.ACAD_Classes)
                .ThenInclude(cta => cta.ClassStatus)
                .ToListAsync();
        }
    }
}


