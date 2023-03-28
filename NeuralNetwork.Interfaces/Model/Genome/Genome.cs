using System.Text;

namespace NeuralNetwork.Interfaces.Model
{
    public class Genome
    {
        public int GeneNumber { get; set; }

        public Gene[] Genes { get; set; }
        
        public Genome(int geneNumber)
        {
            GeneNumber = geneNumber;
            Genes = new Gene[geneNumber];
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            for (int i = 0; i < GeneNumber; i++)
                result.Append($"{Genes[i]}!");

            return result.ToString();
        }
    }
}
