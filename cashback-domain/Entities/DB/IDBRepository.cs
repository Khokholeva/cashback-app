namespace cashback_domain.Entities
{
    public interface IDBRepository<TEntity, TID>
        where TEntity : IEntity<TID>
    {
        public IAsyncEnumerable<TEntity> GetAllEntriesAsync();

        public Task<TEntity> FindByIDAsync(TID id);

        public Task SaveAsync(TEntity entity);

        public Task DeleteByIDAsync(TID id);

        public Task InitializeDBAsync(string dataSource);

    }
}
