using cashback_domain.Entities;
using cashback_infrastructure.DB.LocalDB;

namespace cashback_infrastructure.DB
{
    public class BankCardRepository : IBankCardRepository
    {
        private string dbFilename;
        public string DbFilename { get => dbFilename; set => dbFilename = $"Data source={value}"; }

        public async Task DeleteByIDAsync(int? id)
        {
            using (var connection = await this.GetConnection())
            {
                var cmd = connection.CreateCommand();

                cmd.CommandText = $"DELETE FROM BankCards WHERE id = {id}";

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<IBankCardEntity> FindByIDAsync(int? id)
        {
            using (var connection = await this.GetConnection())
            {
                var cmd = connection.CreateCommand();

                cmd.CommandText = $"SELECT * FROM BankCards WHERE id = {id}";

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var dbId = reader.GetInt32(0);
                        var mcc_categories = reader.GetString(1)
                            .Split(";")
                            .Select(c => { var success = int.TryParse(c, out var g); return (g, success); })
                            .Where(t => t.success)
                            .Select(t => t.g)
                            .ToArray();
                        var bankId = reader.GetInt32(2);
                        var additional = reader.GetString(3);
                        var lastDigits = reader.GetString(4);
                        var expires = reader.GetString(5);
                        var cardLimit = reader.GetInt32(6);
                        var notificationDate = reader.GetString(7);

                        return new BankCardEntityFromDB(mcc_categories, bankId, additional, cardLimit, lastDigits, expires, DateTime.Parse(notificationDate), dbId);
                    }

                    throw new KeyNotFoundException($"ID {id} не нашлось в базе BankCards!");
                    
                }
            }
        }

        public async IAsyncEnumerable<IBankCardEntity> GetAllEntriesAsync()
        {
            using (var connection = await this.GetConnection())
            {
                var cmd = connection.CreateCommand();

                cmd.CommandText = "SELECT * FROM BankCards";

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var id = reader.GetInt32(0);
                        var mcc_categories = reader.GetString(1)
                            .Split(";")
                            .Select(c => { var success = int.TryParse(c, out var g); return (g, success); })
                            .Where(t => t.success)
                            .Select(t => t.g)
                            .ToArray();
                        var bankId = reader.GetInt32(2);
                        var additional = reader.GetString(3);
                        var lastDigits = reader.GetString(4);
                        var expires = reader.GetString(5);
                        var cardLimit = reader.GetInt32(6);
                        var notificationDate = reader.GetString(7);

                        yield return new BankCardEntityFromDB(mcc_categories, bankId, additional, cardLimit, lastDigits, expires, DateTime.Parse(notificationDate), id);
                    }
                }
            }
        }

        public string GetTableCreationCommand()
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
                );";
        }

        public Task InitializeDBAsync(string dataSource)
        {
            return this.InitializeSQliteDB(dataSource);
        }

        public async Task SaveAsync(IBankCardEntity entity)
        {
            using (var connection = await this.GetConnection())
            {
                var cmd = connection.CreateCommand();
                var idString = entity.Id is not null ? "id, " : string.Empty;
                var idValueString = entity.Id is not null ? "$id, " : string.Empty;
                cmd.CommandText = $"INSERT INTO BankCards ({idString}mcc_categories, bank_id, additional, last_digits, expires, card_limit, notification_date) VALUES ({idValueString}$mcc, $bid, $add, $last, $exp, $card_limit, $not)";

                if (entity.Id is not null)
                    cmd.Parameters.AddWithValue("$id", entity.Id);
                cmd.Parameters.AddWithValue("$mcc", string.Join(";", entity.CashBackCategoriesIDs));
                cmd.Parameters.AddWithValue("$bid", entity.BankId);
                cmd.Parameters.AddWithValue("$add", entity.AdditionalInfo);
                cmd.Parameters.AddWithValue("$not", DateTime.Now.ToString());
                cmd.Parameters.AddWithValue("$last", entity.LastDigits);
                cmd.Parameters.AddWithValue("$exp", entity.Expires);
                cmd.Parameters.AddWithValue("$card_limit", entity.Limit.ToString() ?? "");

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }

    internal class BankCardEntityFromDB : IBankCardEntity
    {
        public BankCardEntityFromDB(
            int[] cashBackCategoriesIDs,
            int bankId,
            string additionalInfo,
            int? limit,
            string lastDigits,
            string expires,
            DateTime nextNotification,
            int? id)
        {
            CashBackCategoriesIDs = cashBackCategoriesIDs;
            BankId = bankId;
            AdditionalInfo = additionalInfo;
            Limit = limit;
            LastDigits = lastDigits;
            Expires = expires;
            NextNotification = nextNotification;
            Id = id;
        }

        public int[] CashBackCategoriesIDs { get; set; }
        public int BankId { get; init; }
        public string AdditionalInfo { get; set; }
        public int? Limit { get; set; }
        public string LastDigits { get; set; }
        public string Expires { get; set; }
        public DateTime NextNotification { get; set; }
        public int? Id { get; set; }
    }
}
