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

        public async Task<IReadOnlyList<WeeklyFeedbackViewDto>> GetByClassWeekAsync(Guid ClassID, int weekNumber, CancellationToken ct = default)
        {
            return await _db.ACAD_WeeklyFeedbacks
                .Where(x => x.ClassID == ClassID && x.WeekNumber == weekNumber)
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
                    // TODO: join Class/Student/Teacher to enrich names if needed
                })
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<WeeklyFeedbackViewDto>> GetByStudentAsync(Guid studentId, Guid? ClassID, CancellationToken ct = default)
        {
            var q = _db.ACAD_WeeklyFeedbacks.AsQueryable().Where(x => x.StudentID == studentId);
            if (ClassID.HasValue) q = q.Where(x => x.ClassID == ClassID.Value);

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
                })
                .ToListAsync(ct);
        }

        public async Task<WeeklyFeedbackViewDto?> GetDetailAsync(Guid id, CancellationToken ct = default)
        {
            return await _db.ACAD_WeeklyFeedbacks
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
                })
                .FirstOrDefaultAsync(ct);
        }
    }
}
