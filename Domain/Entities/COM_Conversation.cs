using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using static Domain.Entities.EntityBases.AuditableInterfaces;


namespace Domain.Entities;

public partial class COM_Conversation : EntityBase, IHasCreationTime
{
    public Guid SenderID { get; set; }

    public Guid RecipientID { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [ForeignKey(nameof(RecipientID))]
    public virtual IDN_Account Recipient { get; set; } = null!;

    [ForeignKey(nameof(SenderID))]
    public virtual IDN_Account Sender { get; set; } = null!;
}
