using NeuralNetwork.Interfaces.Model;
using System.Collections.Generic;

namespace NeuralNetwork.Interfaces
{
    public interface IUnitBrains
    {
        public (int ouputId, float neuronIntensity) ComputeOutput(List<float> inputs);

        public Brain GetBrain();

        public Dictionary<int, float> ComputeOuputs(List<float> inputs);
    }
}