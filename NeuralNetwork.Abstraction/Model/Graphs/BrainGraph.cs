using System.Collections.Generic;

namespace NeuralNetwork.Abstraction.Model
{
    public class BrainGraph : GenericGraph<BrainGenomePair>
    {
        public Dictionary<string, BrainGenomePair> BrainNodes => NodesToDictionary;

        public Dictionary<string, GenericEdge<BrainGenomePair>> EdgeDic => EdgesToDictionary;

        public Brain DecisionBrain { get; set; }
    }
}
