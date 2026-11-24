using cashback_domain.Enums;

namespace cashback_domain.Entities
{
    public interface IShopEntity : IEntity<int?>
    {
        public IMCCEntity MainMCC { get; init; }
        public IMCCEntity[] PossibleMCCs { get; init; }
        public string Name { get; init; }
        public IDiscountCardEntity[] DiscountCards { get; init; }
        public string BriefDescription { get; init; }
        public IBankCardEntity[] BankCardsWithShopMCCs { get; }
    }
}
