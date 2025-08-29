using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities;

public partial class COM_FeedbackRecord : EntityBase
{
    public string? FormUrl { get; set; }

    public string? ResultUrl { get; set; }

    [Precision(0)]
    public DateTime CreateAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey(nameof(CreatedBy))]
    public virtual IDN_Account? CreatedByNavigation { get; set; }
}
