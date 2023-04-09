using System.Collections.Generic;
using System.Linq;

namespace BrainEncryption.Abstraction.Model
{
    public class BrainNeurons
    {
        public BrainNeurons(int layerNumber)
        {
            Inputs = new List<NeuronInput>();
            Neutrals = new List<NeuronNeutral>();
            Outputs = new List<NeuronOutput>();
            SinkNeuron = new NeuronOutput(-1, layerNumber, 1);
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

        public void AddNeuron(Neuron neuron)
        {
            if (neuron is NeuronInput)
                Inputs.Add(neuron as NeuronInput);
            else if (neuron is NeuronNeutral)
                Neutrals.Add(neuron as NeuronNeutral);
            else
                Outputs.Add(neuron as NeuronOutput);
        }
    }
}
