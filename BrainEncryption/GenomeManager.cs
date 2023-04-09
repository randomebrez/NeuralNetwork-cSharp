using BrainEncryption.Abstraction;
using BrainEncryption.Abstraction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrainEncryption
{
    public class GenomeManager : IGenome
    {
        public Genome GenerateGenome(GenomeCaracteristics caracteristics)
        {
            var genome = new Genome(caracteristics.GeneNumber);
            for (int i = 0; i < caracteristics.GeneNumber; i++)
            {
                var geneCodeRandomIndex = StaticHelper.GetRandomValue(0, caracteristics.GeneCodes.Count - 1);
                var geneCode = caracteristics.GeneCodes[geneCodeRandomIndex];
                var gene = new Gene(geneCode, caracteristics.WeightBitArraySize);
                RandomizeGeneBytes(gene);
                genome.Genes[i] = gene;
            }
            return genome;
        }

        public HashSet<string> GetGeneCodes(NetworkCaracteristics caracteristics)
        {
            var geneCodes = new HashSet<string>();
            for (int input = 0; input < caracteristics.InputNumber; input++)
            {
                for (int output = 0; output < caracteristics.OutputNumber; output++)
                    geneCodes.Add($"I:{input}-O:{output}");
            }

            for (int i = 0; i < caracteristics.NeutralNumbers.Count(); i++)
            {
                for (int j = 0; j < caracteristics.NeutralNumbers[i]; j++)
                {
                    // Input links
                    for (int input = 0; input < caracteristics.InputNumber; input++)
                        geneCodes.Add($"I:{input}-N{i + 1}:{j}");

                    // Output links
                    for (int output = 0; output < caracteristics.OutputNumber; output++)
                        geneCodes.Add($"N{i + 1}:{j}-O:{output}");

                    // Other neutral layers links
                    for (int neutralLayer = i + 1; neutralLayer < caracteristics.NeutralNumbers.Count(); neutralLayer++)
                    {
                        for (int neutral = 0; neutral < caracteristics.NeutralNumbers[neutralLayer]; neutral++)
                            geneCodes.Add($"N{i + 1}:{j}-N{neutralLayer + 1}:{neutral}");
                    }
                }
            }

            return geneCodes;
        }

        public Genome GetGenomeFromString(string genomeString)
        {
            var splittedGenes = genomeString.Split('!');
            var geneNumber = splittedGenes.Length - 1;
            var genome = new Genome(geneNumber);
            for (int i = 0; i < geneNumber; i++)
                genome.Genes[i] = StringToGene(splittedGenes[i]);

            return genome;
        }


        public Genome CrossOver(GenomeCaracteristics caracteristics, Genome genomeDtoA, Genome genomeDtoB, int crossOverNumber)
        {
            var newGenome = new Genome(caracteristics.GeneNumber);
            var crossOver = false;

            // Binomial law with average of first success around (GenomeLength/crossOverNumber) : Loi de poisson
            var crossOverTreshold = (float)crossOverNumber / caracteristics.GeneNumber;

            for (int i = 0; i < caracteristics.GeneNumber; i++)
            {
                crossOver = StaticHelper.GetUniformProbability(100 * caracteristics.GeneNumber) < crossOverTreshold;

                if (crossOver)
                    newGenome.Genes[i] = genomeDtoB.Genes[i];
                else
                    newGenome.Genes[i] = genomeDtoA.Genes[i];
            }

            return newGenome.DeepCopy();
        }

        public Genome MutateGenome(Genome baseGenome, GenomeCaracteristics caracteristics, float geneMutationRate)
        {
            // Check mutation actually occured on returned object
            foreach (var gene in baseGenome.Genes)
            {
                var mutationOccur = StaticHelper.GetUniformProbability((int)(10 / geneMutationRate)) < geneMutationRate;
                if (mutationOccur == false)
                    continue;

                MutateGene(gene, caracteristics.GeneCodes);
            }
            return baseGenome;
        }

        public Brain TranslateGenome(NetworkCaracteristics networkCarac, Genome genome)
        {
            var brain = new Brain(networkCarac.OutputLayerId);
            var edges = new List<Edge>();
            //Read each gene
            foreach (var geneGroup in genome.Genes.GroupBy(t => t.EdgeIdentifier))
            {
                Edge edge = null;
                foreach (var gene in geneGroup)
                {
                    if (gene.IsActive == false)
                        continue;

                    if (edge == null)
                        edge = GetEdgeFromGene(networkCarac, gene, brain.Neurons);
                    else
                    {
                        edge.Weight += ComputeEdgeWeigh(gene);
                    }
                }
                if (edge != null && edge.Weight != 0)
                {
                    edge.Weight /= (float)geneGroup.Count();
                    edges.Add(edge);
                }
            }

            brain.Edges = edges;

            return brain;
        }


        private Edge GetEdgeFromGene(NetworkCaracteristics networkCarac, Gene gene, BrainNeurons neurons)
        {
            var NeuronIdentifiers = gene.EdgeIdentifier.Split('-');
            if (neurons.GetNeuronByName(NeuronIdentifiers[0]) == null)
                neurons.AddNeuron(BuildNeuron(networkCarac, NeuronIdentifiers[0]));
            if (neurons.GetNeuronByName(NeuronIdentifiers[1]) == null)
                neurons.AddNeuron(BuildNeuron(networkCarac, NeuronIdentifiers[1]));

            return new Edge
            {
                Identifier = gene.EdgeIdentifier,
                Origin = neurons.GetNeuronByName(NeuronIdentifiers[0]),
                Target = neurons.GetNeuronByName(NeuronIdentifiers[1]),
                Weight = ComputeEdgeWeigh(gene)
            };
        }

        private float ComputeEdgeWeigh(Gene gene)
        {
            var weighBits = gene.WeighBits;
            var weighResult = 0f;
            for (int i = 0; i < weighBits.Length; i++)
            {
                if (weighBits[i] == false)
                    continue;
                weighResult += (float)Math.Pow(2, i);
            }
            weighResult /= gene.EdgeWeightMaxValue;
            return gene.WeighSign ? weighResult : weighResult * (-1);
        }

        private Neuron BuildNeuron(NetworkCaracteristics networkCarac, string identifier)
        {
            var splitGene = identifier.Split(':');
            var neuronId = splitGene[1];
            switch (splitGene[0][0])
            {
                case 'I':
                    return new NeuronInput(int.Parse(neuronId), 0);
                case 'N':
                    var layerId = int.Parse(splitGene[0].Split('N')[0]);
                    return new NeuronNeutral(int.Parse(neuronId), layerId, networkCarac.Tanh90Percent);
                case 'O':
                    return new NeuronOutput(int.Parse(neuronId), networkCarac.OutputLayerId, networkCarac.Sigmoid90Percent);
            }
            return null;
        }

        private void RandomizeGeneBytes(Gene gene)
        {
            gene.IsActive = true; //StaticHelper.GetBooleanValue();
            gene.WeighSign = StaticHelper.GetBooleanValue();
            for (int i = 0; i < gene.WeighBits.Length; i++)
                gene.WeighBits[i] = StaticHelper.GetBooleanValue();
            gene.Bias = StaticHelper.GetUniformProbability();
        }

        private Gene StringToGene(string geneString)
        {
            var splittedGene = geneString.Split("|");
            var weighBytesNumber = splittedGene[1].Length - 1;
            var gene = new Gene(splittedGene[0], weighBytesNumber);
            gene.WeighSign = splittedGene[1][0] == '1';
            for (int i = 0; i < weighBytesNumber; i++)
                gene.WeighBits[i] = splittedGene[1][i + 1] == '1';

            gene.IsActive = true;
            return gene;
        }

        private void MutateGene(Gene gene, List<string> geneCodes, bool allowMutateActiveByte = false)
        {
            var maxValue = allowMutateActiveByte ? 3 : 2;

            var geneTypeToMutate = StaticHelper.GetRandomValue(0, maxValue);
            switch (geneTypeToMutate)
            {
                // Weigh bits encryption
                case 0:
                    var byteToMutate = StaticHelper.GetRandomValue(0, gene.WeighBits.Length - 1);
                    gene.WeighBits[byteToMutate] = !gene.WeighBits[byteToMutate];
                    break;
                // Sign bit encryption
                case 1:
                    gene.WeighSign = !gene.WeighSign;
                    break;
                // Edge encryption
                case 2:
                    var geneCodeRandomIndex = StaticHelper.GetRandomValue(0, geneCodes.Count - 1);
                    while (geneCodes[geneCodeRandomIndex] == gene.EdgeIdentifier)
                        geneCodeRandomIndex = StaticHelper.GetRandomValue(0, geneCodes.Count - 1);
                    gene.EdgeIdentifier = geneCodes[geneCodeRandomIndex];
                    //gene.Bias = StaticHelper.GetUniformProbability();
                    break;
                case 3:
                    gene.IsActive = !gene.IsActive;
                    break;
            }
        }
    }
}