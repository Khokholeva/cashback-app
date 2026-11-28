
using cashback_domain.Entities;
using Microsoft.Data.Sqlite;

namespace cashback_infrastructure.DB
{
    public class ShopLocalRepository : SQLiteDBRepo<IShopEntity>, IShopLocalRepository
    {
        private readonly IDiscountCardLocalRepository _discountCardRepository;
        private readonly IMCCLocalRepository _mccRepository;
        private readonly IBankCardLocalRepository _bankCardRepository;

        public ShopLocalRepository(
            IDiscountCardLocalRepository discountCardRepository,
            IMCCLocalRepository mccRepository,
            IBankCardLocalRepository bankCardRepository)
        {
            _discountCardRepository = discountCardRepository;
            _mccRepository = mccRepository;
            _bankCardRepository = bankCardRepository;
        }

        private async Task<IShopEntity> ReadFromDB(SqliteDataReader reader)
        {
            var id = reader.GetInt32(0);
            var name = reader.GetString(1);

            var mainMccId = reader.IsDBNull(2)
                ? (int?)null
                : reader.GetInt32(2);

            var mainMcc = mainMccId is null
                ? null
                : await _mccRepository.FindByIDAsync(mainMccId);

            var possibleMccs = new List<IMCCEntity>();

            if (!reader.IsDBNull(3))
            {
                foreach (var mccString in reader.GetString(3).Split(';'))
                {
                    if (int.TryParse(mccString, out var mccId))
                    {
                        possibleMccs.Add(
                            await _mccRepository.FindByIDAsync(mccId));
                    }
                }
            }

            var discountCards = await _discountCardRepository
                .GetAllShopCards(id)
                .ToArrayAsync();

            var mccIds = possibleMccs.Select(mcc =>
                mcc.Id ?? throw new InvalidOperationException(
                    "У одного из PossibleMCCs отсутствует ID!"))
                .ToArray();

            var bankCards = await _bankCardRepository
                    .GetAllWithCorrectMCCs(mccIds)
                    .ToArrayAsync();

            return new ShopEntityFromDB(
                mainMcc,
                possibleMccs.ToArray(),
                name,
                discountCards,
                id,
                bankCards
            );
        }

        public override async Task DeleteByIDAsync(int? id)
        {
            if (id is null)
                throw new ArgumentNullException($"{nameof(id)} был NULL!");

            using (var connection = await GetConnection())
            {
                var cmd = connection.CreateCommand();

                cmd.CommandText = "DELETE FROM Shops WHERE id = $id";
                cmd.Parameters.AddWithValue("$id", id.Value);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public override async Task<IShopEntity> FindByIDAsync(int? id)
        {
            if (id is null)
                throw new ArgumentNullException($"{nameof(id)} был NULL!");

            using (var connection = await GetConnection())
            {
                var cmd = connection.CreateCommand();

                cmd.CommandText = @"
                    SELECT id, name, main_mcc, mcc_list
                    FROM Shops
                    WHERE id = $id";

                cmd.Parameters.AddWithValue("$id", id.Value);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                        return await ReadFromDB(reader);

                    throw new KeyNotFoundException(
                        $"ID {id} не нашлось в базе Shops!");
                }
            }
        }

        public override async IAsyncEnumerable<IShopEntity> GetAllEntriesAsync()
        {
            using (var connection = await GetConnection())
            {
                var cmd = connection.CreateCommand();

                cmd.CommandText = @"
                    SELECT id, name, main_mcc, mcc_list
                    FROM Shops";

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                        yield return await ReadFromDB(reader);
                }
            }
        }

        protected override string GetTableCreationCommand()
        {
            return @"
                CREATE TABLE IF NOT EXISTS Shops (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL,
                    main_mcc INTEGER,
                    mcc_list TEXT
                );

                CREATE INDEX IF NOT EXISTS ix_shops_main_mcc
                ON Shops (main_mcc);";
        }

        public override async Task SaveAsync(IShopEntity entity)
        {
            using (var connection = await GetConnection())
            {
                var cmd = connection.CreateCommand();

                if (entity.Id is null)
                {
                    cmd.CommandText = @"
                        INSERT INTO Shops
                            (name, main_mcc, mcc_list)
                        VALUES
                            ($name, $main_mcc, $mcc_list)";
                }
                else
                {
                    cmd.CommandText = @"
                        UPDATE Shops
                        SET
                            name = $name,
                            main_mcc = $main_mcc,
                            mcc_list = $mcc_list
                        WHERE id = $id";

                    cmd.Parameters.AddWithValue("$id", entity.Id.Value);
                }

                if (entity.MainMCC is not null && entity.MainMCC.Id is null)
                    throw new InvalidOperationException(
                        "У MainMCC отсутствует ID!");

                var mccIds = entity.PossibleMCCs.Select(mcc =>
                    mcc.Id ?? throw new InvalidOperationException(
                        "У одного из PossibleMCCs отсутствует ID!"));

                var mccString = string.Join(";", mccIds);

                cmd.Parameters.AddWithValue("$name", entity.Name);

                cmd.Parameters.AddWithValue(
                    "$main_mcc",
                    (object?)entity.MainMCC?.Id ?? DBNull.Value);

                cmd.Parameters.AddWithValue(
                    "$mcc_list",
                    mccString.Length > 0 ? mccString : DBNull.Value);

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }

    internal class ShopEntityFromDB : IShopEntity
    {
        public ShopEntityFromDB(
            IMCCEntity? mainMcc,
            IMCCEntity[] possibleMccs,
            string name,
            IDiscountCardEntity[] discountCards,
            int? id,
            IBankCardEntity[] bankCardsWithShopMCCs)
        {
            MainMCC = mainMcc;
            PossibleMCCs = possibleMccs;
            Name = name;
            DiscountCards = discountCards;
            Id = id;
            BankCardsWithShopMCCs = bankCardsWithShopMCCs;
        }

        public IMCCEntity? MainMCC { get; init; }

        public IMCCEntity[] PossibleMCCs { get; init; }

        public string Name { get; init; }

        public IDiscountCardEntity[] DiscountCards { get; init; }

        public int? Id { get; init; }

        public IBankCardEntity[] BankCardsWithShopMCCs { get; }
    }
}
