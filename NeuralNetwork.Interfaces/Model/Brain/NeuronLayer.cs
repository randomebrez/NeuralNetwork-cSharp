using System.Collections.Generic;

namespace NeuralNetwork.Interfaces.Model
{
    public class NeuronLayer
    {
        public int NeuronNumber => Neurons.Count;

        public LayerTypeEnum LayerType { get; set; }

        public int Id { get; set; }

        public List<Neuron> Neurons { get; set; } = new List<Neuron>();
    }
}
