using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects.Models;

public partial class HR_TeacherAvailability
{
    [Key]
    public Guid AvailabilityID { get; set; }

    public Guid TeacherID { get; set; }

    [Precision(0)]
    public DateTime TeachDate { get; set; }

    public int? Slot { get; set; }

    public virtual IDN_Teacher Teacher { get; set; } = null!;
}
