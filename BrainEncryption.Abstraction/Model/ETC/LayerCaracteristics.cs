namespace BrainEncryption.Abstraction.Model
{
    public class LayerCaracteristics
    {
        public LayerCaracteristics(LayerTypeEnum type)
        {
            Type = type;
        }

        public int LayerId { get; set; }

        public LayerTypeEnum Type { get; }

        public int NeuronNumber { get; set; }

        public ActivationFunctionEnum ActivationFunction { get; set; }

        public float ActivationFunction90PercentTreshold { get; set; }
    }
}
