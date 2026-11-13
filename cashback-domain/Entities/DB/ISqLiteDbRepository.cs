using Microsoft.Data.Sqlite;

namespace cashback_domain.Entities
{
    public interface ISqLiteDbRepository<TEntity> : IDBRepository<TEntity, int?>
        where TEntity : IEntity<int?>
    {
        public string DbFilename { get; set; }

        public string GetTableCreationCommand();

        
    }
}
