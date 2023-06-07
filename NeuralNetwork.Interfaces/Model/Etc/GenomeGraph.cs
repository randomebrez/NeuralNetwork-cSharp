using System.Collections.Generic;

namespace NeuralNetwork.Interfaces.Model
{
    public class GenomeGraph
    {
        public GenomeGraph()
        {
            GenomeNodes = new List<BrainGenomePair>();
            GenomeEdges = new Dictionary<string, List<string>>();
            DistinctGenomes = new Dictionary<string, Genome>();
        }

        public Dictionary<string, Genome> DistinctGenomes { get; set; }

        public List<BrainGenomePair> GenomeNodes { get; set; }

        public Dictionary<string, List<string>> GenomeEdges { get; set; }
    }
}
