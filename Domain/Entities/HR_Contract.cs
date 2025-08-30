using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities;

public partial class HR_Contract : AuditedEntity
{
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

    public bool IsDeleted { get; set; }

    [ForeignKey(nameof(ContractStatusID))]
    public virtual CORE_LookUp ContractStatus { get; set; } = null!;

    [ForeignKey(nameof(CreatedBy))]
    public virtual IDN_Account? CreatedByNavigation { get; set; }

    [ForeignKey(nameof(TeacherID))]
    public virtual IDN_Teacher Teacher { get; set; } = null!;

    [ForeignKey(nameof(UpdatedBy))]
    public virtual IDN_Account? UpdatedByNavigation { get; set; }
}
