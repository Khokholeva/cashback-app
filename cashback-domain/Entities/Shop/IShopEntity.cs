using cashback_domain.Entities.Discount_Card;
using cashback_domain.Enums;

namespace cashback_domain.Entities.Shop
{
    internal interface IShopEntity : IEntity<int>
    {
        public int MainMCC { get; init; }
        public int[] PossibleMCCs { get; init; }
        public string Name { get; init; }
        public ShopCategory Category { get; init; }
        public IDiscountCardEntity[] DiscountCards { get; init; }
        public string[] Addresses { get; init; }
        public string BriefDescription { get; init; }
    }
}
