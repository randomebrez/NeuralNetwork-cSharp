using NeuralNetwork.Abstraction.Model;
using System.Collections.Generic;

namespace NeuralNetwork.Abstraction
{
    public interface IBrainCalculator
    {
        void BrainCompute(Brain brain, List<float> inputs);

        List<float> BrainGraphCompute(BrainGraph graph, Dictionary<string, List<float>> inputs);
    }
}