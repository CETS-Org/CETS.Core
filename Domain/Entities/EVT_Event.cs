using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.EntityBase;
using Microsoft.EntityFrameworkCore;


namespace Domain.Entities;

public partial class EVT_Event : IEntityBase
{
    [Key]
    [Column("EventID")]
    public Guid Id { get; set; }

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

    public virtual CORE_LookUp EventType { get; set; } = null!;
}
