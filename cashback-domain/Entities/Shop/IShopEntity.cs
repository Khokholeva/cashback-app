using cashback_domain.Entities.Discount_Card;
using cashback_domain.Entities.MCC;
using cashback_domain.Enums;

namespace cashback_domain.Entities.Shop
{
    public interface IShopEntity : IEntity<int>
    {
        public IMCC MainMCC { get; init; }
        public IMCC[] PossibleMCCs { get; init; }
        public string Name { get; init; }
        public ShopCategory Category { get; init; }
        public IDiscountCardEntity[] DiscountCards { get; init; }
        public string[] Addresses { get; init; }
        public string BriefDescription { get; init; }
    }
}
