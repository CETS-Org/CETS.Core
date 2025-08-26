using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects.Models;

public partial class FIN_Promotion
{
    [Key]
    public Guid PromotionID { get; set; }

    public Guid PromotionTypeID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [StringLength(200)]
    public string Name { get; set; } = null!;

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? PercentOff { get; set; }

    [Column(TypeName = "decimal(12, 2)")]
    public decimal? AmountOff { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public bool IsActive { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    public virtual IDN_Account? CreatedByNavigation { get; set; }

    public virtual ICollection<FIN_InvoiceItem> FIN_InvoiceItems { get; set; } = new List<FIN_InvoiceItem>();

    public virtual CORE_LookUp PromotionType { get; set; } = null!;

    public virtual IDN_Account? UpdatedByNavigation { get; set; }
}
