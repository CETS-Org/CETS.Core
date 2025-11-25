using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.FAC.FAC_Room.Requests
{
	public class UpdateRoomRequest
	{
	
	
		public string? RoomCode { get; set; } = null!;

	
		public int? Capacity { get; set; }

		public Guid? RoomTypeId { get; set; }

		public Guid? RoomStatusId { get; set; }

		public string? OnlineMeetingUrl { get; set; }

		public bool? IsActive { get; set; }
	}
}



