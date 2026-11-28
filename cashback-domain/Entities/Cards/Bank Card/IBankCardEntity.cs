namespace cashback_domain.Entities
{
    public interface IBankCardEntity : INotificationEntity<int?>
    {
        public Dictionary<int, int> CashBackCategories { get; }
        public IBankEntity Bank { get; }
        public string? AdditionalInfo { get; }
        public int? Limit { get; }
        public string? LastDigits { get; }
        public string? Expires { get; }
        public string BackGroundColor { get; }
        public string Name { get; }
    }
}
