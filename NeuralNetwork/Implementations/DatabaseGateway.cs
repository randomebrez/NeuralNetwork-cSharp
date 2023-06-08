using DataBase;
using DataBase.Abstraction;
using NeuralNetwork.Interfaces;
using NeuralNetwork.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NeuralNetwork.Implementations
{
    public class DatabaseGateway : IDatabaseGateway
    {
        private IDataBaseStorage _dbGateway;


        public DatabaseGateway(string sqlFilePath)
        {
            _dbGateway = new DataBaseStorage(sqlFilePath);
        }


        public async Task<int> AddNewSimulationAsync()
        {
            return await _dbGateway.AddNewSimulationAsync().ConfigureAwait(false);
        }

        public async Task StoreBrainsAsync(int simulationId, int generationId, List<Brain> brains)
        {
            try
            {
                //await _dbGateway.StoreBrainsAsync(brains.Select(t => t.ToDb(simulationId, generationId)).ToList());                
            }
            catch (Exception ex)
            {
                throw new Exception($"Something went wrong while adding unit brains", ex);
            }
        }
    }
}
