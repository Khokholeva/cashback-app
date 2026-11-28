using cashback_domain.Enums;

namespace cashback_domain.Entities
{
    public interface IShopEntity : IEntity<int?>
    {
        public IMCCEntity? MainMCC { get; }
        public IMCCEntity[] PossibleMCCs { get; }
        public string Name { get; }
        public IDiscountCardEntity[] DiscountCards { get; }
        public IBankCardEntity[] BankCardsWithShopMCCs { get; }
    }
}
