using cashback_domain.Entities;
using cashback_infrastructure.DB.LocalDB;

namespace cashback_infrastructure.DB
{
    internal class PromotionRepository : IPromotionRepository
    {
        public string DbFilename { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Task DeleteByIDAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IPromotionEntity> FindByIDAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IPromotionEntity>> GetAllEntriesAsync()
        {
            throw new NotImplementedException();
        }

        public string GetTableCreationCommand()
        {
            return @"CREATE TABLE IF NOT EXISTS Promocodes (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    discount-value INTEGER,
                    valid-until TEXT,
                    shop-id INTEGER
                );";
        }

        public Task InitializeDBAsync(string dataSource)
        {
            return this.InitializeSQliteDB(dataSource);
        }

        public Task SaveAsync(IPromotionEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
