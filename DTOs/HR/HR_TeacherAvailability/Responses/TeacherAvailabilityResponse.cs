using System;

namespace DTOs.HR.HR_TeacherAvailability.Responses
{
	public class TeacherAvailabilityResponse
	{
		public Guid Id { get; set; }
		public Guid TeacherID { get; set; }
		public string TeachDay{ get; set; } = string.Empty;
        public Guid? TimeSlotID { get; set; }
    }
}



