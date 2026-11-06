using System;
using System.Collections.Generic;
using System.Text;
using cashback_domain.Entities.DB;

namespace cashback_domain.Entities.MCC
{
    public interface IMCCRepository : ISqLiteDbRepository<IMCCEntity>
    {
    }
}
