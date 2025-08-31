using UUIDNext;

namespace Utils.Helpers
{
    /// <summary>
    /// Utility class for generating database-friendly UUIDs
    /// </summary>
    public class IdGenerator
    {
        /// <summary>
        /// Generates a new database-friendly UUID optimized for SQL Server
        /// </summary>
        /// <returns>A new Guid that is optimized for SQL Server storage and indexing</returns>
        public Guid GenerateId()
        {
            return Uuid.NewDatabaseFriendly(Database.SqlServer);
        }

        /// <summary>
        /// Generates a new database-friendly UUID optimized for SQL Server
        /// </summary>
        /// <param name="database">The database type to optimize for (defaults to SQL Server)</param>
        /// <returns>A new Guid that is optimized for the specified database storage and indexing</returns>
        public Guid GenerateId(Database database = Database.SqlServer)
        {
            return Uuid.NewDatabaseFriendly(database);
        }

        /// <summary>
        /// Generates multiple database-friendly UUIDs
        /// </summary>
        /// <param name="count">Number of UUIDs to generate</param>
        /// <param name="database">The database type to optimize for (defaults to SQL Server)</param>
        /// <returns>An array of Guids optimized for the specified database</returns>
        public Guid[] GenerateIds(int count, Database database = Database.SqlServer)
        {
            if (count <= 0)
                throw new ArgumentException("Count must be greater than 0", nameof(count));

            var ids = new Guid[count];
            for (int i = 0; i < count; i++)
            {
                ids[i] = Uuid.NewDatabaseFriendly(database);
            }
            return ids;
        }

        /// <summary>
        /// Generates multiple database-friendly UUIDs as an enumerable
        /// </summary>
        /// <param name="count">Number of UUIDs to generate</param>
        /// <param name="database">The database type to optimize for (defaults to SQL Server)</param>
        /// <returns>An enumerable of Guids optimized for the specified database</returns>
        public IEnumerable<Guid> GenerateIdsEnumerable(int count, Database database = Database.SqlServer)
        {
            if (count <= 0)
                throw new ArgumentException("Count must be greater than 0", nameof(count));

            for (int i = 0; i < count; i++)
            {
                yield return Uuid.NewDatabaseFriendly(database);
            }
        }
    }
}
