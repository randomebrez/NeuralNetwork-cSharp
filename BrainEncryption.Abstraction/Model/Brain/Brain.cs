using System.Collections.Generic;

namespace BrainEncryption.Abstraction.Model
{
    public class Brain
    {
        public Brain(int outputLayerId, string name)
        {
            Name = name;
            Edges = new List<Edge>();
            Neurons = new BrainNeurons();
            OutputLayerId = outputLayerId;
        }

        public string Name { get; private set; }

        public List<Edge> Edges { get; set; }

        public BrainNeurons Neurons { get; set; }

        public int OutputLayerId { get; set; }
    }
}