namespace NeuralNetwork.Interfaces.Model
{
    public class Gene
    {
        public Gene(string identifier, int weighBytesLength)
        {
            VertexIdentifier = identifier;
            WeighBytes = new bool[weighBytesLength];
            for (int i = 0; i < weighBytesLength; i++)
                WeightBytesMaxValue += (int)Math.Pow(2, i);
        }

        public int WeightBytesMaxValue { get; set; }
        
        public string VertexIdentifier { get; set; }

        public bool IsActive { get; set; }

        public bool WeighSign { get; set; }
        
        public bool[] WeighBytes { get; set; }
        
        
        public float Bias { get; set; }
    }
}
