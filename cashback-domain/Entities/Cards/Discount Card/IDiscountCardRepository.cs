namespace cashback_domain.Entities
{
    public interface IDiscountCardRepository : ISqLiteDbRepository<IDiscountCardEntity>
    {
        IAsyncEnumerable<IDiscountCardEntity> GetAllShopCards(int? shopID);
    }
}
