using System;
using System.Collections.Generic;

namespace BrainEncryption.Abstraction.Model
{
    public class Brain
    {
        public Brain(int outputLayerId)
        {
            UniqueIdentifier = Guid.NewGuid();
            Edges = new List<Edge>();
            Neurons = new BrainNeurons(outputLayerId);
            OutputLayerId = outputLayerId;
        }

        public Guid UniqueIdentifier { get; private set; }

        public List<Edge> Edges { get; set; }

        public BrainNeurons Neurons { get; set; }

        public int OutputLayerId { get; set; }

        public int UseForChildCounter { get; set; }

        public int MaxChildNumber { get; set; } = 3;
    }
}