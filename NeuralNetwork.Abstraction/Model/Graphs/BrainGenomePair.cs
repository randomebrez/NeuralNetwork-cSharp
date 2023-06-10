namespace NeuralNetwork.Abstraction.Model
{
    public class GenomeCaracteristicPair
    {
        public Genome Genome { get; set; }

        public BrainCaracteristics Caracteristics { get; set; }
    }

    public class BrainGenomePair : GenomeCaracteristicPair
    {
        public string Key { get; set; }

        public Brain Brain { get; set; }
    }
}
