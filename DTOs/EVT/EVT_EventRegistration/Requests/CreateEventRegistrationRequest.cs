using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.EVT.EVT_EventRegistration.Requests
{
	public class CreateEventRegistrationRequest
	{
		[Required]
		public Guid EventID { get; set; }

		public Guid? AccountID { get; set; }

		[StringLength(256)]
		public string? Email { get; set; }

		[Required]
		public DateTime RegistrationDate { get; set; }
	}
}



