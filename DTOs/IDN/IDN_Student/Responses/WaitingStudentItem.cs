using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.IDN.IDN_Student.Responses
{
    public class WaitingStudentResponse
    {
        public Guid StudentId { get; set; }
        public Guid EnrollmentId { get; set; }
        public string StudentCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }

    public class WaitingStudentSearchResult
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public bool HasMore { get; set; }
        public List<WaitingStudentResponse> Items { get; set; } = new();
    }
}
