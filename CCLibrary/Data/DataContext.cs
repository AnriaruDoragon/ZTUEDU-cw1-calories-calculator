using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CCLibrary.Data
{
    public partial class DataContext : DbContext
    {
        private readonly SqliteConnection _connection;

        public DataContext(SqliteConnection connection)
        {
            _connection = connection;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite(_connection.DataSource);
    }
}
