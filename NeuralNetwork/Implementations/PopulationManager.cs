using NeuralNetwork.Interfaces;
using NeuralNetwork.Interfaces.Model;
using NeuralNetwork.Helpers;
using NeuralNetwork.Implementations;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork.Managers
{
    public class PopulationManager : IPopulation
    {
        public HashSet<string> GeneCodes { get; private set; }
        private Dictionary<string, Neuron> _neuronsDict;
        private BrainNeurons _baseNeurons;

        private readonly NetworkCaracteristics _dimension;

        public PopulationManager(NetworkCaracteristics dimension)
        {
            _dimension = dimension;
            Initialyze();
        }

        public Brain[] GenerateFirstGeneration(int childNumber)
        {
            var newBrains = new Brain[childNumber];
            for (int i = 0; i < childNumber; i++)
            {
                var newBrain = BrainHelper.GenerateRandomBrain(_dimension, _baseNeurons, GeneCodes.ToList(), _neuronsDict);
                if (newBrain != null)
                    newBrains[i] = newBrain;
            }
            return newBrains;
        }

        public Brain[] GenerateNewGeneration(int childNumber, List<Brain> selectedBrains)
        {
            var newBrains = new Brain[childNumber];
            var maximumTry = 10;
            var childrenNumberByBrain = 3;
            var childrenFromParentNumber = childrenNumberByBrain * selectedBrains.Count() < childNumber ? childrenNumberByBrain * selectedBrains.Count() : childNumber;
            for (int i = 0; i < childrenFromParentNumber; i++)
            {
                var validBrain = false;
                var currentTry = 0;
                var brain = new Brain();
                while (validBrain == false && currentTry < maximumTry)
                {
                    currentTry++;
                    brain = GetChild(selectedBrains);
                    validBrain = brain != null;
                }
                if (validBrain)
                    newBrains[i] = brain;
            }
            if (childrenFromParentNumber < childNumber)
            {
                for (int i = childrenFromParentNumber; i < childNumber; i++)
                {
                    var validBrain = false;
                    var currentTry = 0;
                    var brain = new Brain();
                    while (validBrain == false && currentTry < maximumTry)
                    {
                        currentTry++;
                        brain = BrainHelper.GenerateRandomBrain(_dimension, _baseNeurons, GeneCodes.ToList(), _neuronsDict);
                        validBrain = brain != null;
                    }
                    if (validBrain)
                        newBrains[i] = brain;
                }
            }

            return newBrains;
        }



        private Brain GetChild(List<Brain> selectedBrains)
        {
            var genomeA = selectedBrains[StaticHelper.GetRandomValue(0, selectedBrains.Count - 1)].Genome;
            var genomeB = selectedBrains[StaticHelper.GetRandomValue(0, selectedBrains.Count - 1)].Genome;
            var mixedGenome = genomeA.CrossOver(genomeB);
            mixedGenome.DeepCopy().MutateGenome(GeneCodes.ToList());
            return mixedGenome.GenerateBrainFromGenome(_baseNeurons, _neuronsDict);
        }

        private void Initialyze()
        {
            _neuronsDict = new Dictionary<string, Neuron>();
            GeneCodes = new HashSet<string>();

            _baseNeurons = BrainHelper.GetBaseNeurons(_dimension);
            GeneCodes = GetGeneCodes();

            foreach (var inputNeuron in _baseNeurons.Inputs)
                _neuronsDict.Add(inputNeuron.UniqueId, inputNeuron);
            foreach (var neutralNeuron in _baseNeurons.Neutrals)
                _neuronsDict.Add(neutralNeuron.UniqueId, neutralNeuron);
            foreach (var outputNeuron in _baseNeurons.Outputs)
                _neuronsDict.Add(outputNeuron.UniqueId, outputNeuron);
        }

        private HashSet<string> GetGeneCodes()
        {
            var geneCodes = new HashSet<string>();
            for (int i = 0; i < _dimension.NeutralNumbers.Count(); i++)
            {
                for (int j = 0; j < _dimension.NeutralNumbers[i]; j++)
                {
                    // Input links
                    for (int input = 0; input < _dimension.InputNumber; input++)
                        geneCodes.Add($"I:{input}-N{i+1}:{j}");

                    // Output links
                    for (int output = 0; output < _dimension.OutputNumber; output++)
                        geneCodes.Add($"N{i + 1}:{j}-O:{output}");

                    // Other neutral layers links
                    for (int neutralLayer = i+1; neutralLayer < _dimension.NeutralNumbers.Count(); neutralLayer++)
                    {
                        for (int neutral = 0; neutral < _dimension.NeutralNumbers[neutralLayer]; neutral++)
                            geneCodes.Add($"N{i+1}:{j}-N{neutralLayer+1}:{neutral}");
                    }
                }
            }

            return geneCodes;
        }
    }

}