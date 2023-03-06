using NeuralNetwork.Interfaces.Model;
using System.Collections.Generic;

namespace NeuralNetwork.Interfaces
{
    public interface IBrain
    {
        public (int ouputId, float neuronIntensity) ComputeOutput(List<float> inputs);

        public Brain GetBrain();
    }
}