using System.Collections.Generic;
using NeuralNetwork.Interfaces.Model;

namespace NeuralNetwork.Interfaces
{
    public interface IPopulationManager
    {
        Unit[] GenerateFirstGeneration(int childNumber, List<BrainCaracteristics> brainCaracteristics);

        Unit[] GenerateNewGeneration(int childNumber, List<Unit> selectedUnits, List<BrainCaracteristics> brainCaracteristics, int crossOverNumber, float mutationRate);

        Unit[] GetUnitFromGenomes(BrainCaracteristics brainCaracteristic, List<string> genomeStrings);
    }
}
