
using cashback_domain.Entities;
using cashback_domain.Enums;
using cashback_infrastructure.DB.LocalDB;
using Microsoft.Data.Sqlite;

namespace cashback_infrastructure.DB
{
    internal class PromotionRepository : SQLiteDBRepo<IPromotionEntity>, IPromotionRepository
    {
        public override async Task DeleteByIDAsync(int? id)
        {
            if (id is null)
                throw new ArgumentNullException($"{nameof(id)} был NULL!");

            using (var connection = await GetConnection())
            {
                var cmd = connection.CreateCommand();

                cmd.CommandText = "DELETE FROM Promocodes WHERE id = $id";
                cmd.Parameters.AddWithValue("$id", id.Value);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        private IPromotionEntity ReadFromDB(SqliteDataReader reader)
        {
            var id = reader.GetInt32(0);
            var discountValue = reader.GetDouble(1);
            var discountMeasurementUnit = (DiscountMeasurementUnit)reader.GetInt32(2);

            var shopId = reader.IsDBNull(3)
                ? (int?)null
                : reader.GetInt32(3);

            var validUntilString = reader.IsDBNull(4)
                ? null
                : reader.GetString(4);

            var validUntil = DateTime.TryParse(
                validUntilString,
                out var date)
                    ? date
                    : DateTime.MinValue;

            return new PromotionEntityFromDB(
                discountValue,
                discountMeasurementUnit,
                shopId,
                validUntil,
                id
            );
        }

        public override async Task<IPromotionEntity> FindByIDAsync(int? id)
        {
            if (id is null)
                throw new ArgumentNullException($"{nameof(id)} был NULL!");

            using (var connection = await GetConnection())
            {
                var cmd = connection.CreateCommand();

                cmd.CommandText = @"
                    SELECT id, discount_value, discount_measurement_unit, shop_id, valid_until
                    FROM Promocodes
                    WHERE id = $id";

                cmd.Parameters.AddWithValue("$id", id.Value);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                        return ReadFromDB(reader);

                    throw new KeyNotFoundException(
                        $"ID {id} не нашлось в базе Promocodes!");
                }
            }
        }

        public override async IAsyncEnumerable<IPromotionEntity> GetAllEntriesAsync()
        {
            using (var connection = await GetConnection())
            {
                var cmd = connection.CreateCommand();

                cmd.CommandText = @"
                    SELECT id, discount_value, discount_measurement_unit, shop_id, valid_until
                    FROM Promocodes";

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
                CREATE TABLE IF NOT EXISTS Promocodes (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    discount_value REAL NOT NULL,
                    discount_measurement_unit INTEGER NOT NULL,
                    valid_until TEXT NOT NULL,
                    shop_id INTEGER
                );

                CREATE INDEX IF NOT EXISTS ix_promocodes_shop ON Promocodes (shop_id);";
        }

        public override async Task SaveAsync(IPromotionEntity entity)
        {
            using (var connection = await GetConnection())
            {
                var cmd = connection.CreateCommand();

                if (entity.Id is null)
                {
                    cmd.CommandText = @"
                        INSERT INTO Promocodes
                            (discount_value, discount_measurement_unit, valid_until, shop_id)
                        VALUES
                            ($discount, $unit, $valid_until, $shop_id)";
                }
                else
                {
                    cmd.CommandText = @"
                        UPDATE Promocodes
                        SET
                            discount_value = $discount,
                            discount_measurement_unit = $unit,
                            valid_until = $valid_until,
                            shop_id = $shop_id
                        WHERE id = $id";

                    cmd.Parameters.AddWithValue("$id", entity.Id.Value);
                }

                cmd.Parameters.AddWithValue("$discount", entity.DiscountValue);
                cmd.Parameters.AddWithValue("$unit", (int)entity.DiscountMeasurementUnit);

                cmd.Parameters.AddWithValue(
                    "$valid_until",
                    DateTime.Now);

                cmd.Parameters.AddWithValue(
                    "$shop_id",
                    (object?)entity.ShopID ?? DBNull.Value);

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }

    internal class PromotionEntityFromDB : IPromotionEntity
    {
        public PromotionEntityFromDB(
            double discountValue,
            DiscountMeasurementUnit discountMeasurementUnit,
            int? shopId,
            DateTime validUntil,
            int? id)
        {
            DiscountValue = discountValue;
            DiscountMeasurementUnit = discountMeasurementUnit;
            ShopID = shopId;
            ValidUntil = validUntil;
            Id = id;
        }

        public double DiscountValue { get; init; }

        public DiscountMeasurementUnit DiscountMeasurementUnit { get; init; }

        public int? ShopID { get; init; }

        public DateTime ValidUntil { get; init; }

        public int? Id { get; init; }
    }
}
