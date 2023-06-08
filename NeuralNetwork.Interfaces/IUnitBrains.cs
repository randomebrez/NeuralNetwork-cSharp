using System.Collections.Generic;

namespace NeuralNetwork.Interfaces
{
    public interface IUnitBrains
    {
        //OU_TEST : Only used by test prog
        void ComputeBrain(string brainKey, List<float> inputs);

        (int ouputId, float neuronIntensity) GetBestOutput(string brainKey);

        List<float> GetOutputs(string brainKey);
    }
}