using System.Collections.Generic;

namespace NeuralNetwork.Interfaces
{
    public interface IUnitBrains
    {
        void ComputeBrain(string brainKey, List<float> inputs);

        (int ouputId, float neuronIntensity) GetBestOutput(string brainKey);

        List<float> GetOutputs(string brainKey);
    }
}