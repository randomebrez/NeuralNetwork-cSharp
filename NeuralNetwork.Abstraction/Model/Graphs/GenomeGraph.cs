using System;

namespace NeuralNetwork.Abstraction.Model
{
    public class GenomeGraph : GenericGraph<GenomeCaracteristicPair>
    {
        public Guid ParentA { get; set; }

        public Guid ParentB { get; set; }
    }
}
