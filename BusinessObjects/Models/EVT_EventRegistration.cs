using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;

namespace BusinessObjects.Models;

public partial class EVT_EventRegistration : IEntityBase
{
    [Key]
    [Column("RegistrationID")]
    public Guid Id { get; set; }

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
