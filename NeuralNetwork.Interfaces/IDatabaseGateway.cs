using NeuralNetwork.Interfaces.Model;

namespace NeuralNetwork.Interfaces
{
    public interface IDatabaseGateway
    {
        Task StoreUnitsAsync(List<Unit> units);

        Task StoreUnitBrainsAsync(List<Unit> units);

        Task StoreLifeStepAsync(List<(Unit unit, string[] positions)> units);

        Task CreateVerticesAsync(HashSet<string> geneCodes);
    }
}
