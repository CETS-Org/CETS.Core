using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using static Domain.Entities.EntityBases.AuditableInterfaces;


namespace Domain.Entities;

public partial class COM_FeedbackRecord : EntityBase, IHasCreationTime, IHasCreator
{
    public string? FormUrl { get; set; }

    public string? ResultUrl { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey(nameof(CreatedBy))]
    public virtual IDN_Account? CreatedByNavigation { get; set; }
}
