using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_CourseSkill.Requests
{
    public class CreateSkillRequest
    {
        public Guid CourseID { get; set; }
        public Guid SkillID { get; set; }
    }
}
