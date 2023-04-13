namespace NeuralNetwork.Interfaces.Model
{
    public class LayerCaracteristics
    {
        public LayerCaracteristics(LayerTypeEnum type, int layerId, int neuronNumber, ActivationFunctionEnum activationFunction, float caracteristicValue)
        {
            Type = type;
            LayerId = layerId;
            ActivationFunction = activationFunction;
            ActivationFunction90PercentTreshold = caracteristicValue;
            NeuronNumber = neuronNumber;
        }

        public LayerTypeEnum Type { get; }

        public int NeuronNumber { get; }

        public int LayerId { get; }

        public ActivationFunctionEnum ActivationFunction { get; }

        public float ActivationFunction90PercentTreshold { get; }
    }
}
