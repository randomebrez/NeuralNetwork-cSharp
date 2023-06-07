namespace NeuralNetwork.Interfaces.Model
{
    public class LayerCaracteristics
    {
        public LayerCaracteristics(LayerTypeEnum type, int layerId, int neuronNumber = 0, ActivationFunctionEnum activationFunction = ActivationFunctionEnum.Identity, float caracteristicValue = 0f)
        {
            Type = type;
            LayerId = layerId;
            ActivationFunction = activationFunction;
            ActivationFunction90PercentTreshold = caracteristicValue;
            NeuronNumber = neuronNumber;
        }

        public LayerTypeEnum Type { get; }

        public int NeuronNumber { get; set; }

        public int LayerId { get; }

        public ActivationFunctionEnum ActivationFunction { get; set;  }

        public float ActivationFunction90PercentTreshold { get; set; }
    }
}
