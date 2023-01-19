namespace NeuralNetwork.Interfaces.Model
{
    public class Vertex
    {
        public string Identifier { get; set; }

        public Neuron Origin { get; set; }

        public Neuron Target { get; set; }

        public float Weight { get; set; }
    }
}
