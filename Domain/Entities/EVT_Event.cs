using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities;

public partial class EVT_Event : EntityBase
{
    public Guid EventTypeID { get; set; }

    [StringLength(255)]
    public string Name { get; set; } = null!;

    [Precision(0)]
    public DateTime StartDate { get; set; }

    [Precision(0)]
    public DateTime EndDate { get; set; }

    public int? MaxSize { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<EVT_EventFeedback> EVT_EventFeedbacks { get; set; } = new List<EVT_EventFeedback>();

    public virtual ICollection<EVT_EventRegistration> EVT_EventRegistrations { get; set; } = new List<EVT_EventRegistration>();

    [ForeignKey(nameof(EventTypeID))]
    public virtual CORE_LookUp EventType { get; set; } = null!;
}
