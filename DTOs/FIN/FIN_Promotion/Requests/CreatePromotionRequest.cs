using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.FIN.FIN_Promotion.Requests
{
	public class CreatePromotionRequest
	{
		[Required]
		public Guid PromotionTypeID { get; set; }

		[Required]
		[StringLength(50)]
		public string Code { get; set; } = null!;

		[Required]
		[StringLength(200)]
		public string Name { get; set; } = null!;

		public decimal? PercentOff { get; set; }
		public decimal? AmountOff { get; set; }
		public DateOnly? StartDate { get; set; }
		public DateOnly? EndDate { get; set; }
		public bool IsActive { get; set; }
	}
}


