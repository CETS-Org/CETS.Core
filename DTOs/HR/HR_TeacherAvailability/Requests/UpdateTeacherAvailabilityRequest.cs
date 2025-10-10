using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.HR.HR_TeacherAvailability.Requests
{
	public class UpdateTeacherAvailabilityRequest
	{
		[Required]
		public Guid TeacherID { get; set; }

		[Required]
		public DayOfWeek TeachDay { get; set; }

        public Guid? TimeSlotID { get; set; }
    }
}



