using cashback_domain.Entities;

namespace cashback_infrastructure.DB
{
    internal class PromotionRepository : IPromotionRepository
    {
        public string DbFilename { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }

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

        public Task InitializeDBAsync()
        {
            return this.InitializeDBAsync<IPromotionEntity>();
        }

        public Task SaveAsync(IPromotionEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
