using System;
using System.Collections.Generic;
using System.Text;
using cashback_domain.Entities.Bank_Card;
using cashback_domain.Entities.DB;
using cashback_domain.Entities.Discount_Card;

namespace cashback_domain.Entities
{
    public interface IDiscountCardRepository : ISqLiteDbRepository<IDiscountCardEntity>
    {
    }
}
