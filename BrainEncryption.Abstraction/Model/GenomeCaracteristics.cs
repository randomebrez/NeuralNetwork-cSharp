using System.Collections.Generic;
using System.Linq;

namespace BrainEncryption.Abstraction.Model
{
    public class GenomeCaracteristics
    {
        public GenomeCaracteristics(int geneNumber, int weightBitArraySize, HashSet<string> geneCodes)
        {
            GeneNumber = geneNumber;
            WeightBitArraySize = weightBitArraySize;
            GeneCodes = geneCodes.ToList();
        }

        public int GeneNumber { get; set; }

        public int WeightBitArraySize { get; set; }

        public List<string> GeneCodes { get; set; }
    }
}
