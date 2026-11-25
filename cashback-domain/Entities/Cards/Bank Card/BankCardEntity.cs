namespace cashback_domain.Entities
{
    public class BankCardEntity : IBankCardEntity
    {
        public BankCardEntity(IBankEntity bank)
        {
            Bank = bank;
            CashBackCategories = new Dictionary<int, int>();
        }

        public Dictionary<int, int> CashBackCategories { get; init; }
        public IBankEntity Bank {  get; init; }
        public string? AdditionalInfo { get; set; }
        public DateTime? NextNotification { get; set; }
        public int? Id { get; init; }
        public int? Limit { get; set; }
        public string? LastDigits { get; set; }
        public string? Expires { get; set; }
    }
}
