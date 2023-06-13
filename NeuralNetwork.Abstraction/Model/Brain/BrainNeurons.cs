using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            var exceptMessage = new StringBuilder();
            try
            {
                var type = name[0];
                switch (type)
                {
                    case 'I':
                        return InputLayer.Neurons.FirstOrDefault(t => t.UniqueId == name);
                    case 'N':
                        var layerIndex = (int)Char.GetNumericValue(name[1]);
                        return NeutralLayers[layerIndex - 1].Neurons.FirstOrDefault(t => t.UniqueId == name);
                    case 'O':
                        return OutputLayer.Neurons.FirstOrDefault(t => t.UniqueId == name);
                    default:
                        return null;
                }
            }
            catch (System.Exception e)
            {
                exceptMessage.AppendLine($"{name}; I:{InputLayer.Neurons.Count}; N:{NeutralLayers[0].Neurons.Count}; I:{OutputLayer.Neurons.Count}");
                throw new System.Exception(exceptMessage.ToString(), e); 
            }
        }
    }
}
