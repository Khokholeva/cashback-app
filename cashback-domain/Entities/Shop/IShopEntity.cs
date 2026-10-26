using cashback_domain.Enums;

namespace cashback_domain.Entities.Shop
{
    internal interface IShopEntity : IEntity<int>
    {
        public int MainMCC { get; init; }
        public int[] PossibleMCCs { get; init; }
        public string Name { get; init; }
        public ShopCategory Category { get; init; }

    }
}
