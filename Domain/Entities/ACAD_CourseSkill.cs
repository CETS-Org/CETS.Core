using Domain.Entities.EntityBases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ACAD_CourseSkill : EntityBase
    {
        public Guid CourseID { get; set; }

        public Guid SkillID { get; set; }

        [ForeignKey(nameof(CourseID))]
        public virtual ACAD_Course Course { get; set; } = null!;

        [ForeignKey(nameof(SkillID))]
        public virtual CORE_LookUp Benefit { get; set; } = null!;
    }
}
