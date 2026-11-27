
using cashback_domain.Entities;
using Microsoft.Data.Sqlite;

namespace cashback_infrastructure.DB
{
    public class ShopLocalRepository : SQLiteDBRepo<IShopEntity>, IShopLocalRepository
    {
        private readonly IDiscountCardLocalRepository _discountCardRepository;
        private readonly IMCCLocalRepository _mccRepository;
        private IBankCardLocalRepository _bankCardRepository;

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

            var mainMccId = reader.GetInt32(2);
            var mainMcc = await _mccRepository.FindByIDAsync(mainMccId);

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

            var briefDescription = reader.IsDBNull(4)
                ? null
                : reader.GetString(4);

            var discountCards = new List<IDiscountCardEntity>();

            return new ShopEntityFromDB(
                mainMcc,
                possibleMccs.ToArray(),
                name,
                await _discountCardRepository.GetAllShopCards(id).ToArrayAsync(),
                briefDescription,
                id,
                await _bankCardRepository
                .GetAllWithCorrectMCCs(
                    possibleMccs.Select(mcc => mcc.Id.Value)
                    .ToArray()
                    ).ToArrayAsync()
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
                    SELECT id, name, main_mcc, mcc_list, brief_description
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
                    SELECT id, name, main_mcc, mcc_list, brief_description
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
                    main_mcc INTEGER NOT NULL,
                    mcc_list TEXT,
                    brief_description TEXT
                );

                CREATE INDEX IF NOT EXISTS ix_shops_main_mcc ON Shops (main_mcc);";
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
                            (name, main_mcc, mcc_list, brief_description)
                        VALUES
                            ($name, $main_mcc, $mcc_list, $brief_description)";
                }
                else
                {
                    cmd.CommandText = @"
                        UPDATE Shops
                        SET
                            name = $name,
                            main_mcc = $main_mcc,
                            mcc_list = $mcc_list,
                            brief_description = $brief_description
                        WHERE id = $id";

                    cmd.Parameters.AddWithValue("$id", entity.Id.Value);
                }

                if (entity.MainMCC.Id is null)
                    throw new InvalidOperationException(
                        "У MainMCC отсутствует ID!");

                var mccIds = entity.PossibleMCCs.Select(mcc =>
                    mcc.Id ?? throw new InvalidOperationException(
                        "У одного из PossibleMCCs отсутствует ID!"));

                var mccString = string.Join(";", mccIds);

                cmd.Parameters.AddWithValue("$name", entity.Name);
                cmd.Parameters.AddWithValue(
                    "$main_mcc",
                    entity.MainMCC.Id.Value);

                cmd.Parameters.AddWithValue(
                    "$mcc_list",
                    mccString.Length > 0 ? mccString : DBNull.Value);

                cmd.Parameters.AddWithValue(
                    "$brief_description",
                    (object?)entity.BriefDescription ?? DBNull.Value);

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }

    internal class ShopEntityFromDB : IShopEntity
    {
        public ShopEntityFromDB(
            IMCCEntity mainMcc,
            IMCCEntity[] possibleMccs,
            string name,
            IDiscountCardEntity[] discountCards,
            string briefDescription,
            int? id,
            IBankCardEntity[] bankCardsWithShopMCCs)
        {
            MainMCC = mainMcc;
            PossibleMCCs = possibleMccs;
            Name = name;
            DiscountCards = discountCards;
            BriefDescription = briefDescription;
            Id = id;
            BankCardsWithShopMCCs = bankCardsWithShopMCCs;
        }

        public IMCCEntity MainMCC { get; init; }

        public IMCCEntity[] PossibleMCCs { get; init; }

        public string Name { get; init; }

        public IDiscountCardEntity[] DiscountCards { get; init; }

        public string BriefDescription { get; init; }

        public int? Id { get; init; }

        public IBankCardEntity[] BankCardsWithShopMCCs { get; }
    }
}
