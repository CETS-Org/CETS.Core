using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects.Models;

public partial class COM_Conversation
{
    [Key]
    public Guid ConversationID { get; set; }

    public Guid SenderID { get; set; }

    public Guid RecipientID { get; set; }

    [Precision(0)]
    public DateTime StartAt { get; set; }

    public virtual IDN_Account Recipient { get; set; } = null!;

    public virtual IDN_Account Sender { get; set; } = null!;
}
