using System.Collections.Generic;
using System.Threading.Tasks;
using NeuralNetwork.Interfaces.Model;

namespace NeuralNetwork.Interfaces
{
    public interface IDatabaseGateway
    {
        Task StoreBrainsAsync(List<Brain> genomes, int simulationId);
    }
}
