using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities;

public partial class FAC_Room : AuditedEntity
{

    [StringLength(50)]
    [Unicode(false)]
    public string RoomCode { get; set; } = null!;

    public int Capacity { get; set; }

    public Guid RoomTypeId { get; set; }

    public string? OnlineMeetingUrl { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<ACAD_ClassMeeting> ACAD_ClassMeetings { get; set; } = new List<ACAD_ClassMeeting>();

    [ForeignKey(nameof(CreatedBy))]
    public virtual IDN_Account? CreatedByNavigation { get; set; }

    [ForeignKey(nameof(RoomTypeId))]
    public virtual CORE_LookUp RoomType { get; set; } = null!;

    [ForeignKey(nameof(UpdatedBy))]
    public virtual IDN_Account? UpdatedByNavigation { get; set; }
}
