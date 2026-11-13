using cashback_domain.Entities;
using cashback_infrastructure.DB.LocalDB;

namespace cashback_infrastructure.DB
{
    public class MCCRepository : IMCCRepository
    {
        public string DbFilename { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Task DeleteByIDAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IMCCEntity> FindByIDAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IMCCEntity>> GetAllEntriesAsync()
        {
            throw new NotImplementedException();
        }

        public string GetTableCreationCommand()
        {
            return @"CREATE TABLE IF NOT EXISTS mcc (
                code-value INTEGER PRIMARY KEY,
                name TEXT NOT NULL,
                description TEXT
            )";
        }

        public Task InitializeDBAsync(string dataSource)
        {
            return this.InitializeSQliteDB(dataSource); 
        }

        public Task SaveAsync(IMCCEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
