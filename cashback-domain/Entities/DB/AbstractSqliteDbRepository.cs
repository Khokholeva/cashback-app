using Microsoft.Data.Sqlite;

namespace cashback_domain.Entities
{
    [Obsolete]
    public abstract class AbstractSqliteDbRepository<TEntity> : ISqLiteDbRepository<TEntity>
        where TEntity : IEntity<int>
    {
        public string DbFilename { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public abstract Task DeleteByIDAsync(int id);

        public abstract Task<TEntity> FindByIDAsync(int id);

        public abstract Task<IEnumerable<TEntity>> GetAllEntriesAsync();

        public abstract string GetTableCreationCommand();

        public async Task InitializeDBAsync()
        {
            using var connection = new SqliteConnection(DbFilename);
            await connection.OpenAsync();

            var createCmd = connection.CreateCommand();

            createCmd.CommandText = GetTableCreationCommand();

            await createCmd.ExecuteNonQueryAsync();
        }

        public Task InitializeDBAsync(string dataSource)
        {
            throw new NotImplementedException();
        }

        public abstract Task SaveAsync(TEntity entity);
    }
}
