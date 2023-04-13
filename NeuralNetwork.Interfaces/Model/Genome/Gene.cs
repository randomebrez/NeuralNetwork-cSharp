namespace NeuralNetwork.Interfaces.Model
{
    public class Gene
    {
        public string EdgeIdentifier { get; set; }

        public bool IsActive { get; set; }

        public bool WeighSign { get; set; }
        
        public bool[] WeighBits { get; set; }
        
        public float Bias { get; set; }

        public string GeneToString { get; set; }
    }
}
