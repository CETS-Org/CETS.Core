using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;
using static Domain.Entities.EntityBases.AuditableInterfaces;


namespace Domain.Entities;

public partial class COM_Notification : EntityBase, IHasCreationTime
{
    public string Content { get; set; } = null!;

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    public bool IsPush { get; set; }
}
