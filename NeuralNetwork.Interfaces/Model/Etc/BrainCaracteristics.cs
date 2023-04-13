using System.Collections.Generic;

namespace NeuralNetwork.Interfaces.Model
{
    public class BrainCaracteristics
    {
        public BrainCaracteristics(string name)
        {
            NeutralLayers = new List<LayerCaracteristics>();
            Name = name;
        }

        public string Name { get; set; }


        // Neural network part
        public LayerCaracteristics InputLayer { get; set; }

        public List<LayerCaracteristics> NeutralLayers { get; set; }

        public LayerCaracteristics OutputLayer { get; set; }


        // Genome part
        public GenomeCaracteristics GenomeCaracteristics { get; set; }

        public HashSet<string> GeneCodes { get; set; }
    }
}
