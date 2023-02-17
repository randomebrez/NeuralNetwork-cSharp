using NeuralNetwork.Interfaces.Model;

namespace NeuralNetwork.Helpers
{
    public static class BrainHelper
    {
        public static Brain? GenerateRandomBrain(NetworkCaracteristics dimension, BrainNeurons availableNeurons, List<string> geneCodes, Dictionary<string, Neuron> neuronsDict)
        {
            var newBrain = new Brain
            {
                Neurons = availableNeurons
            };

            bool brainValid = false;
            int maxTry = 10;
            int currentTry = 0;
            while (!brainValid && currentTry < maxTry)
            {
                currentTry++;

                // Generate genome
                var genome = GenomeHelper.GenerateGenome(dimension.GeneNumber, dimension.WeighBytesNumber, geneCodes);
                newBrain.Genome = genome;

                // Translate Genome into vertices
                newBrain.Vertices = genome.TranslateGenome(neuronsDict);

                // Check that brain is valid : at least 1 path from an input to an output
                brainValid = newBrain.IsBrainValid();
            }

            if (brainValid)
                return newBrain;

            Console.WriteLine("Not a valid brain");
            return null;
        }

        public static Brain? GenerateRandomBrainWithMatrix(NetworkCaracteristics dimension, BrainNeurons availableNeurons, List<string> geneCodes, Dictionary<string, Neuron> neuronsDict)
        {
            var newBrain = new Brain
            {
                Neurons = availableNeurons
            };

            bool brainValid = true;
            int maxTry = 10;
            int currentTry = 0;
            while (!brainValid && currentTry < maxTry)
            {
                currentTry++;

                // Generate genome
                var genome = GenomeHelper.GenerateGenome(dimension.GeneNumber, dimension.WeighBytesNumber, geneCodes);
                newBrain.Genome = genome;

                // Translate Genome into vertices
                newBrain.Matrices = genome.TranslateGenomeToMatrix(dimension, neuronsDict);

                // Check that brain is valid : at least 1 path from an input to an output
                //brainValid = newBrain.IsBrainValid();
            }

            if (brainValid)
                return newBrain;

            Console.WriteLine("Not a valid brain");
            return null;
        }

        public static Brain GenerateBrainFromGenome(this Genome genome, BrainNeurons availableNeurons, Dictionary<string, Neuron> neuronsDict)
        {
            var newBrain = new Brain
            {
                Neurons = availableNeurons
            };
            newBrain.Vertices = genome.TranslateGenome(neuronsDict);
            newBrain.Genome = genome;

            // Check that brain is valid : at least 1 path from an input to an output
            //var validBrain = newBrain.IsBrainValid();

            return newBrain;
            //return validBrain ? newBrain : null;
        }

        public static Brain GenerateBrainFromGenomeWithMatrix(this Genome genome, NetworkCaracteristics dimension, BrainNeurons availableNeurons, Dictionary<string, Neuron> neuronsDict)
        {
            var newBrain = new Brain
            {
                Neurons = availableNeurons
            };
            newBrain.Matrices = genome.TranslateGenomeToMatrix(dimension, neuronsDict);
            newBrain.Genome = genome;

            // Check that brain is valid : at least 1 path from an input to an output
            //var validBrain = newBrain.IsBrainValid();

            return newBrain;
            //return validBrain ? newBrain : null;
        }

        public static bool IsBrainValid(this Brain brain)
        {
            // Vertices whose target is an 'output'
            var outputTargeted = brain.Vertices.Where(t => t.Target.Layer == 2).ToList();
            // If any whose origin is an 'input' : OK
            if (outputTargeted.Any(t => t.Origin.Layer == 0))
                return true;
            // If neither whose origin is an 'neutral' : NOK
            if (outputTargeted.Any(t => t.Origin.Layer == 1) == false)
                return false;
            // Is any vertex whose origin is an 'input' && target is one of the 'neutral' that targets an 'output'
            var neutralToReach = outputTargeted.Select(t => t.Origin.UniqueId).ToHashSet();
            return brain.Vertices.Where(t => t.Origin.Layer == 0).Any(t => neutralToReach.Contains(t.Target.UniqueId));
        }

        public static BrainNeurons GetBaseNeurons(NetworkCaracteristics dimension)
        {
            var result = new BrainNeurons();

            for (int i = 0; i < dimension.InputNumber; i++)
                result.Inputs.Add(new NeuronInput(i, 0));

            for (int j = 0; j < dimension.NeutralNumber; j++)
                result.Neutrals.Add(new NeuronNeutral(j, 1));

            for (int k = 0; k < dimension.OutputNumber; k++)
                result.Outputs.Add(new NeuronOutput(k, 2));

            return result;
        }
    }
}
