using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities;

public partial class ACAD_SyllabusItem : EntityBase
{
    public Guid SyllabusID { get; set; }

    public int SessionNumber { get; set; }

    [StringLength(255)]
    public string TopicTitle { get; set; } = null!;

    public int? EstimatedMinutes { get; set; }

    public bool Required { get; set; }

    public string? Objectives { get; set; }

    public string? ContentSummary { get; set; }

    public string? PreReadingUrl { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<ACAD_ClassMeeting> ACAD_ClassMeetings { get; set; } = new List<ACAD_ClassMeeting>();

    [ForeignKey(nameof(CreatedBy))]
    public virtual IDN_Account? CreatedByNavigation { get; set; }

    [ForeignKey(nameof(SyllabusID))]
    public virtual ACAD_Syllabus Syllabus { get; set; } = null!;

    [ForeignKey(nameof(UpdatedBy))]
    public virtual IDN_Account? UpdatedByNavigation { get; set; }
}
