using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;

namespace Infrastructure.Repositories.ACAD
{
    public class ACAD_CourseTeacherAssignmentRepository : BaseRepository<ACAD_CourseTeacherAssignment>, IACAD_CourseTeacherAssignmentRepository
    {
        public ACAD_CourseTeacherAssignmentRepository(AppDbContext context) : base(context)
        {
        }
    }
}


