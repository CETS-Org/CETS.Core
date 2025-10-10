using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities;

public partial class HR_TeacherAvailability : EntityBase
{
    public Guid TeacherID { get; set; }

    [Precision(0)]
    public DayOfWeek TeachDay { get; set; }

    public Guid TimeSlotID { get; set; }

    public virtual CORE_LookUp TimeSlot { get; set; } = null!;

    [ForeignKey(nameof(TeacherID))]
    public virtual IDN_Teacher Teacher { get; set; } = null!;
}
