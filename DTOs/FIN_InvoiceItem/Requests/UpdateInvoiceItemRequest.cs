using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.FIN_InvoiceItem.Requests
{
	public class UpdateInvoiceItemRequest
	{
		[Required]
		public Guid InvoiceID { get; set; }

		public Guid? CourseID { get; set; }
		public Guid? CoursePackageID { get; set; }

		[Range(1, int.MaxValue)]
		public int Quantity { get; set; }

		[Range(0, double.MaxValue)]
		public decimal UnitPrice { get; set; }

		[Range(0, double.MaxValue)]
		public decimal Subtotal { get; set; }

		[Range(0, double.MaxValue)]
		public decimal Total { get; set; }

		public Guid? PromotionID { get; set; }
	}
}


