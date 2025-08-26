using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects.Models;

public partial class IDN_TeacherCredential
{
    [Key]
    public Guid CredentialID { get; set; }

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

    public virtual IDN_Account? UpdatedByNavigation { get; set; }
}
