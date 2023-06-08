using System;
using System.Text;

namespace BrainEncryption.Abstraction.Model
{
    public class Gene
    {
        public bool IsActive { get; set; }

        public Gene(string identifier, int weighBytesLength)
        {
            EdgeIdentifier = identifier;
            WeighBits = new bool[weighBytesLength];
            for (int i = 0; i < weighBytesLength; i++)
                EdgeWeightMaxValue += (int)Math.Pow(2, i);
        }

        public string EdgeIdentifier { get; set; }
        public bool WeighSign { get; set; }
        public bool[] WeighBits { get; set; }
        public float Bias { get; set; }


        public int EdgeWeightMaxValue { get; }
        public override string ToString()
        {
            var result = new StringBuilder(EdgeIdentifier);

            result.Append(WeighSign ? "|1" : "|0");

            for(int i = 0; i < WeighBits.Length; i++)
            {
                var toInt = WeighBits[i] ? 1 : 0;
                result.Append($"{toInt}");
            }

            //result.Append($"|{Bias}");

            return result.ToString();
        }
    }
}
