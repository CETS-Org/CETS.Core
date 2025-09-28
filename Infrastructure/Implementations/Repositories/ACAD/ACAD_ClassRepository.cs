using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_Class.Responses;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.ACAD
{
    public class ACAD_ClassRepository : BaseRepository<ACAD_Class>, IACAD_ClassRepository
    {
        public ACAD_ClassRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<LearningClassResponse>> GetLearningClassByStudentId(Guid studentId)
        {
            var result = await (
                from enroll in _context.ACAD_Enrollments
                join course in _context.ACAD_Courses on enroll.CourseID equals course.Id
                join cls in _context.ACAD_Classes on enroll.ClassID equals cls.Id
                join meeting in _context.ACAD_ClassMeetings on cls.Id equals meeting.ClassID
                join room in _context.FAC_Rooms on meeting.RoomID equals room.Id
                join assign in _context.ACAD_CourseTeacherAssignments on cls.TeacherAssignmentID equals assign.Id
                join teacherAcc in _context.IDN_Accounts on assign.TeacherID equals teacherAcc.Id
                join statusLookup in _context.CORE_LookUps on cls.ClassStatusID equals statusLookup.Id
                join slotLookup in _context.CORE_LookUps on meeting.SlotID equals slotLookup.Id
                where enroll.StudentID == studentId
                select new
                {
                    Enrollment = enroll,
                    Class = cls,
                    Course = course,
                    Meeting = meeting,
                    Room = room,
                    TeacherAccount = teacherAcc,
                    StatusLookup = statusLookup,
                    SlotLookup = slotLookup
                }
            ).ToListAsync();

            List<LearningClassResponse> response = new List<LearningClassResponse>();

            foreach (var e in result)
            {
                var classes = new LearningClassResponse()
                {
                    Id = e.Class.Id,
                    StatusName = e.StatusLookup.Name,      // trạng thái lớp
                    CourseName = e.Course.CourseName,
                    ClassName = e.Class.ClassName,
                    TeacherName = e.TeacherAccount.FullName, // tên giáo viên từ Account
                    StartDate = e.Class.StartDate,
                    EndDate = e.Class.EndDate,
                    TimeSlot = e.SlotLookup.Name,          // Slot từ lookup
                    RoomCode = e.Room.RoomCode,
                    IsActive = e.Class.IsActive
                };

                response.Add(classes);
            }

            return response;
        }

        /*private async Task<string?> GetRoomByClassIdAsync(Guid? classId)
        {
            if (classId == null)
                return null;

            return await _context.ACAD_ClassMeetings
                .Where(c => c.Class.Id == classId)
                .Select(c => c.Room.RoomCode)  
                .FirstOrDefaultAsync();
        }*/
    }
}


