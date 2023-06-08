using Microsoft.EntityFrameworkCore;
using DataBase.Abstraction.Model;

namespace DataBase.Abstraction
{
    public class Context : DbContext
    {
        private readonly string _connectionString;
        public Context(string connectionString)
        {
            _connectionString = connectionString;
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_connectionString}");
        }

        public DbSet<BrainDb> Brains { get; set; }

        public DbSet<SimulationDb> Simulations { get; set; }
    }
}
