using cashback_domain.Entities;
using cashback_infrastructure.DB.LocalDB;

namespace cashback_infrastructure.DB
{
    internal class ShopRepository : IShopRepository
    {
        public string DbFilename { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Task DeleteByIDAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IShopEntity> FindByIDAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IShopEntity>> GetAllEntriesAsync()
        {
            throw new NotImplementedException();
        }

        public string GetTableCreationCommand()
        {
            return @"CREATE TABLE IF NOT EXISTS Shops (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                main-mcc INTEGER,
                mcc-list TEXT,
                category TEXT NOT NULL,
                discount-card-ids TEXT,
                addresses TEXT,
                brief-description TEXT);";
        }

        public Task InitializeDBAsync(string dataSource)
        {
            return this.InitializeSQliteDB(dataSource);
        }

        public Task SaveAsync(IShopEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
