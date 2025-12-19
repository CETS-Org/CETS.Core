using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTOs.ACAD.ACAD_Enrollment.Requests
{
    public class BulkUpdateFinalGradesRequest
    {
        [Required]
        [MinLength(1, ErrorMessage = "At least one final grade must be provided")]
        [MaxLength(500, ErrorMessage = "Maximum 500 final grades per request")]
        public List<FinalGradeUpdate> FinalGrades { get; set; } = new List<FinalGradeUpdate>();
    }

    public class FinalGradeUpdate
    {
        [Required]
        public Guid EnrollmentId { get; set; }

        [Range(0, 100, ErrorMessage = "Final grade must be between 0 and 100")]
        public decimal? FinalGrade { get; set; }
    }
}





