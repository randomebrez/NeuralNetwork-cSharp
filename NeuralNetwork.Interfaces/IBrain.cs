using NeuralNetwork.Interfaces.Model;
using System.Collections.Generic;

namespace NeuralNetwork.Interfaces
{
    public interface IBrain
    {
        void ComputeBrain(Brain brain, List<float> inputs);

        List<float> ComputeBrainGraph(BrainGraph graph, Dictionary<string, List<float>> inputs);
    }
}