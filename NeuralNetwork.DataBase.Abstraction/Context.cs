using Microsoft.EntityFrameworkCore;
using NeuralNetwork.DataBase.Abstraction.Model;

namespace NeuralNetwork.DataBase.Abstraction
{
    public class Context : DbContext
    {
        private readonly string _connectionString;
        public Context(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_connectionString}");
        }

        public DbSet<UnitDb> Units { get; set; }

        public DbSet<BrainDb> Brains { get; set; }

        public DbSet<BrainVertexDb> Vertices { get; set; }

        public DbSet<BrainVertexLinksDb> BrainVertexLinks { get; set; }

        public DbSet<InputDb> Inputs { get; set; }

        public DbSet<OutputDb> Outputs { get; set; }

        public DbSet<UnitStepDb> UnitSteps { get; set; }
    }
}
