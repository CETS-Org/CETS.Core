using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.HR.HR_TeacherAvailability.Requests
{
	public class CreateTeacherAvailabilityRequest
	{
		[Required]
		public Guid TeacherID { get; set; }

		[Required]
		public DateTime TeachDate { get; set; }

		public int? Slot { get; set; }
	}
}



