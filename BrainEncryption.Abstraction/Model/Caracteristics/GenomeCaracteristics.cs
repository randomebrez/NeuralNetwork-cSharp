namespace BrainEncryption.Abstraction.Model
{
    public class GenomeCaracteristics
    {
        public GenomeCaracteristics(int geneNumber, int weightBitArraySize, float weightMinimumValue)
        {
            GeneNumber = geneNumber;
            WeightBitArraySize = weightBitArraySize;
            WeightMinimumValue = weightMinimumValue;
        }

        public int GeneNumber { get; set; }
        public float WeightMinimumValue { get; set; }
        public int WeightBitArraySize { get; set; }
    }
}
