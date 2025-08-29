using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities;

public partial class EVT_EventRegistration : EntityBase
{
    public Guid EventID { get; set; }

    public Guid? AccountID { get; set; }

    [StringLength(256)]
    public string? Email { get; set; }

    [Precision(0)]
    public DateTime RegistrationDate { get; set; }

    [Precision(0)]
    public DateTime? CheckInAt { get; set; }

    [Precision(0)]
    public DateTime? CheckOutAt { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey(nameof(AccountID))]
    public virtual IDN_Account? Account { get; set; }

    [ForeignKey(nameof(EventID))]
    public virtual EVT_Event Event { get; set; } = null!;
}
