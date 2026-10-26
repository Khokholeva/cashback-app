using System;
using System.Collections.Generic;
using System.Text;
using cashback_domain.Enums;

namespace cashback_domain.Entities.Bank_Card
{
    internal interface IBankCardEntity : IEntity<int>
    {
        public CashBackCategory[] CashBackCategories { get; init; }
        public Bank Bank { get; init; }
        public string AdditionalInfo { get; init; }
    }
}
