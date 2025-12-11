using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ACAD_WeeklyFeedback
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ClassID { get; set; }
        public Guid? ClassMeetingID { get; set; }
        public Guid TeacherID { get; set; }
        public Guid StudentID { get; set; }

        public int WeekNumber { get; set; } 

        public string Participation { get; set; } = null!;
        public string AssignmentQuality { get; set; } = null!;
        public string SkillProgress { get; set; } = null!;
        public string? NextStep { get; set; }
        public string? CustomNote { get; set; }

        /// <summary>1: draft, 2: submitted</summary>
        public int Status { get; set; } = 1;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [ForeignKey(nameof(ClassID))]
        public virtual ACAD_Class Class { get; set; } = null!;

        [ForeignKey(nameof(ClassMeetingID))]
        public virtual ACAD_ClassMeeting ClassMeeting { get; set; } = null!;

        [ForeignKey(nameof(TeacherID))]
        public virtual IDN_Teacher Teacher { get; set; } = null!;

        [ForeignKey(nameof(StudentID))]
        public virtual IDN_Student Student { get; set; } = null!;
    }
}
