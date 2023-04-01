using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CCLibrary.Data
{
    public partial class DataContext : DbContext
    {
        private readonly SqliteConnection? _connection;

        public DataContext() { }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite(Data.Database.DbSource);
    }
}
