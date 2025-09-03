using System;

namespace DTOs.EVT.EVT_Event.Responses
{
	public class EventResponse
	{
		public Guid Id { get; set; }
		public Guid EventTypeID { get; set; }
		public string Name { get; set; } = null!;
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int? MaxSize { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public Guid? UpdatedBy { get; set; }
		public bool IsDeleted { get; set; }
	}
}



