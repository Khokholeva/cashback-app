namespace cashback_domain.Entities
{
    public class BankCardEntity : IBankCardEntity
    {
        public BankCardEntity(int bankId)
        {
            BankId = bankId;
            CashBackCategories = new Dictionary<int, int>();
        }

        public Dictionary<int, int> CashBackCategories { get; init; }
        public int BankId {  get; init; }
        public string AdditionalInfo { get; set; }
        public DateTime? NextNotification { get; set; }
        public int? Id { get; init; }
        public int? Limit { get; set; }
        public string LastDigits { get; set; }
        public string Expires { get; set; }
    }
}
