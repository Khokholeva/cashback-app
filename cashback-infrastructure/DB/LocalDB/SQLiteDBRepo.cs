using cashback_domain.Entities;
using Microsoft.Data.Sqlite;

namespace cashback_infrastructure.DB
{
    public abstract class SQLiteDBRepo<TEntity> : ISqLiteDbRepository<TEntity>
        where TEntity : IEntity<int?>
    {
        //private readonly string DbFilename;

        //public SQLiteDBRepo(string dbFilename)
        //{
        //    DbFilename = $"Data Source={dbFilename}";
        //}

        //public Task DeleteAsync<T>(T entity)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task DeleteByIDAsync<TID>(TID id)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<T> FindByIDAsync<T, TID>(TID id)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<IEnumerable<T>> GetAllEntriesAsync<T>()
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task InitializeDBAsync()
        //{
        //    using var connection = new SqliteConnection(DbFilename);
        //    await connection.OpenAsync();

        //    var createCmd = connection.CreateCommand();

        //    createCmd.CommandText = @"CREATE TABLE IF NOT EXISTS Shops (
        //        id INTEGER PRIMARY KEY AUTOINCREMENT,
        //        main-mcc INTEGER,
        //        mcc-list TEXT,
        //        category TEXT NOT NULL,
        //        discount-card-ids TEXT,
        //        addresses TEXT,
        //        brief-description TEXT);

        //        CREATE TABLE IF NOT EXISTS DiscountCards (
        //            id INTEGER PRIMARY KEY AUTOINCREMENT,
        //            barcode-value TEXT NOT NULL,
        //            barcode-type TEXT NOT NULL,
        //            shop-id INTEGER
        //        );

        //        CREATE TABLE IF NOT EXISTS BankCards (
        //            id INTEGER PRIMARY KEY AUTOINCREMENT,
        //            mcc-categories TEXT,
        //            bank-name TEXT NOT NULL,
        //            additional TEXT
        //        );

        //        CREATE TABLE IF NOT EXISTS Promocodes (
        //            id INTEGER PRIMARY KEY AUTOINCREMENT,
        //            discount-value INTEGER,
        //            valid-until TEXT,
        //            shop-id INTEGER
        //        );

        //        CREATE TABLE IF NOT EXISTS Products (
        //            id INTEGER PRIMARY KEY AUTOINCREMENT,
        //            name TEXT NOT NULL,
        //            additional TEXT,
        //            urls TEXT,
        //            shop-ids TEXT
        //        );
        //    ";
        //}

        //public Task SaveAsync<T>(T entity)
        //{
        //    throw new NotImplementedException();
        //}
        public string DbFilename { get; protected set; }

        public abstract Task DeleteByIDAsync(int? id);

        public abstract Task<TEntity> FindByIDAsync(int? id);

        public abstract IAsyncEnumerable<TEntity> GetAllEntriesAsync();

        protected abstract string GetTableCreationCommand();

        public async Task InitializeDBAsync(string dataSource)
        {
            DbFilename = $"Data source={dataSource}";

            using (var connection = await GetConnection())
            {
                var createCmd = connection.CreateCommand();

                createCmd.CommandText = GetTableCreationCommand();

                await createCmd.ExecuteNonQueryAsync();
            }
        }

        protected async Task<SqliteConnection> GetConnection()
        {
            var connection = new SqliteConnection(DbFilename);
            await connection.OpenAsync();

            using var pragma = connection.CreateCommand();
            pragma.CommandText = "PRAGMA foreign_keys = ON;";
            await pragma.ExecuteNonQueryAsync();

            return connection;
        }

        public abstract Task SaveAsync(TEntity entity);
    }
}
