using System.Collections.Generic;

namespace NeuralNetwork.Interfaces.Model
{ 
    public class BrainGraph
    {
        public BrainGraph()
        {
            BrainNodes = new Dictionary<string, BrainGenomePair>();
            BrainEdges = new Dictionary<string, List<BrainGenomePair>>();
        }

        public Dictionary<string, BrainGenomePair> BrainNodes { get; set; }

        public Brain DecisionBrain { get; set; }

        public Dictionary<string, List<BrainGenomePair>> BrainEdges { get; set; }
    }
}
