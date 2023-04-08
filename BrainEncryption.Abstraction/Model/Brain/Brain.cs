using NeuralNetwork.Interfaces.Model;

namespace BrainEncryption.Abstraction.Model
{
    public class Brain
    {
        public Brain()
        {
            UniqueIdentifier = Guid.NewGuid();
            Edges = new List<Edge>();
        }

        public Guid UniqueIdentifier { get; private set; }

        public List<Edge> Edges { get; set; }

        public BrainNeurons Neurons { get; set; }

        public int UseForChildCounter { get; set; }

        public int MaxChildNumber { get; set; } = 3;
    }
}