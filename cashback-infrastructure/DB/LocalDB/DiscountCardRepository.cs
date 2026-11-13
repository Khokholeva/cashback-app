using cashback_domain.Entities;
using cashback_infrastructure.DB.LocalDB;

namespace cashback_infrastructure.DB
{
    public class DiscountCardRepository : IDiscountCardRepository
    {
        public string DbFilename { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Task DeleteByIDAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IDiscountCardEntity> FindByIDAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IDiscountCardEntity>> GetAllEntriesAsync()
        {
            throw new NotImplementedException();
        }

        public string GetTableCreationCommand()
        {
            return @"CREATE TABLE IF NOT EXISTS DiscountCards (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    barcode-value TEXT NOT NULL,
                    barcode-type TEXT NOT NULL,
                    shop-id INTEGER,
                    notification-date TEXT
                );";
        }

        public Task InitializeDBAsync(string dataSource)
        {
            return this.InitializeSQliteDB(dataSource);
        }

        public Task SaveAsync(IDiscountCardEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
