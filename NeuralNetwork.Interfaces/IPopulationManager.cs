using System.Collections.Generic;
using NeuralNetwork.Interfaces.Model;
using NeuralNetwork.Interfaces.Model.Etc;

namespace NeuralNetwork.Interfaces
{
    public interface IPopulationManager
    {
        //OU_TEST : Only used by test prog
        Unit[] GenerateFirstGeneration(int childNumber, List<BrainCaracteristics> brainCaracteristics);

        Unit[] GenerateNewGeneration(int childNumber, List<Unit> selectedUnits, List<BrainCaracteristics> brainCaracteristics, ReproductionCaracteristics reproductionCaracteristics);

        Unit[] GetUnitFromGenomes(BrainCaracteristics brainCaracteristic, List<string> genomeStrings);


        void DuplicateBrain(Unit unit, string brainKey, string newBrainKey);
    }
}
