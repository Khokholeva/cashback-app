using cashback_domain.Entities;

namespace cashback_infrastructure.DB
{
    internal class ProductTrackerRepository : IProductTrackerRepository
    {
        public string DbFilename { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }

        public Task DeleteByIDAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IProductTrackerEntity> FindByIDAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IProductTrackerEntity>> GetAllEntriesAsync()
        {
            throw new NotImplementedException();
        }

        public string GetTableCreationCommand()
        {
            return @"CREATE TABLE IF NOT EXISTS Products (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL,
                    additional TEXT,
                    urls TEXT,
                    shop-ids TEXT
                );";
        }

        public Task InitializeDBAsync()
        {
            return this.InitializeDBAsync<IProductTrackerEntity>();
        }

        public Task SaveAsync(IProductTrackerEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
