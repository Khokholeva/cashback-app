using System;
using System.Collections.Generic;
using System.Text;
using cashback_domain.Entities.Bank_Card;
using cashback_domain.Entities.MCC;
using cashback_domain.Enums;

namespace cashback_domain.Entities.Cards.Bank_Card
{
    public class BankCardEntity : IBankCardEntity
    {
        public IMCCEntity[] CashBackCategories { get; init; }
        public Bank Bank { get; init; }
        public string AdditionalInfo { get; init; }
        public DateTime NextNotification { get; set; }
        public int Id { get; set; }

        public void SaveToDB(DB.IDBRepository dBRepository)
        {
            throw new NotImplementedException();
        }
    }
}
