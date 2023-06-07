using System.Collections.Generic;

namespace NeuralNetwork.Interfaces.Model
{ 
    public class BrainGraph
    {
        public BrainGraph()
        {
            BrainNodes = new Dictionary<string, Brain>();
            BrainEdges = new Dictionary<string, List<Brain>>();
        }

        public Dictionary<string, Brain> BrainNodes { get; set; }

        public Brain DecisionBrain { get; set; }

        public Dictionary<string, List<Brain>> BrainEdges { get; set; }
    }
}
