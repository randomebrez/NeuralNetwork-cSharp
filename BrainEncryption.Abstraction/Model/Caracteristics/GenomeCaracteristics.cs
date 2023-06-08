namespace BrainEncryption.Abstraction.Model
{
    public class GenomeCaracteristics
    {
        public GenomeCaracteristics(int geneNumber, int weightBitArraySize)
        {
            GeneNumber = geneNumber;
            WeightBitArraySize = weightBitArraySize;
        }

        public int GeneNumber { get; set; }
        public int WeightBitArraySize { get; set; }
    }
}
