using cashback_domain.Entities;
using cashback_infrastructure.DB.LocalDB;
using Microsoft.Data.Sqlite;

namespace cashback_infrastructure.DB
{
    public class BankCardRepository : SQLiteDBRepo<IBankCardEntity>, IBankCardRepository
    {

        public override async Task DeleteByIDAsync(int? id)
        {
            if (id is null)
                throw new ArgumentNullException($"{nameof(id)} был NULL!");
            using (var connection = await GetConnection())
            {
                var cmd = connection.CreateCommand();

                cmd.CommandText = $"DELETE FROM BankCards WHERE id = {id}";

                await cmd.ExecuteNonQueryAsync();
            }
        }

        private IBankCardEntity ReadFromDB(SqliteDataReader reader)
        {
            var id = reader.GetInt32(0);
            var mcc_categories = reader.IsDBNull(1) ? new Dictionary<int, int>() :
                reader.GetString(1)
                .Split(";")
                .Select(v => { var idAndCashback = v.Split("^"); return (idAndCashback[0], idAndCashback[1]); })
                .Select(c =>
                {
                    var success1 = int.TryParse(c.Item1, out var mcc);
                    var success2 = int.TryParse(c.Item2, out var cashback);
                    return (mcc, cashback, success1 && success2);
                })
                .Where(t => t.Item3)
                .ToDictionary(t => t.mcc, t => t.cashback);
            var bankId = reader.GetInt32(2);
            var additional = reader.IsDBNull(3) ? null : reader.GetString(3);
            var lastDigits = reader.IsDBNull(4) ? null : reader.GetString(4);
            var expires = reader.IsDBNull(5) ? null : reader.GetString(5);
            var cardLimit = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6);
            var notificationDate = reader.IsDBNull(7) ? null : reader.GetString(7);

            return new BankCardEntityFromDB(
                mcc_categories,
                bankId,
                additional,
                cardLimit,
                lastDigits,
                expires,
                DateTime.TryParse(notificationDate, out var res) ? res : null,
                id);
        }

        public override async Task<IBankCardEntity> FindByIDAsync(int? id)
        {
            if (id is null)
                throw new ArgumentNullException($"{nameof(id)} был NULL!");
            using (var connection = await GetConnection())
            {
                var cmd = connection.CreateCommand();

                cmd.CommandText = $"SELECT id, mcc_categories, bank_id, additional, last_digits, expires, card_limit, notification_date FROM BankCards WHERE id = {id}";

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                        return ReadFromDB(reader);

                    throw new KeyNotFoundException($"ID {id} не нашлось в базе BankCards!");
                }
            }
        }

        public override async IAsyncEnumerable<IBankCardEntity> GetAllEntriesAsync()
        {
            using (var connection = await GetConnection())
            {
                var cmd = connection.CreateCommand();

                cmd.CommandText = "SELECT id, mcc_categories, bank_id, additional, last_digits, expires, card_limit, notification_date FROM BankCards";

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
                CREATE TABLE IF NOT EXISTS BankCards (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    mcc_categories TEXT,
                    bank_id INTEGER NOT NULL,
                    additional TEXT,
                    last_digits TEXT,
                    expires TEXT,
                    card_limit INTEGER,
                    notification_date TEXT
                );

                CREATE INDEX IF NOT EXISTS ix_bankcards_bank ON BankCards (bank_id);";
        }

        public override async Task SaveAsync(IBankCardEntity entity)
        {
            using (var connection = await GetConnection())
            {
                var cmd = connection.CreateCommand();
                var idString = entity.Id is not null ? "id, " : string.Empty;
                var idValueString = entity.Id is not null ? "$id, " : string.Empty;
                if (entity.Id is null)
                    cmd.CommandText =
                        $"INSERT INTO BankCards (mcc_categories, bank_id, additional, last_digits, expires, card_limit, notification_date) " +
                        $"VALUES ($mcc, $bid, $add, $last, $exp, $card_limit, $not)";
                else
                    cmd.CommandText =
                        $"UPDATE BankCards " +
                        $"SET mcc_categories = $mcc, bank_id = $bid, additional = $add, last_digits = $last, " +
                        $"expires = $exp, card_limit = $card_limit, notification_date = $not " +
                        $"WHERE id = $id";

                if (entity.Id is not null)
                    cmd.Parameters.AddWithValue("$id", entity.Id);
                var mccString = string.Join(";", entity.CashBackCategories?.Select(kv => $"{kv.Key}^{kv.Value}"));
                cmd.Parameters.AddWithValue("$mcc", mccString.Length > 0 ? mccString : DBNull.Value);
                cmd.Parameters.AddWithValue("$bid", entity.BankId);
                cmd.Parameters.AddWithValue("$add", (object?)entity.AdditionalInfo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("$not", DateTime.Now.ToString());
                cmd.Parameters.AddWithValue("$last", (object?)entity.LastDigits ?? DBNull.Value);
                cmd.Parameters.AddWithValue("$exp", (object?)entity.Expires ?? DBNull.Value);
                cmd.Parameters.AddWithValue("$card_limit", (object?)entity.Limit ?? DBNull.Value);

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }

    internal class BankCardEntityFromDB : IBankCardEntity
    {
        public BankCardEntityFromDB(
            Dictionary<int, int> cashBackCategories,
            int bankId,
            string additionalInfo,
            int? limit,
            string lastDigits,
            string expires,
            DateTime? nextNotification,
            int? id)
        {
            CashBackCategories = cashBackCategories;
            BankId = bankId;
            AdditionalInfo = additionalInfo;
            Limit = limit;
            LastDigits = lastDigits;
            Expires = expires;
            NextNotification = nextNotification;
            Id = id;
        }

        public Dictionary<int, int> CashBackCategories { get; init; }
        public int BankId { get; init; }
        public string AdditionalInfo { get; set; }
        public int? Limit { get; set; }
        public string LastDigits { get; set; }
        public string Expires { get; set; }
        public DateTime? NextNotification { get; set; }
        public int? Id { get; init; }
    }
}
