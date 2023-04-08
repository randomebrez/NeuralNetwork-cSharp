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

        public int InputNumber { get; set; }

        public List<int> NeutralNumbers { get; set; }

        public int OutputNumber { get; set; }

        public int GeneNumber { get; set; }

        public int WeighBytesNumber { get; set; }
    }
}
