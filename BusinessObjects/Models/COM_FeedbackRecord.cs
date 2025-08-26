using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects.Models;

public partial class COM_FeedbackRecord
{
    [Key]
    public Guid FeedbackRecordID { get; set; }

    public string? FormUrl { get; set; }

    public string? ResultUrl { get; set; }

    [Precision(0)]
    public DateTime CreateAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public virtual IDN_Account? CreatedByNavigation { get; set; }
}
