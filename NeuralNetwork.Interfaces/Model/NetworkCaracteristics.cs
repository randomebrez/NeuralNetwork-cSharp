using System.Collections.Generic;

namespace NeuralNetwork.Interfaces.Model
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

        public int GeneNumber { get; set; }

        public int WeighBytesNumber { get; set; }


    }
}
