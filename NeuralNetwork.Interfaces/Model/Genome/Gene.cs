using System;
using System.Text;

namespace NeuralNetwork.Interfaces.Model
{
    public class Gene
    {
        public Gene(string identifier, int weighBytesLength)
        {
            EdgeIdentifier = identifier;
            WeighBytes = new bool[weighBytesLength];
            for (int i = 0; i < weighBytesLength; i++)
                WeightBytesMaxValue += (int)Math.Pow(2, i);
        }

        public int WeightBytesMaxValue { get; set; }
        
        public string EdgeIdentifier { get; set; }

        public bool IsActive { get; set; }

        public bool WeighSign { get; set; }
        
        public bool[] WeighBytes { get; set; }
        
        public float Bias { get; set; }

        public override string ToString()
        {
            var result = new StringBuilder(EdgeIdentifier);

            result.Append(WeighSign ? "|1" : "|0");

            for(int i = 0; i < WeighBytes.Length; i++)
            {
                var toInt = WeighBytes[i] ? 1 : 0;
                result.Append($"{toInt}");
            }

            result.Append($"|{Bias}");

            return result.ToString();
        }
    }
}
