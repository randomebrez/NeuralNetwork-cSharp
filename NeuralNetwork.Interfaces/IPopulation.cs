using System.Collections.Generic;
using NeuralNetwork.Interfaces.Model;

namespace NeuralNetwork.Interfaces
{
    public interface IPopulation
    {
        Brain[] GenerateFirstGeneration(int childNumber);

        Brain[] GenerateNewGeneration(int childNumber, List<Brain> selectedBrains);
    }
}
