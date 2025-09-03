using System;

namespace DTOs.EVT.EVT_EventRegistration.Responses
{
	public class EventRegistrationResponse
	{
		public Guid Id { get; set; }
		public Guid EventID { get; set; }
		public Guid? AccountID { get; set; }
		public string? Email { get; set; }
		public DateTime RegistrationDate { get; set; }
		public DateTime? CheckInAt { get; set; }
		public DateTime? CheckOutAt { get; set; }
		public bool IsDeleted { get; set; }
	}
}



