using System;
using System.Collections.Generic;

namespace NeuralNetwork.Abstraction.Model
{
    public class GenomeGraph : GenericGraph<GenomeCaracteristicPair>
    {
        public Dictionary<string, GenericEdge<GenomeCaracteristicPair>> EdgeDic => EdgesToDictionary;

        public Guid ParentA { get; set; }

        public Guid ParentB { get; set; }
    }
}
