namespace NeuralNetwork.Interfaces.Model
{
    public class BrainGenomePair
    {
        public string Key { get; set; }

        public Brain Brain { get; set; }

        public Genome Genome { get; set; }
    }
}
