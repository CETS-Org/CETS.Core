using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.EVT.EVT_Event.Requests
{
	public class CreateEventRequest
	{
		[Required]
		public Guid EventTypeID { get; set; }

		[Required]
		[StringLength(255)]
		public string Name { get; set; } = null!;

		[Required]
		public DateTime StartDate { get; set; }

		[Required]
		public DateTime EndDate { get; set; }

		public int? MaxSize { get; set; }
	}
}



