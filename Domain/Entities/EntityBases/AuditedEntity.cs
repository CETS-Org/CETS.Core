using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Entities.EntityBases.AuditableInterfaces;

namespace Domain.Entities.EntityBases
{
    public abstract class AuditedEntity : EntityBase, IHasCreationTime, IHasCreator, IHasModificationTime, IHasModifier
    {
        [Precision(0)]
        public DateTime CreatedAt { get; set; }

        public Guid CreatedBy { get; set; }
        
        [Precision(0)]
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}
