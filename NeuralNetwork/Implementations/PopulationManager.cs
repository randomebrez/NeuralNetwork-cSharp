using NeuralNetwork.Interfaces;
using NeuralNetwork.Interfaces.Model;
using NeuralNetwork.Helpers;
using System.Collections.Generic;
using System.Linq;
using System;
using BrainEncryption.Abstraction;
using BrainEncryption;

namespace NeuralNetwork.Managers
{
    public class PopulationManager : IPopulationManager
    {
        private IGenome _genomeEncryption;
        private IBrainBuilder _brainBuilder;

        public PopulationManager()
        {
            _genomeEncryption = new GenomeManager();
            _brainBuilder = new BrainBuilder();
        }


        public Unit[] GenerateFirstGeneration(int childNumber, List<BrainCaracteristics> brainCaracteristics)
        {
            return GenerateRandomUnits(childNumber, brainCaracteristics);
        }

        public Unit[] GenerateNewGeneration(int childNumber, List<Unit> selectedUnits, List<BrainCaracteristics> brainCaracteristics, int crossOverNumber, float mutationRate)
        {
            var newUnits = new Unit[childNumber];

            var currentUnitCount = 0;
            var fertileUnits = selectedUnits;
            while (currentUnitCount < childNumber && fertileUnits.Count > 1)
            {
                newUnits[currentUnitCount] = GetChild(fertileUnits, brainCaracteristics, crossOverNumber, mutationRate);
                fertileUnits = fertileUnits.Where(t => t.UseForChildCounter < t.MaxChildNumber).ToList();
                currentUnitCount++;
            }
            var bestUnits = selectedUnits.OrderByDescending(t => t.MaxChildNumber).ToList();
            var unitsToComplete = MathF.Min(childNumber - currentUnitCount, bestUnits.Count);
            for (int i = 0; i < unitsToComplete; i++)
            {
                bestUnits[i].UseForChildCounter = 0;
                newUnits[currentUnitCount] = bestUnits[i];
                currentUnitCount++;
            }
            if (childNumber - currentUnitCount > 0)
            {
                var randomUnits = GenerateRandomUnits(childNumber - currentUnitCount, brainCaracteristics);
                for (int i = currentUnitCount; i < childNumber; i++)
                    newUnits[i] = randomUnits[i - currentUnitCount];
            }
            return newUnits;
        }

        public Unit[] GetUnitFromGenomes(BrainCaracteristics brainCaracteristic, List<string> genomeStrings)
        {
            var units = new Unit[genomeStrings.Count];
            for (int i = 0; i < genomeStrings.Count; i++)
            {
                var unit = new Unit();
                var brain = _brainBuilder.BuildBrain(brainCaracteristic.ToNetworkCaracteristic());
                var genome = _genomeEncryption.GetGenomeFromString(genomeStrings[i]);
                _genomeEncryption.TranslateGenome(brain, genome);

                var pair = new BrainGenomePair
                {
                    Key = brainCaracteristic.Name,
                    Brain = brain.ToPublic(),
                    Genome = genome.ToPublic()
                };
                unit.Brains.Add(brainCaracteristic.Name, pair);
            }

            return units;
        }



        private Unit GetChild(List<Unit> selectedUnits, List<BrainCaracteristics> brainCaracteristics, int crossOverNumber, float mutationRate)
        {
            var unit = new Unit();
            var firstindex = Helpers.StaticHelper.GetRandomValue(0, selectedUnits.Count - 1);
            var parentA = selectedUnits[firstindex];
            var secondIndex = firstindex;
            while (secondIndex == firstindex)
                secondIndex = Helpers.StaticHelper.GetRandomValue(0, selectedUnits.Count - 1);
            var parentB = selectedUnits[secondIndex];

            foreach (var brainCarac in brainCaracteristics)
            {
                var mappedBrainCarac = brainCarac.ToNetworkCaracteristic();
                var geneCodes = _genomeEncryption.GetGeneCodes(mappedBrainCarac);
                var genomeCarac = brainCarac.GenomeCaracteristics.ToGenomeCaracteristic(geneCodes);

                //Build brain structure
                var brain = _brainBuilder.BuildBrain(mappedBrainCarac);

                // Cross over genomes
                var genomeA = _genomeEncryption.GetGenomeFromString(parentA.Brains[brainCarac.Name].Genome.GenomeToString);
                var genomeB = _genomeEncryption.GetGenomeFromString(parentB.Brains[brainCarac.Name].Genome.GenomeToString);
                var mixedGenome = _genomeEncryption.CrossOver(genomeCarac, genomeA, genomeB, crossOverNumber);

                // Apply mutation on generated genome
                var mutatedGenome = _genomeEncryption.MutateGenome(mixedGenome, genomeCarac, mutationRate);

                // Fill in brain's edges
                _genomeEncryption.TranslateGenome(brain, mutatedGenome);

                var pair = new BrainGenomePair
                {
                    Key = brainCarac.Name,
                    Brain = brain.ToPublic(),
                    Genome = mutatedGenome.ToPublic()
                };
                unit.Brains.Add(brainCarac.Name, pair);
                unit.ParentA = parentA.Identifier;
                unit.ParentB = parentB.Identifier;
            }

            parentA.UseForChildCounter++;
            parentB.UseForChildCounter++;
            return unit;
        }

        private Unit[] GenerateRandomUnits(int childNumber, List<BrainCaracteristics> brainCaracteristics)
        {
            var result = new Unit[childNumber];
            var firstLoop = true;
            foreach (var brainCarac in brainCaracteristics)
            {
                var geneCodes = _genomeEncryption.GetGeneCodes(brainCarac.ToNetworkCaracteristic());
                var genomeCarac = brainCarac.GenomeCaracteristics.ToGenomeCaracteristic(geneCodes);
                var networkCarac = brainCarac.ToNetworkCaracteristic();
                for (int i = 0; i < childNumber; i++)
                {
                    // ToDo : Possibility to inject MaxChildNumber property
                    if (firstLoop)
                        result[i] = new Unit();

                    var unit = result[i];

                    // Build brain neurons
                    var brain = _brainBuilder.BuildBrain(networkCarac);
                    // Generate genome
                    var newGenome = _genomeEncryption.GenerateGenome(genomeCarac);
                    // Translate it to generate brain's edges
                    _genomeEncryption.TranslateGenome(brain, newGenome);

                    var pair = new BrainGenomePair
                    {
                        Key = brainCarac.Name,
                        Brain = brain.ToPublic(),
                        Genome = newGenome.ToPublic()
                    };
                    unit.Brains.Add(brainCarac.Name, pair);
                }

                firstLoop = false;
            }
            return result;
        }
    }
}