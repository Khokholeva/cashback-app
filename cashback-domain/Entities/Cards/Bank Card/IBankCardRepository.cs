namespace cashback_domain.Entities
{
    public interface IBankCardRepository : ISqLiteDbRepository<IBankCardEntity>
    {
        IAsyncEnumerable<IBankCardEntity> GetAllWithCorrectMCCs(int[] mcc);
    }
}
