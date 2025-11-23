using System;

namespace DTOs.FAC.FAC_Room.Responses
{
	public class RoomResponse
	{
		public Guid Id { get; set; }
		public string RoomCode { get; set; } = null!;
		public int Capacity { get; set; }
		public Guid RoomTypeId { get; set; }
		public string? RoomTypeName { get; set; }
		public Guid RoomStatusId { get; set; }
		public string? RoomStatusName { get; set; }
		public string? OnlineMeetingUrl { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public Guid? UpdatedBy { get; set; }
	}
}



