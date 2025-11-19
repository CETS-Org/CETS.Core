using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Domain.Entities.EntityBases.AuditableInterfaces;

namespace Domain.Entities
{
    public partial class ACAD_PlacementTest : EntityBase, IHasCreationTime, IHasModificationTime, IHasCreator, IHasModifier
    {
        [StringLength(100)]
        public string Title { get; set; } = null!; // Tên bài test

        public int DurationMinutes { get; set; } // Thời lượng bài test

        public string? StoreUrl { get; set; } // Link JSON bài test trên cloud

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
    }


}
