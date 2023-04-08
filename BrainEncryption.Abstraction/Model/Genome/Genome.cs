using System.Text;

namespace BrainEncryption.Abstraction.Model
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
    }
}
