using System.Collections.Generic;

namespace NeuralNetwork.Interfaces.Model
{
    public class BrainCaracteristics
    {
        public BrainCaracteristics(string name)
        {
            NeutralNumbers = new List<int>();
            Name = name;
        }

        public string Name { get; set; }


        // Neural network part
        public int InputNumber { get; set; }

        public List<int> NeutralNumbers { get; set; }

        public int OutputNumber { get; set; }

        public float Tanh90Percent { get; set; }

        public float Sigmoid90Percent { get; set; }


        // Genome part
        public int GeneNumber { get; set; }

        public int WeighBytesNumber { get; set; }

        public HashSet<string> GeneCodes { get; set; }
    }
}
