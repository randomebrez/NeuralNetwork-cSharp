using System.Collections.Generic;

namespace NeuralNetwork.Abstraction.Model
{
    public class GenomeGraph
    {
        public GenomeGraph()
        {
            GenomeNodes = new List<GenomeCaracteristicPair>();
            GenomeEdges = new Dictionary<string, List<string>>();
        }

        public List<GenomeCaracteristicPair> GenomeNodes { get; set; }

        public Dictionary<string, List<string>> GenomeEdges { get; set; }
    }
}
