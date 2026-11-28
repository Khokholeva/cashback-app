using cashback_domain.Entities;
using Microsoft.Data.Sqlite;

namespace cashback_infrastructure.DB
{
    public class BankLocalRepository : SQLiteDBRepo<IBankEntity>, IBankLocalRepository
    {
        public override async Task DeleteByIDAsync(int? id)
        {
            if (id is null)
                throw new ArgumentNullException($"{nameof(id)} был NULL!");

            await using var connection = await GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = "DELETE FROM Banks WHERE id = $id";
            cmd.Parameters.AddWithValue("$id", id.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        private IBankEntity ReadFromDB(SqliteDataReader reader)
        {
            var id = reader.GetInt32(0);
            var name = reader.GetString(1);

            return new BankEntityFromDB(name, id);
        }

        public override async Task<IBankEntity> FindByIDAsync(int? id)
        {
            if (id is null)
                throw new ArgumentNullException($"{nameof(id)} был NULL!");

            await using var connection = await GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = "SELECT id, name FROM Banks WHERE id = $id";
            cmd.Parameters.AddWithValue("$id", id.Value);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return ReadFromDB(reader);

            throw new KeyNotFoundException($"ID {id} не нашлось в базе Banks!");
        }

        public override async IAsyncEnumerable<IBankEntity> GetAllEntriesAsync()
        {
            await using var connection = await GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = "SELECT id, name FROM Banks";

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                yield return ReadFromDB(reader);
        }

        protected override string GetTableCreationCommand()
        {
            return @"CREATE TABLE IF NOT EXISTS Banks (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT NOT NULL
            );";
        }

        public override async Task SaveAsync(IBankEntity entity)
        {
            await using var connection = await GetConnection();
            var cmd = connection.CreateCommand();

            if (entity.Id is null)
            {
                cmd.CommandText =
                    "INSERT INTO Banks (name) " +
                    "VALUES ($name)";
            }
            else
            {
                cmd.CommandText =
                    "UPDATE Banks SET name = $name " +
                    "WHERE id = $id";
                cmd.Parameters.AddWithValue("$id", entity.Id);
            }

            cmd.Parameters.AddWithValue("$name", entity.Name);

            await cmd.ExecuteNonQueryAsync();
        }
    }

    internal class BankEntityFromDB : IBankEntity
    {
        public BankEntityFromDB(string name, int? id)
        {
            Name = name;
            Id = id;
        }

        public string Name { get; init; }
        public int? Id { get; init; }
    }
}