namespace cashback_domain.Entities
{
    public interface IDiscountCardLocalRepository : ISqLiteDbRepository<IDiscountCardEntity>
    {
        IAsyncEnumerable<IDiscountCardEntity> GetAllShopCards(int? shopID);
    }
}
