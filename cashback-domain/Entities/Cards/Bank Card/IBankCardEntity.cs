using System;
using System.Collections.Generic;
using System.Text;
using cashback_domain.Entities.MCC;
using cashback_domain.Enums;

namespace cashback_domain.Entities.Bank_Card
{
    public interface IBankCardEntity : INotificationEntity<int>
    {
        public IMCCEntity[] CashBackCategories { get; init; }
        public Bank Bank { get; init; }
        public string AdditionalInfo { get; init; }
    }
}
