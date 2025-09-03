using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.EVT.EVT_EventRegistration.Requests
{
	public class UpdateEventRegistrationRequest
	{
		[Required]
		public Guid EventID { get; set; }

		public Guid? AccountID { get; set; }

		[StringLength(256)]
		public string? Email { get; set; }

		public DateTime RegistrationDate { get; set; }

		public DateTime? CheckInAt { get; set; }

		public DateTime? CheckOutAt { get; set; }

	}
}



