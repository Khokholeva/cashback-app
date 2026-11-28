using cashback_domain.Entities;
using Microsoft.Data.Sqlite;

namespace cashback_infrastructure.DB
{
    public class MCCLocalRepository : SQLiteDBRepo<IMCCEntity>, IMCCLocalRepository
    {
        public override async Task DeleteByIDAsync(int? id)
        {
            if (id is null)
                throw new ArgumentNullException($"{nameof(id)} был NULL!");

            await using var connection = await GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = "DELETE FROM mcc WHERE id = $id";
            cmd.Parameters.AddWithValue("$id", id.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        private IMCCEntity ReadFromDB(SqliteDataReader reader)
        {
            var id = reader.GetInt32(0);
            var name = reader.GetString(1);

            return new MCCEntityFromDB(name, id);
        }

        public override async Task<IMCCEntity> FindByIDAsync(int? id)
        {
            if (id is null)
                throw new ArgumentNullException($"{nameof(id)} был NULL!");

            await using var connection = await GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = "SELECT id, name FROM mcc WHERE id = $id";
            cmd.Parameters.AddWithValue("$id", id.Value);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return ReadFromDB(reader);

            throw new KeyNotFoundException($"MCC-кода {id} не нашлось в базе mcc!");
        }

        public override async IAsyncEnumerable<IMCCEntity> GetAllEntriesAsync()
        {
            await using var connection = await GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = "SELECT id, name FROM mcc";

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                yield return ReadFromDB(reader);
        }

        protected override string GetTableCreationCommand()
        {
            return @"CREATE TABLE IF NOT EXISTS mcc (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT NOT NULL
            );";
        }

        public override async Task SaveAsync(IMCCEntity entity)
        {
            await using var connection = await GetConnection();
            var cmd = connection.CreateCommand();

            if (entity.Id is null)
            {
                cmd.CommandText =
                    "INSERT INTO mcc (name) " +
                    "VALUES ($name)";
            }
            else
            {
                cmd.CommandText =
                    "UPDATE mcc SET name = $name " +
                    "WHERE id = $id";

                cmd.Parameters.AddWithValue("$id", entity.Id.Value);
            }

            cmd.Parameters.AddWithValue("$name", entity.Name);

            await cmd.ExecuteNonQueryAsync();
        }
    }

    internal class MCCEntityFromDB : IMCCEntity
    {
        public MCCEntityFromDB(string name, int? id)
        {
            Name = name;
            Id = id;
        }

        public string Name { get; init; }

        public int? Id { get; init; }
    }
}