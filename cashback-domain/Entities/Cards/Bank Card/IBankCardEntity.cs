namespace cashback_domain.Entities
{
    public interface IBankCardEntity : INotificationEntity<int>
    {
        public IMCCEntity[] CashBackCategories { get; init; }
        public int BankId { get; init; }
        public string AdditionalInfo { get; init; }
    }
}
