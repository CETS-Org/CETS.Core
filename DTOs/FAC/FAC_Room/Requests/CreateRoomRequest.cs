using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.FAC.FAC_Room.Requests
{
	public class CreateRoomRequest
	{
		[Required]
		[StringLength(50)]
		public string RoomCode { get; set; } = null!;

		[Range(1, int.MaxValue)]
		public int Capacity { get; set; }

		[Required]
		public Guid RoomTypeId { get; set; }

		[StringLength(2048)]
		public string? OnlineMeetingUrl { get; set; }

		public bool IsActive { get; set; }
	}
}



