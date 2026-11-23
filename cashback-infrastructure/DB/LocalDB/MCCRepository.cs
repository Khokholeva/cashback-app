using cashback_domain.Entities;
using cashback_infrastructure.DB.LocalDB;
using Microsoft.Data.Sqlite;

namespace cashback_infrastructure.DB
{
    public class MCCRepository : SQLiteDBRepo<IMCCEntity>, IMCCRepository
    {
        public override async Task DeleteByIDAsync(int? id)
        {
            if (id is null)
                throw new ArgumentNullException($"{nameof(id)} был NULL!");

            await using var connection = await GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = "DELETE FROM mcc WHERE code_value = $id";
            cmd.Parameters.AddWithValue("$id", id.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        private IMCCEntity ReadFromDB(SqliteDataReader reader)
        {
            var code = reader.GetInt32(0);                          // INTEGER PRIMARY KEY
            var name = reader.GetString(1);                         // NOT NULL
            // description в схеме nullable, но в IMCCEntity — non-nullable string.
            // Кладём null как в шаблоне; лучше сделать Description → string? в интерфейсе.
            var description = reader.IsDBNull(2) ? null! : reader.GetString(2);

            return new MCCEntityFromDB(name, description, code);
        }

        public override async Task<IMCCEntity> FindByIDAsync(int? id)
        {
            if (id is null)
                throw new ArgumentNullException($"{nameof(id)} был NULL!");

            await using var connection = await GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = "SELECT code_value, name, description FROM mcc WHERE code_value = $id";
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

            cmd.CommandText = "SELECT code_value, name, description FROM mcc";

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                yield return ReadFromDB(reader);
        }

        protected override string GetTableCreationCommand()
        {
            // Баг в исходном DDL: `code-value` — невалидный идентификатор (`-` = вычитание).
            // Меняем на snake_case `code_value`, как в остальных таблицах (bank_id, last_digits, ...).
            return @"CREATE TABLE IF NOT EXISTS mcc (
                code_value INTEGER PRIMARY KEY,
                name TEXT NOT NULL,
                description TEXT
            );";
        }

        public override async Task SaveAsync(IMCCEntity entity)
        {
            if (entity.Id is null)
                throw new ArgumentException(
                    "MCCEntity.Id (код MCC) должен быть задан!",
                    nameof(entity));

            await using var connection = await GetConnection();
            var cmd = connection.CreateCommand();

            // code_value — INTEGER PRIMARY KEY, значение задаёт вызывающий.
            // INSERT OR REPLACE покрывает и создание, и обновление одним запросом.
            cmd.CommandText =
                "INSERT OR REPLACE INTO mcc (code_value, name, description) " +
                "VALUES ($code, $name, $desc)";

            cmd.Parameters.AddWithValue("$code", entity.Id.Value);
            cmd.Parameters.AddWithValue("$name", entity.Name);
            cmd.Parameters.AddWithValue("$desc", (object?)entity.Description ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }
    }

    internal class MCCEntityFromDB : IMCCEntity
    {
        public MCCEntityFromDB(string name, string description, int? id)
        {
            Name = name;
            Description = description;
            Id = id;
        }

        public string Name { get; init; }
        public string Description { get; init; }
        public int? Id { get; init; }
    }
}