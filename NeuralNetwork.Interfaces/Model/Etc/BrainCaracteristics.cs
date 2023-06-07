using System.Collections.Generic;

namespace NeuralNetwork.Interfaces.Model
{
    public class BrainCaracteristics
    {
        public BrainCaracteristics()
        {
            NeutralLayers = new List<LayerCaracteristics>();
        }

        public string TemplateName { get; set; }

        public string BrainName { get; set; }

        public bool IsDecisionBrain { get; set; }


        // Neural network part
        public LayerCaracteristics InputLayer { get; set; }

        public List<LayerCaracteristics> NeutralLayers { get; set; }

        public LayerCaracteristics OutputLayer { get; set; }


        // Genome part
        public GenomeCaracteristics GenomeCaracteristics { get; set; }
    }
}
