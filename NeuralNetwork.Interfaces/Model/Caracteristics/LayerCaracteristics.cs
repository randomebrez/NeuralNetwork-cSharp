namespace NeuralNetwork.Interfaces.Model
{
    public class LayerCaracteristics
    {
        public LayerCaracteristics(int layerId, LayerTypeEnum type)
        {
            LayerId = layerId;
            Type = type;
        }

        public int LayerId { get; }
        public LayerTypeEnum Type { get; }

        public int NeuronNumber { get; set; } = 0;
        public float NeuronTreshold { get; set; } = 0;

        public ActivationFunctionEnum ActivationFunction { get; set; } = ActivationFunctionEnum.Identity;
        public float ActivationFunction90PercentTreshold { get; set; } = 0;
    }
}
