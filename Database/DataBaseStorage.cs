using DataBase.Abstraction;
using DataBase.Abstraction.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataBase
{
    public class DataBaseStorage : IDataBaseStorage
    {
        private readonly Context _context;
        public DataBaseStorage(string sqlFilePath)
        {
            var context = new Context(sqlFilePath);
            context.Database.EnsureCreated();
            _context = context;
        }

        public async Task<int> AddNewSimulationAsync()
        {
            var newSimulation = _context.Simulations.Add(new SimulationDb());
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return newSimulation.Entity.Id;
        }

        public async Task StoreBrainsAsync(List<BrainDb> brains)
        {
            try
            {
                await _context.Brains.AddRangeAsync(brains).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                ClearChangeTracker();
            }
            catch (Exception ex)
            {
                throw new Exception($"Something went wrong while adding unit brains", ex);
            }
        }

        private void ClearChangeTracker()
        {
            _context.ChangeTracker.Clear();
        }
    }
}