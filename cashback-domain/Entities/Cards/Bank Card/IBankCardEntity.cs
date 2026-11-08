using System;
using System.Collections.Generic;
using System.Text;
using cashback_domain.Entities;
using cashback_domain.Entities.MCC;

namespace cashback_domain.Entities
{
    public interface IBankCardEntity : INotificationEntity<int>
    {
        public IMCCEntity[] CashBackCategories { get; init; }
        public int BankId { get; init; }
        public string AdditionalInfo { get; init; }
    }
}
