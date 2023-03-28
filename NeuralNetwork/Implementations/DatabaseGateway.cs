using Microsoft.EntityFrameworkCore;
using NeuralNetwork.DataBase.Abstraction;
using NeuralNetwork.DataBase.Abstraction.Model;
using NeuralNetwork.Interfaces;
using NeuralNetwork.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeuralNetwork.Implementations
{
    public class BucketList<T>
    {
        public List<IEnumerable<T>> Elements { get; private set; }
        public BucketList(IEnumerable<T> elements, int bucketSize = 2000)
        {
            Elements = new List<IEnumerable<T>>();
            var index = 0;
            var itemNumber = elements.Count();
            while (index < itemNumber)
            {
                Elements.Add(elements.Skip(index).Take(Math.Min(itemNumber, index + bucketSize)));
                index += bucketSize;
            }
        }
    }

    public class DatabaseGateway : IDatabaseGateway
    {
        private readonly Context _context;
        public DatabaseGateway(Context context)
        {
            context.Database.EnsureCreated();
            _context = context;
        }

        public async Task<int> AddNewSimulationAsync()
        {
            var newSimulation = _context.Simulations.Add(new SimulationDb());
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return newSimulation.Entity.Id;
        }

        public async Task StoreBrainsAsync(List<Brain> brains, int simulationId)
        {
            try
            {
                var dbBrains = new List<BrainDb>();
                for(int i = 0; i < brains.Count(); i++)
                {
                    var brain = brains[i];
                    var dbBrain = new BrainDb
                    {
                        Genome = brain.Genome.ToString(),
                        Score = brain.Score,
                        UniqueId = brain.UniqueIdentifier.ToString(),
                        SimulationId = simulationId
                    };
                    dbBrains.Add(dbBrain);
                }
                await _context.Brains.AddRangeAsync(dbBrains).ConfigureAwait(false);
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
