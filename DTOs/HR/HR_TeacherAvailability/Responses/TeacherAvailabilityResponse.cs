using System;

namespace DTOs.HR.HR_TeacherAvailability.Responses
{
	public class TeacherAvailabilityResponse
	{
		public Guid Id { get; set; }
		public Guid TeacherID { get; set; }
		public DateTime TeachDate { get; set; }
		public int? Slot { get; set; }
	}
}



