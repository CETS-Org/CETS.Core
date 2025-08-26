using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects.Models;

public partial class FIN_Payment
{
    [Key]
    public Guid PaymentID { get; set; }

    public Guid InvoiceID { get; set; }

    [Precision(0)]
    public DateTime PaymentDate { get; set; }

    [Column(TypeName = "decimal(12, 2)")]
    public decimal Amount { get; set; }

    public Guid PaymentMethodID { get; set; }

    [StringLength(255)]
    public string? TransactionID { get; set; }

    public Guid? GatewayID { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? GatewayStatus { get; set; }

    public string? GatewayPayload { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<FIN_PaymentRefund> FIN_PaymentRefunds { get; set; } = new List<FIN_PaymentRefund>();

    public virtual ICollection<FIN_PaymentWebhook> FIN_PaymentWebhooks { get; set; } = new List<FIN_PaymentWebhook>();

    public virtual CORE_LookUp? Gateway { get; set; }

    public virtual FIN_Invoice Invoice { get; set; } = null!;

    public virtual CORE_LookUp PaymentMethod { get; set; } = null!;
}
