namespace NeuralNetwork.Interfaces.Model
{
    public class LayerCaracteristics
    {
        public LayerCaracteristics(LayerTypeEnum type, int layerId, int neuronNumber = 0, float neuronTreshold = 0f, ActivationFunctionEnum activationFunction = ActivationFunctionEnum.Identity, float caracteristicValue = 0f)
        {
            Type = type;
            LayerId = layerId;
            NeuronNumber = neuronNumber;
            NeuronTreshold = neuronTreshold;
            ActivationFunction = activationFunction;
            ActivationFunction90PercentTreshold = caracteristicValue;
        }

        public LayerTypeEnum Type { get; }

        public int LayerId { get; }

        public int NeuronNumber { get; set; }

        public float NeuronTreshold { get; set; }

        public ActivationFunctionEnum ActivationFunction { get; set;  }

        public float ActivationFunction90PercentTreshold { get; set; }
    }
}
