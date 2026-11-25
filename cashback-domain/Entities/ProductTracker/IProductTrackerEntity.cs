namespace cashback_domain.Entities
{
    public interface IProductTrackerEntity : IEntity<int?>
    {
        public Dictionary<int, (int Price, string URL)> PricesAtShops { get; init; }
        public DateTime? PriceGotAt { get; init; }
        string Name { get; init; }
        string? AdditionalInfo { get; set; }
    }
}
