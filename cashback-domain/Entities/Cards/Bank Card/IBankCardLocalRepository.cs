namespace cashback_domain.Entities
{
    public interface IBankCardLocalRepository : ISqLiteDbRepository<IBankCardEntity>
    {
        IAsyncEnumerable<IBankCardEntity> GetAllWithCorrectMCCs(int[] mcc);
    }
}
