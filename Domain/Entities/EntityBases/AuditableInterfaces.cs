using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.EntityBases
{
    public class AuditableInterfaces
    {
        /// <summary>
        /// Represents an entity that has a creation timestamp.
        /// </summary>
        public interface IHasCreationTime
        {
            DateTime CreatedAt { get; set; }
        }
  

        /// <summary>
        /// Represents an entity that has a creator.
        /// </summary>
        public interface IHasCreator
        {
            Guid CreatedBy { get; set; }
        }

        /// <summary>
        /// Represents an entity that has a modification timestamp.
        /// </summary>
        public interface IHasModificationTime
        {
            DateTime? UpdatedAt { get; set; }
        }

        /// <summary>
        /// Represents an entity that has a modifier.
        /// </summary>
        public interface IHasModifier
        {
            Guid? UpdatedBy { get; set; }
        }
    }
}
