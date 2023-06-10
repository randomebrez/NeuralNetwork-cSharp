using System.Collections.Generic;
using System.Threading.Tasks;
using NeuralNetwork.Abstraction.Model;

namespace NeuralNetwork.Abstraction
{
    public interface IDatabaseGateway
    {
        Task StoreBrainsAsync(int simulationId, int generationId, List<Brain> brains);

        Task<int> AddNewSimulationAsync();
    }
}
