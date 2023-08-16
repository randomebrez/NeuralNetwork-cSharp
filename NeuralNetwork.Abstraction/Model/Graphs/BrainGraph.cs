using System.Collections.Generic;

namespace NeuralNetwork.Abstraction.Model
{
    public class BrainGraph : GenericGraph<BrainGenomePair>
    {
        public Dictionary<string, BrainGenomePair> BrainNodes => NodesToDictionary;

        public Brain DecisionBrain { get; set; }
    }
}
