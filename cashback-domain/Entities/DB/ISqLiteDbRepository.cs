using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;

namespace cashback_domain.Entities.DB
{
    public interface ISqLiteDbRepository<TEntity> : IDBRepository<TEntity, int>
        where TEntity : IEntity<int>
    {
        public string DbFilename { get; init; }

        public string GetTableCreationCommand();

        
    }

    public static class ext
    {
        public static async Task InitializeDBAsync<T>(this ISqLiteDbRepository<T> repo)
            where T : IEntity<int>
        {
            using var connection = new SqliteConnection(repo.DbFilename);
            await connection.OpenAsync();

            var createCmd = connection.CreateCommand();

            createCmd.CommandText = repo.GetTableCreationCommand();

            await createCmd.ExecuteNonQueryAsync();
        }
    }
}
