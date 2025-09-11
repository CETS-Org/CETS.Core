using Domain.Entities.EntityBases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ACAD_CourseRequirement : EntityBase
    {
        public Guid CourseID { get; set; }

        public Guid RequirementID { get; set; }

        [ForeignKey(nameof(RequirementID))]
        public virtual CORE_LookUp Requirement { get; set; } = null!;

        [ForeignKey(nameof(CourseID))]
        public virtual ACAD_Course Course { get; set; } = null!;
    }
}
