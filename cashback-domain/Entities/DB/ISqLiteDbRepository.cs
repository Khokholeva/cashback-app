using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;

namespace cashback_domain.Entities.DB
{
    public interface ISqLiteDbRepository<TEntity> : IDBRepository<TEntity, int>
        where TEntity : IEntity<int>
    {
        string DbFilename { get; init; }

        string GetTableCreationCommand();

        
    }
}
