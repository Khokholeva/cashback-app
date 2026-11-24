using System;
using System.Collections.Generic;
using System.Text;
using cashback_domain.Entities;

namespace cashback_application.Factories
{
    public interface IBankCardFactory
    {
        BankCardEntity CreateBankCardEntity(int bankId);
    }
}
