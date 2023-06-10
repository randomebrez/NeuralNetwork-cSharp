namespace NeuralNetwork.Abstraction.Model
{
    public class Genome
    {
        public int GeneNumber { get; set; }

        public Gene[] Genes { get; set; }
        
        public string GenomeToString { get; set; }
    }
}
