using NeuralNetwork.Interfaces;
using NeuralNetwork.Interfaces.Model;
using NeuralNetwork.Helpers;
using NeuralNetwork.Implementations;
using System.Collections.Generic;
using System.Linq;
using System;

namespace NeuralNetwork.Managers
{
    public class PopulationManager : IPopulation
    {
        public HashSet<string> GeneCodes { get; private set; }
        private BrainNeurons _baseNeurons;
        private BrainNeurons _deepCopiedNeurons => _baseNeurons.DeepCopy();

        private int _maximumTryForBrainGeneration = 10;

        private readonly NetworkCaracteristics _dimension;

        public PopulationManager(NetworkCaracteristics dimension)
        {
            _dimension = dimension;
            _baseNeurons = BrainHelper.GetBaseNeurons(_dimension);
            GeneCodes = GetGeneCodes();
        }

        public Brain[] GenerateFirstGeneration(int childNumber)
        {
            var newBrains = new Brain[childNumber];
            for (int i = 0; i < childNumber; i++)
            {
                var newBrain = BrainHelper.GenerateRandomBrain(_dimension, _deepCopiedNeurons, GeneCodes.ToList());
                if (newBrain != null)
                    newBrains[i] = newBrain;
            }
            return newBrains;
        }

        public Brain[] GenerateNewGeneration(int childNumber, List<Brain> selectedBrains)
        {
            var newBrains = new Brain[childNumber];

            var currentBrainCount = 0;
            var fertileBrains = selectedBrains;
            while (currentBrainCount < childNumber && fertileBrains.Count > 1)
            {
                newBrains[currentBrainCount] = GetChild(fertileBrains);
                fertileBrains = fertileBrains.Where(t => t.UseForChildCounter < t.MaxChildNumber).ToList();
                currentBrainCount++;
            }
            var bestBrains = selectedBrains.OrderByDescending(t => t.MaxChildNumber).ToList();
            var brainsToComplete = MathF.Min(childNumber - currentBrainCount, bestBrains.Count);
            for(int i = 0; i < brainsToComplete; i++)
            {
                bestBrains[i].UseForChildCounter = 0;
                newBrains[currentBrainCount] = bestBrains[i];
                currentBrainCount++;
            }    
            for (int i = currentBrainCount; i < childNumber; i++)
                newBrains[i] = GenerateRandomBrain();
            return newBrains;
        }

        public Brain[] GetBrainFromGenomes(List<string> genomeStrings)
        {
            var result = new Brain[genomeStrings.Count];

            for (int i = 0; i < genomeStrings.Count; i++)
            {
                var genome = GenomeHelper.GetGenomeFromString(genomeStrings[i]);
                result[i] = genome.GenerateBrainFromGenome(_deepCopiedNeurons, Guid.Empty, Guid.Empty);
            }

            return result;
        }

        private Brain GetChild(List<Brain> selectedBrains)
        {
            var brain = new Brain();
            bool exitWhile = false;
            var tryCount = 0;
            while(exitWhile == false && tryCount < _maximumTryForBrainGeneration)
            {
                var firstindex = StaticHelper.GetRandomValue(0, selectedBrains.Count - 1);
                var brainA = selectedBrains[firstindex];
                var secondIndex = firstindex;
                while (secondIndex == firstindex)
                    secondIndex = StaticHelper.GetRandomValue(0, selectedBrains.Count - 1);
                var brainB = selectedBrains[secondIndex];

                var mixedGenome = brainA.Genome.CrossOver(brainB.Genome);
                mixedGenome.MutateGenome(GeneCodes.ToList());
                brain = mixedGenome.GenerateBrainFromGenome(_deepCopiedNeurons, brainA.UniqueIdentifier, brainB.UniqueIdentifier);
                exitWhile = brain.IsBrainValid();
                if (exitWhile)
                {
                    brainA.UseForChildCounter++;
                    brainB.UseForChildCounter++;
                }
                tryCount++;
            }
            return brain;
        }

        private Brain GenerateRandomBrain()
        {
            var validBrain = false;
            var currentTry = 0;
            var brain = new Brain
            {
                UniqueIdentifier = Guid.NewGuid(),
            };
            while (validBrain == false && currentTry < _maximumTryForBrainGeneration)
            {
                currentTry++;
                brain = BrainHelper.GenerateRandomBrain(_dimension, _deepCopiedNeurons, GeneCodes.ToList());
                validBrain = brain != null;
            }

            return brain;
        }

        private HashSet<string> GetGeneCodes()
        {
            var geneCodes = new HashSet<string>();
            for (int input = 0; input < _dimension.InputNumber; input++)
            {
                for (int output = 0; output < _dimension.OutputNumber; output++)
                    geneCodes.Add($"I:{input}-O:{output}");
            }

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