using NeuralNetwork.Database;
using NeuralNetwork.DataBase.Abstraction;
using NeuralNetwork.Helpers;
using NeuralNetwork.Interfaces;
using NeuralNetwork.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeuralNetwork.Implementations
{
    public class DatabaseGateway : IDatabaseGateway
    {
        private IDatabaseStorage _dbGateway;


        public DatabaseGateway(string sqlFilePath)
        {
            _dbGateway = new DatabaseStorage(sqlFilePath);
        }


        public async Task<int> AddNewSimulationAsync()
        {
            return await _dbGateway.AddNewSimulationAsync().ConfigureAwait(false);
        }

        public async Task StoreBrainsAsync(int simulationId, int generationId, List<Brain> brains)
        {
            try
            {
                await _dbGateway.StoreBrainsAsync(brains.Select(t => t.ToDb(simulationId, generationId)).ToList());                
            }
            catch (Exception ex)
            {
                throw new Exception($"Something went wrong while adding unit brains", ex);
            }
        }
    }
}
