using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_WeeklyFeedback.Request;
using DTOs.ACAD.ACAD_WeeklyFeedback.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations.Repositories.ACAD
{
    public class WeeklyFeedbackRepository : IWeeklyFeedbackRepository
    {
        private readonly AppDbContext _db;

        public WeeklyFeedbackRepository(AppDbContext db)
        {
            _db = db;

        }

        public async Task UpsertAsync(Guid teacherId, UpsertWeeklyFeedbackRequestDto req, CancellationToken ct = default)
        {
            foreach (var item in req.Items)
            {
                var entity = await _db.ACAD_WeeklyFeedbacks
                    .FirstOrDefaultAsync(x =>
                        x.ClassID == req.ClassID &&
                        x.StudentID == item.StudentId &&
                        x.WeekNumber == req.WeekNumber, ct);

                if (entity is null)
                {
                    entity = new ACAD_WeeklyFeedback
                    {
                        ClassID = req.ClassID,
                        ClassMeetingID = req.ClassMeetingId,
                        StudentID = item.StudentId,
                        TeacherID = teacherId,
                        WeekNumber = req.WeekNumber,
                    };
                    _db.ACAD_WeeklyFeedbacks.Add(entity);
                }

                entity.Participation = item.Participation;
                entity.AssignmentQuality = item.AssignmentQuality;
                entity.SkillProgress = item.SkillProgress;
                entity.NextStep = item.NextStep;
                entity.CustomNote = item.CustomNote;
                entity.Status = req.Submit ? 2 : 1;
                entity.UpdatedAt = DateTime.UtcNow;
            }

            await _db.SaveChangesAsync(ct);
        }

        public async Task<IReadOnlyList<WeeklyFeedbackViewDto>> GetByClassWeekAsync(
     Guid classId,
     int weekNumber,
     CancellationToken ct = default)
        {
            return await (
                from fb in _db.ACAD_WeeklyFeedbacks

                    // JOIN CLASS
                join cls in _db.ACAD_Classes
                    on fb.ClassID equals cls.Id into clsJoin
                from cls in clsJoin.DefaultIfEmpty()

                    // JOIN STUDENT + ACCOUNT
                join std in _db.IDN_Students
                    on fb.StudentID equals std.Id into stdJoin
                from std in stdJoin.DefaultIfEmpty()

                join stdAcc in _db.IDN_Accounts
                    on std.Id equals stdAcc.Id into stdAccJoin
                from stdAcc in stdAccJoin.DefaultIfEmpty()

                    // JOIN TEACHER + ACCOUNT
                join tea in _db.IDN_Teachers
                    on fb.TeacherID equals tea.Id into teaJoin
                from tea in teaJoin.DefaultIfEmpty()

                join teaAcc in _db.IDN_Accounts
                    on tea.Id equals teaAcc.Id into teaAccJoin
                from teaAcc in teaAccJoin.DefaultIfEmpty()

                where fb.ClassID == classId && fb.WeekNumber == weekNumber

                select new WeeklyFeedbackViewDto
                {
                    Id = fb.Id,
                    ClassID = fb.ClassID,
                    ClassMeetingId = fb.ClassMeetingID,
                    TeacherId = fb.TeacherID,
                    StudentId = fb.StudentID,
                    WeekNumber = fb.WeekNumber,
                    Participation = fb.Participation,
                    AssignmentQuality = fb.AssignmentQuality,
                    SkillProgress = fb.SkillProgress,
                    NextStep = fb.NextStep,
                    CustomNote = fb.CustomNote,
                    Status = fb.Status,
                    UpdatedAt = fb.UpdatedAt,

                    // ⭐ ENRICHED DATA
                    ClassName = cls.ClassName,                     // từ ACAD_Class
                    StudentName = stdAcc.FullName,                // từ IDN_Account
                    TeacherName = teaAcc.FullName                 // từ IDN_Account
                }
            ).ToListAsync(ct);
        }

        public async Task<IReadOnlyList<WeeklyFeedbackViewDto>> GetByStudentAsync(
     Guid studentId,
     Guid? ClassID,
     CancellationToken ct = default)
        {
            var q = _db.ACAD_WeeklyFeedbacks
                .Include(x => x.Class)
                .Include(x => x.Student).ThenInclude(s => s.Account)
                .Include(x => x.Teacher).ThenInclude(t => t.Account)
                .Where(x => x.StudentID == studentId);

            if (ClassID.HasValue)
                q = q.Where(x => x.ClassID == ClassID.Value);

            return await q
                .OrderByDescending(x => x.UpdatedAt)
                .Select(x => new WeeklyFeedbackViewDto
                {
                    Id = x.Id,
                    ClassID = x.ClassID,
                    ClassMeetingId = x.ClassMeetingID,
                    TeacherId = x.TeacherID,
                    StudentId = x.StudentID,
                    WeekNumber = x.WeekNumber,
                    Participation = x.Participation,
                    AssignmentQuality = x.AssignmentQuality,
                    SkillProgress = x.SkillProgress,
                    NextStep = x.NextStep,
                    CustomNote = x.CustomNote,
                    Status = x.Status,
                    UpdatedAt = x.UpdatedAt,

                    // ⭐ Enrich:
                    ClassName = x.Class.ClassName,
                    StudentName = x.Student.Account.FullName,
                    TeacherName = x.Teacher.Account.FullName
                })
                .ToListAsync(ct);
        }


        public async Task<WeeklyFeedbackViewDto?> GetDetailAsync(Guid id, CancellationToken ct = default)
        {
            return await _db.ACAD_WeeklyFeedbacks
                .Include(x => x.Class)
                .Include(x => x.Student).ThenInclude(s => s.Account)
                .Include(x => x.Teacher).ThenInclude(t => t.Account)
                .Where(x => x.Id == id)
                .Select(x => new WeeklyFeedbackViewDto
                {
                    Id = x.Id,
                    ClassID = x.ClassID,
                    ClassMeetingId = x.ClassMeetingID,
                    TeacherId = x.TeacherID,
                    StudentId = x.StudentID,
                    WeekNumber = x.WeekNumber,
                    Participation = x.Participation,
                    AssignmentQuality = x.AssignmentQuality,
                    SkillProgress = x.SkillProgress,
                    NextStep = x.NextStep,
                    CustomNote = x.CustomNote,
                    Status = x.Status,
                    UpdatedAt = x.UpdatedAt,

                    // ⭐ Enrich:
                    ClassName = x.Class.ClassName,
                    StudentName = x.Student.Account.FullName,
                    TeacherName = x.Teacher.Account.FullName
                })
                .FirstOrDefaultAsync(ct);
        }

    }
}
