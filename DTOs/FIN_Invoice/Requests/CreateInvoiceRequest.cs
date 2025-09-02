using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.FIN_Invoice.Requests
{
	public class CreateInvoiceRequest
	{
		[Required]
		public Guid StudentID { get; set; }

		[Required]
		[StringLength(50)]
		public string InvoiceNumber { get; set; } = null!;

		[Required]
		public Guid InvoiceStatusID { get; set; }

		[Required]
		public DateOnly CreateDate { get; set; }

		public DateOnly? DueDate { get; set; }

		[Range(0, double.MaxValue)]
		public decimal Subtotal { get; set; }

		[Range(0, double.MaxValue)]
		public decimal TaxAmount { get; set; }

		[Range(0, double.MaxValue)]
		public decimal TotalAmount { get; set; }

		[StringLength(50)]
		public string? SeriesID { get; set; }

		public int? Sequence { get; set; }

		public Guid? PlanTypeID { get; set; }
	}
}


