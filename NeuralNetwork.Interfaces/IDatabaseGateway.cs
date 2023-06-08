using System.Collections.Generic;
using System.Threading.Tasks;
using NeuralNetwork.Interfaces.Model;

namespace NeuralNetwork.Interfaces
{
    public interface IDatabaseGateway
    {
        Task StoreBrainsAsync(int simulationId, int generationId, List<Brain> brains);

        Task<int> AddNewSimulationAsync();
    }
}
