using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects.Models;

public partial class EVT_EventFeedback
{
    [Key]
    public Guid EventFeedbackID { get; set; }

    public Guid EventID { get; set; }

    public Guid AccountID { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public string? FeedbackUrl { get; set; }

    [Precision(0)]
    public DateTime SubmittedAt { get; set; }

    public virtual IDN_Account Account { get; set; } = null!;

    public virtual EVT_Event Event { get; set; } = null!;
}
