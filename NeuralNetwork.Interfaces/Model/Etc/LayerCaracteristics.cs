namespace NeuralNetwork.Interfaces.Model
{
    public class LayerCaracteristics
    {
        public LayerCaracteristics(LayerTypeEnum type)
        {
            Type = type;
        }

        public LayerTypeEnum Type { get; private set; }

        public int NeuronNumber { get; set; }

        public int LayerId { get; set; }

        public ActivationFunctionEnum ActivationFunction { get; set; }

        public float ActivationFunction90PercentTreshold { get; set; }
    }
}
