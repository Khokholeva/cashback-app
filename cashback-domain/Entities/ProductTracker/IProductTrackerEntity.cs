namespace cashback_domain.Entities
{
    public interface IProductTrackerEntity : IEntity<int?>
    {
        public Dictionary<int, (int Price, string URL)> PricesAtShops { get; }
        public DateTime? PriceGotAt { get; }
        string Name { get; }
        string? AdditionalInfo { get; }
    }
}
