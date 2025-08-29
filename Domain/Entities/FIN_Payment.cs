using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities;

public partial class FIN_Payment : EntityBase
{
    public Guid InvoiceID { get; set; }

    public Guid PaymentMethodID { get; set; }

    public Guid? GatewayID { get; set; }

    public string? GatewayStatus { get; set; }

    [StringLength(255)]
    public string? TransactionID { get; set; }

    public decimal Amount { get; set; }

    [Precision(0)]
    public DateTime PaymentDate { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    public string? GatewayPayload { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<FIN_PaymentRefund> FIN_PaymentRefunds { get; set; } = new List<FIN_PaymentRefund>();

    public virtual ICollection<FIN_PaymentWebhook> FIN_PaymentWebhooks { get; set; } = new List<FIN_PaymentWebhook>();

    [ForeignKey(nameof(GatewayID))]
    public virtual CORE_LookUp? Gateway { get; set; }

    [ForeignKey(nameof(InvoiceID))]
    public virtual FIN_Invoice Invoice { get; set; } = null!;

    [ForeignKey(nameof(PaymentMethodID))]
    public virtual CORE_LookUp PaymentMethod { get; set; } = null!;
}
