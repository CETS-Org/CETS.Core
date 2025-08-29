using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities;

public partial class FIN_PaymentRefund : EntityBase
{
    public Guid PaymentID { get; set; }

    public Guid? GatewayID { get; set; }

    [StringLength(255)]
    public string? RefundTxnId { get; set; }

    public decimal Amount { get; set; }

    [StringLength(500)]
    public string? Reason { get; set; }

    [StringLength(30)]
    public string? GatewayStatus { get; set; }

    public string? GatewayPayload { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey(nameof(GatewayID))]
    public virtual CORE_LookUp? Gateway { get; set; }

    [ForeignKey(nameof(PaymentID))]
    public virtual FIN_Payment Payment { get; set; } = null!;
}
