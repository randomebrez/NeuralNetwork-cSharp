namespace NeuralNetwork.Abstraction.Model
{
    public class Gene
    {
        // Maybe simplify the public version. I think it was just needed to save in DB.
        // Think twice because we can use a public version for a gene design screen in Unity
        public string EdgeIdentifier { get; set; }

        public bool WeighSign { get; set; }

        public bool[] WeighBits { get; set; }

        public float WeighMinimumValue { get; set; }
        public float Bias { get; set; }

        public bool IsActive { get; set; }        

        public string GeneToString { get; set; }
    }
}
