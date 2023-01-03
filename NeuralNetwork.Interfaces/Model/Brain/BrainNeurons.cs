namespace NeuralNetwork.Interfaces.Model
{
    public class BrainNeurons
    {
        public BrainNeurons()
        {
            Inputs = new List<NeuronInput>();
            Neutrals = new List<NeuronNeutral>();
            Outputs = new List<NeuronOutput>();
            SinkNeuron = new NeuronOutput(-1, 2)
            {
                Value = 1
            };
        }

        public List<NeuronInput> Inputs { get; set; }

        public List<NeuronNeutral> Neutrals { get; set; }

        public List<NeuronOutput> Outputs { get; set; }

        public NeuronOutput SinkNeuron { get; set; }
    }
}
