using NeuralNetwork.DataBase.Abstraction.Model;
using NeuralNetwork.Interfaces.Model;

namespace NeuralNetwork.Helpers
{
    public static class DatabaseMapper
    {
        public static BrainDb ToDb(this Brain brain, int simulationId, int generationId)
        {
            return new BrainDb
            {
            };
        }
    }
}
