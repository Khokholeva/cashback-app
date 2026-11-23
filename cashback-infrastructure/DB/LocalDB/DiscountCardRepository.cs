using cashback_domain.Entities;
using cashback_infrastructure.DB.LocalDB;
using Microsoft.Data.Sqlite;
using System.Globalization;
using ZXing.Net.Maui;

namespace cashback_infrastructure.DB
{
    public class DiscountCardRepository : SQLiteDBRepo<IDiscountCardEntity>, IDiscountCardRepository
    {
        private static readonly Dictionary<string, BarcodeFormat> StringToBarcodeType;

        static DiscountCardRepository()
        {
            StringToBarcodeType = typeof(BarcodeFormat)
                .GetFields()
                .Where(field => field.IsLiteral)
                .ToDictionary(
                    field => field.Name,
                    field => (BarcodeFormat)field.GetRawConstantValue());
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
            var barcodeValue = reader.GetString(1);              // NOT NULL
            var barcodeTypeName = reader.GetString(2);           // NOT NULL
            var shopId = reader.GetInt32(3);                     // NOT NULL

            // BarcodeResult — record с init-свойствами.
            // Конструктора с аргументами нет, поэтому только инициализатор.
            var barcode = new BarcodeResult
            {
                Value = barcodeValue,
                Format = StringToBarcodeType.TryGetValue(barcodeTypeName, out var fmt)
                    ? fmt
                    : throw new KeyNotFoundException($"Тип штрихкода {barcodeTypeName} не существует!"),
            };

            return new DiscountCardEntityFromDB(
                barcode, 
                shopId, 
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
                    barcode_value TEXT NOT NULL,
                    barcode_type TEXT NOT NULL,
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

            cmd.Parameters.AddWithValue("$barcode_value", entity.Barcode.Value);
            cmd.Parameters.AddWithValue("$barcode_type", entity.Barcode.Format.ToString());
            cmd.Parameters.AddWithValue("$shop_id", entity.ShopID);
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
            BarcodeResult barcode,
            int shopId,
            DateTime? nextNotification,
            int? id)
        {
            Barcode = barcode;
            ShopID = shopId;
            NextNotification = nextNotification;
            Id = id;
        }

        public int? Id { get; init; }
        public BarcodeResult Barcode { get; init; }
        public int ShopID { get; init; }
        public DateTime? NextNotification { get; set; }
    }
}