using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;

namespace BusinessObjects.Models;

public partial class COM_Conversation : IEntityBase
{
    [Key]
    [Column("ConversationID")]
    public Guid Id { get; set; }

    public Guid SenderID { get; set; }

    public Guid RecipientID { get; set; }

    [Precision(0)]
    public DateTime StartAt { get; set; }

    [ForeignKey(nameof(RecipientID))]
    public virtual IDN_Account Recipient { get; set; } = null!;

    [ForeignKey(nameof(SenderID))]
    public virtual IDN_Account Sender { get; set; } = null!;
}
