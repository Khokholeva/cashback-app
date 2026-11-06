using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using cashback_domain.Entities.Bank_Card;
using cashback_domain.Entities.DB;
using Microsoft.Data.Sqlite;

namespace cashback_domain.Entities.Cards.Bank_Card
{
    public class BankCardRepository : AbstractSqliteDbRepository<IBankCardEntity>
    {
        public override string DbFilename { get; init; }

        public override Task DeleteByIDAsync(int id)
        {
            throw new NotImplementedException();
        }

        public override Task<IBankCardEntity> FindByIDAsync(int id)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<IBankCardEntity>> GetAllEntriesAsync()
        {
            throw new NotImplementedException();
        }

        public override Task SaveAsync(IBankCardEntity entity)
        {
            throw new NotImplementedException();
        }

        public override string GetTableCreationCommand()
        {
            return @"
                CREATE TABLE IF NOT EXISTS BankCards (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    mcc-categories TEXT,
                    bank-name TEXT NOT NULL,
                    additional TEXT
                );";
        }
    }
}
