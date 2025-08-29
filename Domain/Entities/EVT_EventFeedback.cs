using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities;

public partial class EVT_EventFeedback : EntityBase
{
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
