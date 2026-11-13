using System;
using System.Collections.Generic;
using System.Text;
using cashback_domain.Entities;
using Microsoft.Data.Sqlite;

namespace cashback_infrastructure.DB.LocalDB
{
    public static class Extensions
    {
        public static async Task InitializeSQliteDB<T>(this ISqLiteDbRepository<T> repo, string dataSource)
            where T : IEntity<int?>
        {
            repo.DbFilename = dataSource;

            using (var connection = await repo.GetConnection())
            {
                var createCmd = connection.CreateCommand();

                createCmd.CommandText = repo.GetTableCreationCommand();

                await createCmd.ExecuteNonQueryAsync();
            }
        }

        public static async Task<SqliteConnection> GetConnection<T>(this ISqLiteDbRepository<T> repo)
            where T : IEntity<int?>
        {
            var connection = new SqliteConnection(repo.DbFilename);
            await connection.OpenAsync();

            return connection;
        }
    }
}
