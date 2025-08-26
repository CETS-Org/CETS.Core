using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects.Models;

public partial class ACAD_ClassReservation
{
    [Key]
    public Guid ReservationID { get; set; }

    public Guid ClassID { get; set; }

    public Guid StudentID { get; set; }

    [Precision(0)]
    public DateTime ExpiresAt { get; set; }

    public Guid? InvoiceID { get; set; }

    public virtual ACAD_Class Class { get; set; } = null!;

    public virtual FIN_Invoice? Invoice { get; set; }

    public virtual IDN_Student Student { get; set; } = null!;
}
