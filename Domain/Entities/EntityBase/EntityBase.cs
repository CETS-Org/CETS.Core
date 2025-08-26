using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UUIDNext;

namespace Domain.Entities.EntityBase
{
    public abstract class EntityBase : IEntityBase
    {
        public Guid Id { get; set; } = Uuid.NewDatabaseFriendly(Database.SqlServer);
    }

   
}
