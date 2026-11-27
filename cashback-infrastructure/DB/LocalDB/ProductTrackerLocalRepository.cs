
using cashback_domain.Entities;
using Microsoft.Data.Sqlite;

namespace cashback_infrastructure.DB
{
    internal class ProductTrackerLocalRepository
        : SQLiteDBRepo<IProductTrackerEntity>, IProductTrackerLocalRepository
    {
        public override async Task DeleteByIDAsync(int? id)
        {
            if (id is null)
                throw new ArgumentNullException($"{nameof(id)} был NULL!");

            using (var connection = await GetConnection())
            {
                var cmd = connection.CreateCommand();

                cmd.CommandText = "DELETE FROM Products WHERE id = $id";
                cmd.Parameters.AddWithValue("$id", id.Value);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        private IProductTrackerEntity ReadFromDB(SqliteDataReader reader)
        {
            var id = reader.GetInt32(0);

            var name = reader.GetString(1);

            var additional = reader.IsDBNull(2)
                ? null
                : reader.GetString(2);

            var pricesAtShops = new Dictionary<int, (int Price, string URL)>();

            if (!reader.IsDBNull(3))
            {
                var pricesString = reader.GetString(3);

                foreach (var entry in pricesString.Split(';'))
                {
                    var parts = entry.Split('^', 3);

                    if (parts.Length != 3)
                        continue;

                    if (int.TryParse(parts[0], out var shopId) &&
                        int.TryParse(parts[1], out var price))
                    {
                        pricesAtShops[shopId] = (price, parts[2]);
                    }
                }
            }

            var priceGotAtString = reader.IsDBNull(4)
                ? null
                : reader.GetString(4);

            var priceGotAt = DateTime.TryParse(
                priceGotAtString,
                out var date)
                    ? date
                    : DateTime.MinValue;

            return new ProductTrackerEntityFromDB(
                name,
                additional,
                pricesAtShops,
                priceGotAt,
                id
            );
        }

        public override async Task<IProductTrackerEntity> FindByIDAsync(int? id)
        {
            if (id is null)
                throw new ArgumentNullException($"{nameof(id)} был NULL!");

            using (var connection = await GetConnection())
            {
                var cmd = connection.CreateCommand();

                cmd.CommandText = @"
                    SELECT id, name, additional, prices_at_shops, price_got_at
                    FROM Products
                    WHERE id = $id";

                cmd.Parameters.AddWithValue("$id", id.Value);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                        return ReadFromDB(reader);

                    throw new KeyNotFoundException(
                        $"ID {id} не нашлось в базе Products!");
                }
            }
        }

        public override async IAsyncEnumerable<IProductTrackerEntity> GetAllEntriesAsync()
        {
            using (var connection = await GetConnection())
            {
                var cmd = connection.CreateCommand();

                cmd.CommandText = @"
                    SELECT id, name, additional, prices_at_shops, price_got_at
                    FROM Products";

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                        yield return ReadFromDB(reader);
                }
            }
        }

        protected override string GetTableCreationCommand()
        {
            return @"
                CREATE TABLE IF NOT EXISTS Products (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL,
                    additional TEXT,
                    prices_at_shops TEXT,
                    price_got_at TEXT
                );";
        }

        public override async Task SaveAsync(IProductTrackerEntity entity)
        {
            using (var connection = await GetConnection())
            {
                var cmd = connection.CreateCommand();

                if (entity.Id is null)
                {
                    cmd.CommandText = @"
                        INSERT INTO Products
                            (name, additional, prices_at_shops, price_got_at)
                        VALUES
                            ($name, $additional, $prices, $price_got_at)";
                }
                else
                {
                    cmd.CommandText = @"
                        UPDATE Products
                        SET
                            name = $name,
                            additional = $additional,
                            prices_at_shops = $prices,
                            price_got_at = $price_got_at
                        WHERE id = $id";

                    cmd.Parameters.AddWithValue("$id", entity.Id.Value);
                }

                cmd.Parameters.AddWithValue("$name", entity.Name);
                cmd.Parameters.AddWithValue(
                    "$additional",
                    (object?)entity.AdditionalInfo ?? DBNull.Value);

                var pricesString = string.Join(";",
                    entity.PricesAtShops.Select(kv =>
                        $"{kv.Key}^{kv.Value.Price}^{kv.Value.URL}"));

                cmd.Parameters.AddWithValue(
                    "$prices",
                    pricesString.Length > 0 ? pricesString : DBNull.Value);

                cmd.Parameters.AddWithValue(
                    "$price_got_at",
                    DateTime.Now);

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }

    internal class ProductTrackerEntityFromDB : IProductTrackerEntity
    {
        public ProductTrackerEntityFromDB(
            string name,
            string additionalInfo,
            Dictionary<int, (int Price, string URL)> pricesAtShops,
            DateTime priceGotAt,
            int? id)
        {
            Name = name;
            AdditionalInfo = additionalInfo;
            PricesAtShops = pricesAtShops;
            PriceGotAt = priceGotAt;
            Id = id;
        }

        public string Name { get; init; }

        public string? AdditionalInfo { get; set; }

        public Dictionary<int, (int Price, string URL)> PricesAtShops { get; init; }

        public DateTime? PriceGotAt { get; init; }

        public int? Id { get; init; }
    }
}
