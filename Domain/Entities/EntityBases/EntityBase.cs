using UUIDNext;

namespace Domain.Entities.EntityBases
{
    public abstract class EntityBase : IEntityBase
    {
        public Guid Id { get; set; } = Uuid.NewDatabaseFriendly(Database.SqlServer);
    }


}
