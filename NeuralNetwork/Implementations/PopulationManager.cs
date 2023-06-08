using NeuralNetwork.Interfaces;
using NeuralNetwork.Interfaces.Model;
using NeuralNetwork.Helpers;
using System.Collections.Generic;
using System.Linq;
using System;
using BrainEncryption.Abstraction;
using BrainEncryption;
using NeuralNetwork.Interfaces.Model.Etc;

namespace NeuralNetwork.Managers
{
    public class PopulationManager : IPopulationManager
    {
        //OU_TEST : Only used by test prog
        private IGenome _genomeEncryption;
        private IBrainBuilder _brainBuilder;

        public PopulationManager()
        {
            _genomeEncryption = new GenomeEncrypter();
            _brainBuilder = new BrainBuilder();
        }


        public Unit[] GenerateFirstGeneration(int childNumber, List<BrainCaracteristics> brainCaracteristics)
        {
            return GenerateRandomUnits(childNumber, brainCaracteristics);
        }

        public Unit[] GenerateNewGeneration(int childNumber, List<Unit> selectedUnits, List<BrainCaracteristics> brainCaracteristics, ReproductionCaracteristics reproductionCaracteristics)
        {
            var newUnits = new Unit[childNumber];

            var currentUnitCount = 0;
            var fertileUnits = selectedUnits;
            while (currentUnitCount < childNumber && fertileUnits.Count > 1)
            {
                newUnits[currentUnitCount] = GetChild(fertileUnits, brainCaracteristics, reproductionCaracteristics.CrossOverNumber, reproductionCaracteristics.MutationRate);
                fertileUnits = fertileUnits.Where(t => t.ChildrenNumber < t.MaxChildNumber).ToList();
                currentUnitCount++;
            }
            var bestUnits = selectedUnits.OrderByDescending(t => t.MaxChildNumber).ToList();
            var unitsToComplete = MathF.Min(childNumber - currentUnitCount, bestUnits.Count);
            for (int i = 0; i < unitsToComplete; i++)
            {
                bestUnits[i].ChildrenNumber = 0;
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
                    Key = brainCaracteristic.TemplateName,
                    Brain = brain.ToPublic(),
                    Genome = genome.ToPublic(),
                    Caracteristics = brainCaracteristic
                };
                unit.Brains.Add(brainCaracteristic.TemplateName, pair);
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
                var genomeCarac = brainCarac.GenomeCaracteristics.ToGenomeCaracteristic();

                //Build brain structure
                var brain = _brainBuilder.BuildBrain(mappedBrainCarac);

                // Cross over genomes
                var genomeA = _genomeEncryption.GetGenomeFromString(parentA.Brains[brainCarac.TemplateName].Genome.GenomeToString);
                var genomeB = _genomeEncryption.GetGenomeFromString(parentB.Brains[brainCarac.TemplateName].Genome.GenomeToString);
                var mixedGenome = _genomeEncryption.CrossOver(genomeCarac, genomeA, genomeB, crossOverNumber);

                // Apply mutation on generated genome
                var mutatedGenome = _genomeEncryption.MutateGenome(mixedGenome, geneCodes, mutationRate);

                // Fill in brain's edges
                _genomeEncryption.TranslateGenome(brain, mutatedGenome);

                var pair = new BrainGenomePair
                {
                    Key = brainCarac.TemplateName,
                    Brain = brain.ToPublic(),
                    Genome = mutatedGenome.ToPublic(),
                    Caracteristics = brainCarac
                };
                unit.Brains.Add(brainCarac.TemplateName, pair);
                unit.ParentA = parentA.Identifier;
                unit.ParentB = parentB.Identifier;
            }

            parentA.ChildrenNumber++;
            parentB.ChildrenNumber++;
            return unit;
        }

        private Unit[] GenerateRandomUnits(int childNumber, List<BrainCaracteristics> brainCaracteristics)
        {
            var result = new Unit[childNumber];
            var firstLoop = true;
            foreach (var brainCarac in brainCaracteristics)
            {
                var geneCodes = _genomeEncryption.GetGeneCodes(brainCarac.ToNetworkCaracteristic());
                var genomeCarac = brainCarac.GenomeCaracteristics.ToGenomeCaracteristic();
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
                    var newGenome = _genomeEncryption.GenerateGenome(genomeCarac, geneCodes);
                    // Translate it to generate brain's edges
                    _genomeEncryption.TranslateGenome(brain, newGenome);

                    var pair = new BrainGenomePair
                    {
                        Key = brainCarac.TemplateName,
                        Brain = brain.ToPublic(),
                        Genome = newGenome.ToPublic(),
                        Caracteristics = brainCarac
                    };
                    unit.Brains.Add(brainCarac.TemplateName, pair);
                }

                firstLoop = false;
            }
            return result;
        }

        public void DuplicateBrain(Unit unit, string brainKey, string newBrainKey)
        {
            var brainGenome = unit.Brains[brainKey].Genome;
            var brain = _brainBuilder.BuildBrain(unit.Brains[brainKey].Caracteristics.ToNetworkCaracteristic());
            _genomeEncryption.TranslateGenome(brain, brainGenome.ToInternal());
            var pair = new BrainGenomePair
            {
                Key = newBrainKey,
                Brain = brain.ToPublic(),
                Genome = brainGenome,
                Caracteristics = unit.Brains[brainKey].Caracteristics
            };
            unit.Brains.Add(newBrainKey, pair);
        }
    }
}