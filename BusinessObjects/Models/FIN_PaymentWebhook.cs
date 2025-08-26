using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;

namespace BusinessObjects.Models;

public partial class FIN_PaymentWebhook : IEntityBase
{
    [Key]
    [Column("WebhookID")]
    public Guid Id { get; set; }

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

    public virtual CORE_LookUp Gateway { get; set; } = null!;

    public virtual FIN_Payment Payment { get; set; } = null!;
}
