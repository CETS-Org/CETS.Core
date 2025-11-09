using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.ACAD.ACAD_Submission.Requests
{
    public class SubmissionUpdate : IValidatableObject
    {
        [Required(ErrorMessage = "SubmissionId is required")]
        public Guid SubmissionId { get; set; }

        [Range(0, 100, ErrorMessage = "Score must be between 0 and 100")]
        public decimal? Score { get; set; }

        [MaxLength(5000, ErrorMessage = "Feedback cannot exceed 5000 characters")]
        public string? Feedback { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // At least one of Score or Feedback must be provided (non-null)
            if (Score == null && Feedback == null)
            {
                yield return new ValidationResult(
                    "At least one of Score or Feedback must be provided",
                    new[] { nameof(Score), nameof(Feedback) }
                );
            }
        }
    }
}



