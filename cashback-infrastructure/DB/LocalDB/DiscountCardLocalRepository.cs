using cashback_domain.Entities;
using cashback_domain.Entities.Cards.Discount_Card;
using Microsoft.Data.Sqlite;
using System.Globalization;
using ZXing.Net.Maui;

namespace cashback_infrastructure.DB
{
    public class DiscountCardLocalRepository : SQLiteDBRepo<IDiscountCardEntity>, IDiscountCardLocalRepository
    {
        private readonly IShopLocalRepository _shopRepository;

        public DiscountCardLocalRepository(IShopLocalRepository shopRepository)
        {
            _shopRepository = shopRepository;
        }

        public override async Task DeleteByIDAsync(int? id)
        {
            if (id is null)
                throw new ArgumentNullException($"{nameof(id)} был NULL!");

            await using var connection = await GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = "DELETE FROM DiscountCards WHERE id = $id";
            cmd.Parameters.AddWithValue("$id", id.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        private IDiscountCardEntity ReadFromDB(SqliteDataReader reader)
        {
            var id = reader.GetInt32(0);
            var barcodeValue = reader.IsDBNull(1) ? null : reader.GetString(1);
            var barcodeTypeName = reader.IsDBNull(2) ? null : reader.GetString(2);
            var shopId = reader.GetInt32(3);
            var barcode = barcodeValue is null ?
                null :
                new BarcodeFromDB(
                    barcodeValue,
                    barcodeTypeName
                    );

            return new DiscountCardEntityFromDB(
                barcode,
                _shopRepository.FindByIDAsync(shopId).GetAwaiter().GetResult(),
                DateTime.TryParse(
                reader.GetString(4),
                CultureInfo.InvariantCulture,
                DateTimeStyles.RoundtripKind,
                out var res) ? res : null,
                id);
        }

        public override async Task<IDiscountCardEntity> FindByIDAsync(int? id)
        {
            if (id is null)
                throw new ArgumentNullException($"{nameof(id)} был NULL!");

            await using var connection = await GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText =
                "SELECT id, barcode_value, barcode_type, shop_id, notification_date " +
                "FROM DiscountCards WHERE id = $id";
            cmd.Parameters.AddWithValue("$id", id.Value);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return ReadFromDB(reader);

            throw new KeyNotFoundException($"ID {id} не нашлось в базе DiscountCards!");
        }

        public override async IAsyncEnumerable<IDiscountCardEntity> GetAllEntriesAsync()
        {
            await using var connection = await GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText =
                "SELECT id, barcode_value, barcode_type, shop_id, notification_date " +
                "FROM DiscountCards";

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                yield return ReadFromDB(reader);
        }

        protected override string GetTableCreationCommand()
        {
            return @"
                CREATE TABLE IF NOT EXISTS DiscountCards (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    barcode_value TEXT,
                    barcode_type TEXT,
                    shop_id INTEGER NOT NULL,
                    notification_date TEXT
                );

                CREATE INDEX IF NOT EXISTS ix_discountcards_shop ON DiscountCards (shop_id);";
        }

        public override async Task SaveAsync(IDiscountCardEntity entity)
        {
            await using var connection = await GetConnection();
            var cmd = connection.CreateCommand();

            if (entity.Id is null)
            {
                cmd.CommandText =
                    "INSERT INTO DiscountCards (barcode_value, barcode_type, shop_id, notification_date) " +
                    "VALUES ($barcode_value, $barcode_type, $shop_id, $not)";
            }
            else
            {
                cmd.CommandText =
                    "UPDATE DiscountCards " +
                    "SET barcode_value = $barcode_value, barcode_type = $barcode_type, " +
                    "shop_id = $shop_id, notification_date = $not " +
                    "WHERE id = $id";
                cmd.Parameters.AddWithValue("$id", entity.Id.Value);
            }

            var barcodeValue = entity.Barcode is null ? DBNull.Value : (object?)entity.Barcode.Value;
            var barcodeFormat = entity.Barcode is null ? DBNull.Value : (object?)entity.Barcode.Format.ToString();

            cmd.Parameters.AddWithValue("$barcode_value", barcodeValue);
            cmd.Parameters.AddWithValue("$barcode_type", barcodeFormat);
            cmd.Parameters.AddWithValue("$shop_id", entity.Shop.Id);
            cmd.Parameters.AddWithValue(
                "$not",
                DateTime.Now);

            await cmd.ExecuteNonQueryAsync();
        }

        public async IAsyncEnumerable<IDiscountCardEntity> GetAllShopCards(int? shopID)
        {
            if (shopID is null)
                yield break;

            await using var connection = await GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText =
                "SELECT id, barcode_value, barcode_type, shop_id, notification_date " +
                "FROM DiscountCards WHERE shop_id = $shopID";

            cmd.Parameters.AddWithValue("$shopID", shopID.Value);

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
                yield return ReadFromDB(reader);
        }
    }

    internal class DiscountCardEntityFromDB : IDiscountCardEntity
    {
        public DiscountCardEntityFromDB(
            BarcodeFromDB barcode,
            IShopEntity shop,
            DateTime? nextNotification,
            int? id)
        {
            Barcode = barcode;
            Shop = shop;
            NextNotification = nextNotification;
            Id = id;
        }

        public int? Id { get; init; }
        public IBarcode? Barcode { get; set; }
        public IShopEntity Shop { get; init; }
        public DateTime? NextNotification { get; set; }
    }

    internal class BarcodeFromDB : IBarcode
    {
        public BarcodeFromDB(string format, string value)
        {
            Format = format;
            Value = value;
        }

        public string Format { get; init; }
        public string Value { get; init; }
    }
}