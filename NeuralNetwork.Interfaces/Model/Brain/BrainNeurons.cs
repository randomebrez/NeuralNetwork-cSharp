using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork.Interfaces.Model
{
    public class BrainNeurons
    {
        public BrainNeurons()
        {
            Inputs = new List<NeuronInput>();
            Neutrals = new List<NeuronNeutral>();
            Outputs = new List<NeuronOutput>();
        }

        public List<NeuronInput> Inputs { get; set; }

        public List<NeuronNeutral> Neutrals { get; set; }

        public List<NeuronOutput> Outputs { get; set; }

        public NeuronOutput SinkNeuron { get; set; }

        public Neuron GetNeuronByName(string name)
        {
            var type = name[0];
            switch(type)
            {
                case 'I':
                    return Inputs.FirstOrDefault(t => t.UniqueId == name);
                case 'N':
                    return Neutrals.FirstOrDefault(t => t.UniqueId == name);
                case 'O':
                    return Outputs.FirstOrDefault(t => t.UniqueId == name);
            }
            return null;
        }
    }
}
