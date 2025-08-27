using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.EntityBase;
using Microsoft.EntityFrameworkCore;


namespace Domain.Entities;

public partial class HR_TeacherAvailability : IEntityBase
{
    [Key]
    [Column("AvailabilityID")]
    public Guid Id { get; set; }

    public Guid TeacherID { get; set; }

    [Precision(0)]
    public DateTime TeachDate { get; set; }

    public int? Slot { get; set; }

    [ForeignKey(nameof(TeacherID))]
    public virtual IDN_Teacher Teacher { get; set; } = null!;
}
