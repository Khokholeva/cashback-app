using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using cashback_domain.Entities.Bank_Card;
using cashback_domain.Entities.DB;
using Microsoft.Data.Sqlite;

namespace cashback_domain.Entities.Cards.Bank_Card
{
    public interface IBankCardRepository : ISqLiteDbRepository<IBankCardEntity>
    {
    }
}
