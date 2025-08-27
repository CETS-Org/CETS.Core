using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.EntityBase;
using Microsoft.EntityFrameworkCore;


namespace Domain.Entities;

public partial class ACAD_ClassReservation : IEntityBase
{
    [Key]
    [Column("ReservationID")]
    public Guid Id { get; set; }

    public Guid ClassID { get; set; }

    public Guid StudentID { get; set; }

    [Precision(0)]
    public DateTime ExpiresAt { get; set; }

    public Guid? InvoiceID { get; set; }

    [ForeignKey(nameof(ClassID))]
    public virtual ACAD_Class Class { get; set; } = null!;

    [ForeignKey(nameof(InvoiceID))]
    public virtual FIN_Invoice? Invoice { get; set; }

    [ForeignKey(nameof(StudentID))]
    public virtual IDN_Student Student { get; set; } = null!;
}
