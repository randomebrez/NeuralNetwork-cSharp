using System.Collections.Generic;

namespace NeuralNetwork.Interfaces.Model.Caracteristics
{
    public class BrainCaracteristics
    {
        // Needed when translating GenomeGraph to BrainGraph to set it. It is then used for BrainGraph computation to start the recusrion from that brain
        public bool IsDecisionBrain { get; set; }
        public string BrainName { get; set; }


        // Neural network layers caracteristics
        public LayerCaracteristics InputLayer { get; set; }
        public List<LayerCaracteristics> NeutralLayers { get; set; } = new List<LayerCaracteristics>();
        public LayerCaracteristics OutputLayer { get; set; }


        // Genome part
        public GenomeCaracteristics GenomeCaracteristics { get; set; }
    }
}
