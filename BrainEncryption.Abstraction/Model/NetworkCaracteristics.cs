using System.Collections.Generic;
using System.Linq;

namespace BrainEncryption.Abstraction.Model
{
    public class NetworkCaracteristics
    {
        public NetworkCaracteristics()
        {
            NeutralNumbers= new List<int>();
        }

        public int InputNumber { get; set; }

        public List<int> NeutralNumbers { get; set; }

        public int OutputNumber { get; set; }

        public float Tanh90Percent { get; set; }

        public float Sigmoid90Percent { get; set; }

        public int OutputLayerId => 1 + NeutralNumbers.Count();
    }
}
