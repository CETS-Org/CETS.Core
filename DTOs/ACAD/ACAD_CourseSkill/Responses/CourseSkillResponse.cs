using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_CourseSkill.Responses
{
    public class CourseSkillResponse
    {
        public Guid Id { get; set; }
        public Guid CourseID { get; set; }
        public string CourseName { get; set; } = null!;
        public Guid SkillID { get; set; }
        public string SkillName { get; set; } = null!;
    }
}
