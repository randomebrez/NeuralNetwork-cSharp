using NeuralNetwork.Interfaces.Model;
using System.Collections.Generic;

namespace NeuralNetwork.Interfaces
{
    public interface IBrainCalculator
    {
        void BrainCompute(Brain brain, List<float> inputs);

        List<float> BrainGraphCompute(BrainGraph graph, Dictionary<string, List<float>> inputs);
    }
}