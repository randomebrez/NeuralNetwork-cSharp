namespace NeuralNetwork.Interfaces.Model
{
    public class Unit
    {
        public int Id { get; set; }

        public Guid Identifier { get; set; }

        public Brain Brain { get; set; }

        public SpacePosition Position { get; set; }

        public int GenerationId { get; set; }

        public int Age { get; set; }

        public bool Fertile { get; set; }

        private int _staticCounter { get; set; }

        public int LifeTime { get; set; }

        public float? SelectionScore { get; set; }

        public int SimulationId { get; set; }
    }
}