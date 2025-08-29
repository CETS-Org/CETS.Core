using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;


namespace Domain.Entities;

public partial class COM_Notification : EntityBase
{
    public string Content { get; set; } = null!;

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    public bool IsPush { get; set; }
}
