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

        public async Task StoreUnitsAsync(List<Unit> units)
        {
            try
            {
                var dbUnits = units.Select(t => ToDb(t));
                _context.Units.AddRange(dbUnits);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Something went wrong while adding units", ex);
            }
        }

        public async Task StoreUnitBrainsAsync(List<Unit> units)
        {
            try
            {
                var dbVertices = await _context.Vertices.ToListAsync().ConfigureAwait(false);
                var unitIdentifiers = units.Select(t => t.Identifier.ToString()).ToHashSet();
                var dbUnits = await _context.Units.Where(t => unitIdentifiers.Contains(t.Identifier)).ToDictionaryAsync(t => t.Identifier, t => t).ConfigureAwait(false);
                List<BrainDb> brains = new List<BrainDb>();
                List<BrainVertexLinksDb> dbLinks = new List<BrainVertexLinksDb>();
                foreach (var unit in units)
                {
                    var brainVertices = unit.Brain.Vertices.Select(t => t.Identifier).ToHashSet();
                    var dbBrainVertices = dbVertices.Where(t => brainVertices.Contains(t.Identifier)).ToDictionary(t => t.Identifier, u => u);
                    var dbBrain = new BrainDb
                    {
                        UnitId = dbUnits[unit.Identifier.ToString()].Id
                    };
                    brains.Add(dbBrain);
                    dbLinks.AddRange(unit.Brain.Vertices.Select(t => new BrainVertexLinksDb { Brain = dbBrain, Vertex = dbBrainVertices[t.Identifier], VertexWeight = t.Weight }));
                }
                _context.Brains.AddRange(brains);
                _context.BrainVertexLinks.AddRange(dbLinks);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                ClearChangeTracker();
            }
            catch (Exception ex)
            {
                throw new Exception($"Something went wrong while adding unit brains", ex);
            }
        }

        public async Task StoreLifeStepAsync(List<(Unit unit, string[] positions)> units)
        {
            try
            {
                var unitIdentifiers = units.Select(t => t.unit.Identifier.ToString()).ToHashSet();
                var dbUnits = await _context.Units.Where(t => unitIdentifiers.Contains(t.Identifier)).AsNoTracking().ToDictionaryAsync(t => t.Identifier, t => t).ConfigureAwait(false);

                var dbSteps = new List<UnitStepDb>();
                foreach (var unit in units)
                {
                    for (int i = 0; i < unit.positions.Length; i++)
                    {
                        dbSteps.Add(new UnitStepDb
                        {
                            UnitId = dbUnits[unit.unit.Identifier.ToString()].Id,
                            LifeStep = i,
                            Position = unit.positions[i]
                        });
                    }
                }
                _context.UnitSteps.AddRange(dbSteps);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                ClearChangeTracker();
                //_context.Inputs.Add(new InputDb
                //{
                //    Values = inputs,
                //    UnitStep = dbStep.Entity
                //});
                //_context.Outputs.Add(new OutputDb
                //{
                //    Values = outputs,
                //    UnitStep = dbStep.Entity
                //});
            }
            catch (Exception ex)
            {
                throw new Exception($"Something went wrong while storing unit life steps", ex);
            }
        }

        public async Task CreateVerticesAsync(HashSet<string> geneCodes)
        {
            try
            {
                var dbVertices = await _context.Vertices.ToListAsync().ConfigureAwait(false);
                var existingVerticesIdentifier = dbVertices.Select(t => t.Identifier).ToHashSet();
                foreach (var geneCode in geneCodes)
                {
                    if (existingVerticesIdentifier.Contains(geneCode))
                        continue;

                    var newVertex = _context.Vertices.Add(new BrainVertexDb
                    {
                        Identifier = geneCode
                    });
                    dbVertices.Add(newVertex.Entity);
                }
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Something went while fetching vertices", ex);
            }
        }

        public async Task<int> AddNewSimulationAsync()
        {
            var newSimulation = _context.Simulations.Add(new Simulation());
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return newSimulation.Entity.Id;
        }

        private void ClearChangeTracker()
        {
            _context.ChangeTracker.Clear();
        }

        private UnitDb ToDb(Unit unit)
        {
            return new UnitDb
            {
                Identifier = unit.Identifier.ToString(),
                GenerationId = unit.GenerationId,
                SelectionScore = unit.SelectionScore,
                SimulationId = unit.SimulationId
            };
        }

        private Unit ToData(UnitDb unit)
        {
            return new Unit
            {
                Id = unit.Id,
                Identifier = new Guid(unit.Identifier),
                GenerationId = unit.GenerationId,
                SelectionScore = unit.SelectionScore
            };
        }

        private Vertex ToData(BrainVertexDb vertex)
        {
            return new Vertex
            {
                Identifier = vertex.Identifier
            };
        }
    }
}
