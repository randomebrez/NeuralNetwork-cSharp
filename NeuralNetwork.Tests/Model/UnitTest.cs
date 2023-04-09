using NeuralNetwork.Interfaces.Model;

namespace NeuralNetwork.Tests.Model
{
    public class UnitTest
    {
        public Unit Unit { get; set; }

        public int LifeTime { get; set; }

        public int Age { get; set; } = 0;

        public int GenerationId { get; set; }

        public int SimulationId { get; set; }

        public SpacePosition Position { get; set; }
    }
}
