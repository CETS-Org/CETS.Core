using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UUIDNext;

namespace BusinessObjects.Entities
{
    public abstract class EntityBase : IEntityBase
    {
        public Guid Id { get; set; } = Uuid.NewDatabaseFriendly(Database.SqlServer);
    }

    public interface IEntityBase
    {
        Guid Id { get; set; }
    }
}
