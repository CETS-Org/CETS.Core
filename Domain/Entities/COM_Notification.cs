using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.EntityBase;
using Microsoft.EntityFrameworkCore;


namespace Domain.Entities;

public partial class COM_Notification : IEntityBase
{
    [Key]
    [Column("NotificationID")]
    public Guid Id { get; set; }

    public string Content { get; set; } = null!;

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    public bool IsPush { get; set; }
}
