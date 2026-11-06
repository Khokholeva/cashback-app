using System;
using System.Collections.Generic;
using System.Text;
using cashback_domain.Entities.Cards.Discount_Card;
using cashback_domain.Entities.DB;
using cashback_domain.Entities.Discount_Card;

namespace cashback_infrastructure.DB.LocalDB
{
    public class DiscountCardRepository : IDiscountCardRepository
    {
        public string DbFilename { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }

        public Task DeleteByIDAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IDiscountCardEntity> FindByIDAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IDiscountCardEntity>> GetAllEntriesAsync()
        {
            throw new NotImplementedException();
        }

        public string GetTableCreationCommand()
        {
            return @"CREATE TABLE IF NOT EXISTS DiscountCards (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    barcode-value TEXT NOT NULL,
                    barcode-type TEXT NOT NULL,
                    shop-id INTEGER,
                    notification-date TEXT
                );";
        }

        public Task InitializeDBAsync()
        {
            return this.InitializeDBAsync<IDiscountCardEntity>();
        }

        public Task SaveAsync(IDiscountCardEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
