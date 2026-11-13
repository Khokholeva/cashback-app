namespace cashback_domain.Entities
{
    public interface IBankCardEntity : INotificationEntity<int?>
    {
        public int[] CashBackCategoriesIDs { get; set; }
        public int BankId { get; init; }
        public string AdditionalInfo { get; set; }
        public int? Limit { get; set; }
        public string LastDigits { get; set; }
        public string Expires { get; set; }
    }
}
