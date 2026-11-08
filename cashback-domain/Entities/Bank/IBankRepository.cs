using System;
using System.Collections.Generic;
using System.Text;
using cashback_domain.Entities.DB;

namespace cashback_domain.Entities.Bank
{
    public interface IBankRepository : ISqLiteDbRepository<IBankEntity>
    {
    }
}
