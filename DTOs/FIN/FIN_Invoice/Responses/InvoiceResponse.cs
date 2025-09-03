using System;

namespace DTOs.FIN.FIN_Invoice.Responses
{
	public class InvoiceResponse
	{
		public Guid Id { get; set; }
		public Guid StudentID { get; set; }
		public string InvoiceNumber { get; set; } = null!;
		public Guid InvoiceStatusID { get; set; }
		public DateOnly CreateDate { get; set; }
		public DateOnly? DueDate { get; set; }
		public decimal Subtotal { get; set; }
		public decimal TaxAmount { get; set; }
		public decimal TotalAmount { get; set; }
		public string? SeriesID { get; set; }
		public int? Sequence { get; set; }
		public Guid? PlanTypeID { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public Guid? UpdatedBy { get; set; }
		public bool IsDeleted { get; set; }
	}
}


