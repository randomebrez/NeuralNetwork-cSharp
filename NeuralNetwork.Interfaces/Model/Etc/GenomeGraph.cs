using System.Collections.Generic;

namespace NeuralNetwork.Interfaces.Model
{
    public class GenomeGraph
    {
        public GenomeGraph()
        {
            GenomeNodes = new List<BrainGenomePair>();
            GenomeEdges = new Dictionary<string, List<string>>();
        }

        public List<BrainGenomePair> GenomeNodes { get; set; }

        public Dictionary<string, List<string>> GenomeEdges { get; set; }
    }
}
