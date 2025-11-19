using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Domain.Entities.EntityBases.AuditableInterfaces;

namespace Domain.Entities
{
   public partial class ACAD_PlacementQuestion : EntityBase, IHasCreationTime, IHasModificationTime, IHasCreator, IHasModifier
{
    
    public Guid SkillTypeID { get; set; } // "Reading" hoặc "Listening"

    public Guid QuestionTypeID { get; set; } // Liên kết với CORE_LookUp (MCQ, TrueFalse, FillGap, Passage, Audio)

    [StringLength(100)]
    public string Title { get; set; } = null!; // Tên câu hỏi hoặc passage title

    public string? QuestionUrl { get; set; } // Link JSON trên cloud

    public int Difficulty { get; set; } = 1; // 1: câu hỏi đơn, 2: ngắn, 3: dài
    
    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey(nameof(CreatedBy))]
    public virtual IDN_Account? CreatedByNavigation { get; set; }

    [ForeignKey(nameof(UpdatedBy))]
    public virtual IDN_Account? UpdatedByNavigation { get; set; }

    [ForeignKey(nameof(SkillTypeID))]
    public virtual CORE_LookUp Skill { get; set; } = null!;

    [ForeignKey(nameof(QuestionTypeID))]
    public virtual CORE_LookUp QuestionType { get; set; } = null!;
    }


}
