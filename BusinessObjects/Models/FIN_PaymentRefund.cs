using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;

namespace BusinessObjects.Models;

public partial class FIN_PaymentRefund : IEntityBase
{
    [Key]
    [Column("RefundID")]
    public Guid Id { get; set; }

    public Guid PaymentID { get; set; }

    [Column(TypeName = "decimal(12, 2)")]
    public decimal Amount { get; set; }

    [StringLength(500)]
    public string? Reason { get; set; }

    public Guid? GatewayID { get; set; }

    [StringLength(255)]
    public string? RefundTxnId { get; set; }

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

    public virtual CORE_LookUp? Gateway { get; set; }

    public virtual FIN_Payment Payment { get; set; } = null!;
}
