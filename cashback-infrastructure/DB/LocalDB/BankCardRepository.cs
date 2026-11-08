using System;
using System.Collections.Generic;
using System.Text;
using cashback_domain.Entities;
using cashback_domain.Entities.Cards.Bank_Card;
using cashback_domain.Entities.DB;

namespace cashback_infrastructure.DB.LocalDB
{
    public class BankCardRepository : IBankCardRepository
    {
        public string DbFilename { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }

        public Task DeleteByIDAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IBankCardEntity> FindByIDAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IBankCardEntity>> GetAllEntriesAsync()
        {
            throw new NotImplementedException();
        }

        public string GetTableCreationCommand()
        {
            return @"
                CREATE TABLE IF NOT EXISTS BankCards (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    mcc-categories TEXT,
                    bank-name TEXT NOT NULL,
                    additional TEXT,
                    notification-date TEXT
                );";
        }

        public Task InitializeDBAsync()
        {
            return this.InitializeDBAsync<IBankCardEntity>();
        }

        public Task SaveAsync(IBankCardEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
