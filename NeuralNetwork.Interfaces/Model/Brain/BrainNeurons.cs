using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork.Abstraction.Model
{
    public class BrainNeurons
    {
        public NeuronLayer InputLayer { get; set; }

        public List<NeuronLayer> NeutralLayers { get; set; } = new List<NeuronLayer>();

        public NeuronLayer OutputLayer { get; set; }

        public Neuron SinkNeuron { get; set; }

        public Neuron GetNeuronByName(string name)
        {
            var type = name[0];
            switch(type)
            {
                case 'I':
                    return InputLayer.Neurons.FirstOrDefault(t => t.UniqueId == name);
                case 'N':
                    var layerIndex = name[1];
                    return NeutralLayers[(int)layerIndex].Neurons.FirstOrDefault(t => t.UniqueId == name);
                case 'O':
                    return OutputLayer.Neurons.FirstOrDefault(t => t.UniqueId == name);
            }
            return null;
        }
    }
}
