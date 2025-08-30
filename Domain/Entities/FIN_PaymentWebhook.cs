using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Domain.Entities.EntityBases.AuditableInterfaces;


namespace Domain.Entities;

public partial class FIN_PaymentWebhook : EntityBase, IHasCreationTime
{
    public Guid PaymentID { get; set; }

    public Guid EventId { get; set; }

    public Guid GatewayID { get; set; }

    [StringLength(100)]
    public string EventType { get; set; } = null!;

    [Precision(0)]
    public DateTime ReceivedAt { get; set; }

    public string Payload { get; set; } = null!;

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [ForeignKey(nameof(GatewayID))]
    public virtual CORE_LookUp Gateway { get; set; } = null!;

    [ForeignKey(nameof(PaymentID))]
    public virtual FIN_Payment Payment { get; set; } = null!;
}
