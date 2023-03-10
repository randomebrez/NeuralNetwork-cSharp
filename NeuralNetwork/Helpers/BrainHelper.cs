using NeuralNetwork.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork.Helpers
{
    public static class BrainHelper
    {
        public static Brain? GenerateRandomBrain(NetworkCaracteristics dimension, BrainNeurons availableNeurons, List<string> geneCodes)
        {
            var newBrain = new Brain
            {
                UniqueIdentifier = Guid.NewGuid(),
                Neurons = availableNeurons
            };

            // Generate genome
            var genome = GenomeHelper.GenerateGenome(dimension.GeneNumber, dimension.WeighBytesNumber, geneCodes);
            newBrain.Genome = genome;

            // Translate Genome into vertices
            newBrain.Vertices = genome.TranslateGenome(newBrain.Neurons);

            // Check that brain is valid : at least 1 path from an input to an output
            if (newBrain.IsBrainValid())
                return newBrain;

            return null;
        }

        public static Brain GenerateBrainFromGenome(this Genome genome, BrainNeurons availableNeurons, Guid parentA, Guid parentB)
        {
            var newBrain = new Brain
            {
                UniqueIdentifier = Guid.NewGuid(),
                Neurons = availableNeurons,
                FirstParent = parentA,
                SecondParent = parentB
            };
            newBrain.Vertices = genome.TranslateGenome(newBrain.Neurons);
            newBrain.Genome = genome;
            //Check BrainValid

            return newBrain;
        }

        public static bool IsBrainValid(this Brain brain)
        {
            return true;
            //// Vertices whose target is an 'output'
            //var outputTargeted = brain.Vertices.Where(t => t.Target.Layer == 2).ToList();
            //// If any whose origin is an 'input' : OK
            //if (outputTargeted.Any(t => t.Origin.Layer == 0))
            //    return true;
            //// If neither whose origin is an 'neutral' : NOK
            //if (outputTargeted.Any(t => t.Origin.Layer == 1) == false)
            //    return false;
            //// Is any vertex whose origin is an 'input' && target is one of the 'neutral' that targets an 'output'
            //var neutralToReach = outputTargeted.Select(t => t.Origin.UniqueId).ToHashSet();
            //return brain.Vertices.Where(t => t.Origin.Layer == 0).Any(t => neutralToReach.Contains(t.Target.UniqueId));
        }

        public static BrainNeurons GetBaseNeurons(NetworkCaracteristics dimension)
        {
            var result = new BrainNeurons();

            for (int i = 0; i < dimension.InputNumber; i++)
                result.Inputs.Add(new NeuronInput(i, 0));

            for (int j = 0; j < dimension.NeutralNumbers.Count(); j++)
            {
                for (int i = 0; i < dimension.NeutralNumbers[j]; i++)
                    result.Neutrals.Add(new NeuronNeutral(i, j+1));
            }

            for (int k = 0; k < dimension.OutputNumber; k++)
                result.Outputs.Add(new NeuronOutput(k, 1 + dimension.NeutralNumbers.Count()));

            return result;
        }
    }
}
