using NeuralNetwork.DataBase.Abstraction.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NeuralNetwork.DataBase.Abstraction
{
    public interface IDatabaseStorage
    {
        Task<int> AddNewSimulationAsync();

        Task StoreBrainsAsync(List<BrainDb> brains);
    }
}