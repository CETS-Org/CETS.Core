using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects.Models;

public partial class HR_Contract
{
    [Key]
    public Guid ContractID { get; set; }

    public Guid TeacherID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ContractNumber { get; set; } = null!;

    public DateTime? SignedAt { get; set; }

    public DateTime? ExpiredAt { get; set; }

    public Guid ContractStatusID { get; set; }

    public string? ContractUrl { get; set; }

    [StringLength(64)]
    [Unicode(false)]
    public string FileHash { get; set; } = null!;

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public virtual CORE_LookUp ContractStatus { get; set; } = null!;

    public virtual IDN_Account? CreatedByNavigation { get; set; }

    public virtual IDN_Teacher Teacher { get; set; } = null!;

    public virtual IDN_Account? UpdatedByNavigation { get; set; }
}
