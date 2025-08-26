using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.EntityBase;
using Microsoft.EntityFrameworkCore;


namespace Domain.Entities;

public partial class EVT_EventFeedback : IEntityBase
{
    [Key]
    [Column("EventFeedbackID")]
    public Guid Id { get; set; }

    public Guid EventID { get; set; }

    public Guid AccountID { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public string? FeedbackUrl { get; set; }

    [Precision(0)]
    public DateTime SubmittedAt { get; set; }

    [ForeignKey(nameof(AccountID))]
    public virtual IDN_Account Account { get; set; } = null!;

    [ForeignKey(nameof(EventID))]
    public virtual EVT_Event Event { get; set; } = null!;
}
