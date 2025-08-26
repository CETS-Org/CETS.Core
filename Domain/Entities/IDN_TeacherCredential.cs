using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.EntityBase;
using Microsoft.EntityFrameworkCore;


namespace Domain.Entities;

public partial class IDN_TeacherCredential : IEntityBase
{
    [Key]
    [Column("CredentialID")]
    public Guid Id { get; set; }

    public Guid TeacherID { get; set; }

    public Guid CredentialTypeID { get; set; }

    public string? PictureUrl { get; set; }

    [StringLength(255)]
    public string Name { get; set; } = null!;

    [StringLength(255)]
    public string Level { get; set; } = null!;

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual CORE_LookUp CredentialType { get; set; } = null!;

    public virtual IDN_Teacher Teacher { get; set; } = null!;

    [ForeignKey(nameof(UpdatedBy))]
    public virtual IDN_Account? UpdatedByNavigation { get; set; }
}
