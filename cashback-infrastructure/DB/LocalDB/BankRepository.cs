using cashback_domain.Entities;

namespace cashback_infrastructure.DB
{
    public class BankRepository : IBankRepository
    {
        public string DbFilename { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }

        public Task DeleteByIDAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IBankEntity> FindByIDAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IBankEntity>> GetAllEntriesAsync()
        {
            throw new NotImplementedException();
        }

        public string GetTableCreationCommand()
        {
            return @"CREATE TABLE IF NOT EXISTS Banks (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT NOT NULL,
                description TEXT
            );";
        }

        public Task InitializeDBAsync()
        {
            return this.InitializeDBAsync<IBankEntity>();
        }

        public Task SaveAsync(IBankEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
