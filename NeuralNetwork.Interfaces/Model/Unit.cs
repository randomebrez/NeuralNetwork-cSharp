using System;

namespace NeuralNetwork.Abstraction.Model
{
    public class Unit
    {
        public Guid Identifier { get; } = Guid.NewGuid();

        // Parents (can be null)
        public Guid ParentA { get; set; }
        public Guid ParentB { get; set; }

        // Children parameters
        public int MaxChildNumber { get; set; } = 3;
        public int ChildrenNumber { get; set; }


        // GenomeGraph is used for construction of BrainGraph. It can be saved in database to be retranslated. Cost less than register the BrainGraph
        // BrainGraph is used for calculations
        public GenomeGraph GenomeGraph { get; set; }
        public BrainGraph BrainGraph { get; set; }
    }
}