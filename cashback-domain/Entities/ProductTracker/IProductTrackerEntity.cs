namespace cashback_domain.Entities
{
    public interface IProductTrackerEntity : IEntity<int?>
    {
        public Dictionary<IShopEntity, (int Price, string URL)> PricesAtShops { get; init; }
        public DateTime PriceGotAt { get; init; }
    }
}
