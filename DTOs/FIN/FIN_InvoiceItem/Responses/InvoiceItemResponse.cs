using System;

namespace DTOs.FIN.FIN_InvoiceItem.Responses
{
	public class InvoiceItemResponse
	{
		public Guid Id { get; set; }
		public Guid InvoiceID { get; set; }
		public Guid? CourseID { get; set; }
		public Guid? CoursePackageID { get; set; }
		public int Quantity { get; set; }
		public decimal UnitPrice { get; set; }
		public decimal Subtotal { get; set; }
		public decimal Total { get; set; }
		public Guid? PromotionID { get; set; }
		public bool IsDeleted { get; set; }
	}
}


