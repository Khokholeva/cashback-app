using System;
using System.Collections.Generic;
using System.Text;
using cashback_domain.Entities.DB;
using cashback_domain.Entities.MCC;

namespace cashback_infrastructure.DB.LocalDB
{
    public class MCCRepository : IMCCRepository
    {
        public string DbFilename { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }

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

        public Task InitializeDBAsync()
        {
            return this.InitializeDBAsync<IMCCEntity>(); 
        }

        public Task SaveAsync(IMCCEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
