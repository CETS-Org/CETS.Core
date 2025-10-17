using System;

namespace DTOs.FIN.FIN_Promotion.Responses
{
	public class PromotionResponse
	{
		public Guid Id { get; set; }
		public Guid PromotionTypeID { get; set; }
		public string Code { get; set; } = null!;
		public string Name { get; set; } = null!;
		public decimal? PercentOff { get; set; }
		public decimal? AmountOff { get; set; }
		public DateOnly? StartDate { get; set; }
		public DateOnly? EndDate { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public Guid? CreatedBy { get; set; }
		public Guid? UpdatedBy { get; set; }
		public bool IsDeleted { get; set; }
		public AccountSimpleResponse? CreatedByNavigation { get; set; }
		public AccountSimpleResponse? UpdatedByNavigation { get; set; }
		public LookUpSimpleResponse? PromotionType { get; set; }
	}

	public class AccountSimpleResponse
	{
		public Guid AccountId { get; set; }
		public string FullName { get; set; } = null!;
		public string? Email { get; set; }
	}

	public class LookUpSimpleResponse
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = null!;
		public string? Description { get; set; }
	}
}


