using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects.Models;

public partial class COM_Notification
{
    [Key]
    public Guid NotificationID { get; set; }

    public string Content { get; set; } = null!;

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    public bool IsPush { get; set; }
}
