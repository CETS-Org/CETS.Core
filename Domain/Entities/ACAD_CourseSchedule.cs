using Domain.Entities.EntityBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ACAD_CourseSchedule : AuditedEntity
    {
        public Guid CourseID { get; set; }
        public Guid LookUpID { get; set; } // e.g., Time Slot ID
        public string DayOfWeek { get; set; } // Sunday, Monday, ...,Saturday

        // Navigation properties
        public virtual ACAD_Course Course { get; set; } = null!;
        public virtual CORE_LookUp TimeSlot { get; set; } = null!;

    }
}
