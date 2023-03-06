using NeuralNetwork.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork.Helpers
{
    public static class GenomeHelper
    {
        #region Genome

        public static Genome GenerateGenome(int geneNumber, int weighBytesNumber, List<string> geneCodes)
        {
            var genome = new Genome(geneNumber);
            for (int i = 0; i < geneNumber; i++)
            {
                var geneCodeRandomIndex = StaticHelper.GetRandomValue(0, geneCodes.Count - 1);
                var geneCode = geneCodes[geneCodeRandomIndex];
                var gene = new Gene(geneCode, weighBytesNumber);
                gene.RandomizeBytes();
                genome.Genes[i] = gene;
            }
            return genome;
        }

        public static List<Vertex> TranslateGenome(this Genome genome, Dictionary<string, Neuron> availableNeurons)
        {
            var vertices = new List<Vertex>();
            //Read each gene
            foreach (var geneGroup in genome.Genes.GroupBy(t => t.VertexIdentifier))
            {
                Vertex vertex = null;
                foreach (var gene in geneGroup)
                {
                    if (gene.IsActive == false)
                        continue;

                    if (vertex == null)
                        vertex = gene.GetVertexEdgesFromGene(availableNeurons);
                    else
                        vertex.Weight *= ComputeVertexWeigh(gene);
                }
                if (vertex != null)
                    vertices.Add(vertex);
            }

            return vertices;
        }

        public static Genome CrossOver(this Genome genomeDtoA, Genome genomeDtoB)
        {
            var geneNumber = genomeDtoA.Genes.Length;
            var newGenome = new Genome(geneNumber);
            for (int i = 0; i < geneNumber; i++)
            {
                var getFromA = StaticHelper.GetBooleanValue();
                if (getFromA)
                    newGenome.Genes[i] = genomeDtoA.Genes[i];
                else
                    newGenome.Genes[i] = genomeDtoB.Genes[i];
            }
            return newGenome;
        }
        

        public static void MutateGenomeList(this List<Genome> genomes, List<string> geneCodes, float mutationRate = 0.001f, float geneChangeProbability = 0.1f)
        {
            foreach (var genome in genomes)
            {
                var mutationOccur = StaticHelper.GetUniformProbability() < mutationRate;
                if (mutationOccur == false)
                    continue;

                if (StaticHelper.GetUniformProbability() < geneChangeProbability)
                {
                    var geneCodeRandomIndex = StaticHelper.GetRandomValue(0, geneCodes.Count - 1);
                    var gene = new Gene(geneCodes[geneCodeRandomIndex], 4);
                    gene.RandomizeBytes();
                    genome.Genes[StaticHelper.GetRandomValue(0, genome.GeneNumber - 1)] = gene;
                }
                else
                    genome.Genes[StaticHelper.GetRandomValue(0, genome.GeneNumber - 1)].Mutate();
            }
        }

        public static void MutateGenome(this Genome genome, List<string> geneCodes, float mutationRate = 0.001f, float geneChangeProbability = 0.1f)
        {
            foreach (var gene in genome.Genes)
            {
                var mutationOccur = StaticHelper.GetUniformProbability() < mutationRate;
                if (mutationOccur == false)
                    continue;

                if (StaticHelper.GetUniformProbability() < geneChangeProbability)
                {
                    var geneCodeRandomIndex = StaticHelper.GetRandomValue(0, geneCodes.Count - 1);
                    var newGene = new Gene(geneCodes[geneCodeRandomIndex], 4);
                    newGene.RandomizeBytes();
                    genome.Genes[StaticHelper.GetRandomValue(0, genome.GeneNumber - 1)] = newGene;
                }
                else
                    gene.Mutate();
            }
        }

        private static Vertex GetVertexEdgesFromGene(this Gene gene, Dictionary<string, Neuron> availableNeurons)
        {
            var NeuronIdentifiers = gene.VertexIdentifier.Split('-');

            return new Vertex
            {
                Identifier = gene.VertexIdentifier,
                Origin = availableNeurons[NeuronIdentifiers[0]],
                Target = availableNeurons[NeuronIdentifiers[1]],
                Weight = ComputeVertexWeigh(gene)
            };
        }

        private static float ComputeVertexWeigh(this Gene gene)
        {
            var weighBytes = gene.WeighBytes;
            var weighResult = 0f;
            for (int i = 0; i < weighBytes.Length; i++)
            {
                if (weighBytes[i] == false)
                    continue;
                weighResult += (float)Math.Pow(2, i);
            }
            weighResult /= gene.WeightBytesMaxValue;
            return gene.WeighSign ? weighResult : weighResult * (-1);
        }

        #endregion


        #region Gene

        private static void RandomizeBytes(this Gene gene)
        {
            gene.IsActive = true; //StaticHelper.GetBooleanValue();
            gene.WeighSign = StaticHelper.GetBooleanValue();
            for (int i = 0; i < gene.WeighBytes.Length; i++)
                gene.WeighBytes[i] = StaticHelper.GetBooleanValue();
            gene.Bias = StaticHelper.GetUniformProbability();
        }

        private static void Mutate(this Gene gene, bool allowMutateActiveByte = false)
        {
            var maxValue = allowMutateActiveByte ? 3 : 2;

            var geneTypeToMutate = StaticHelper.GetRandomValue(0, maxValue);
            switch (geneTypeToMutate)
            {
                // Bytes that encode weigh
                case 0:
                    var byteToMutate = StaticHelper.GetRandomValue(0, gene.WeighBytes.Length - 1);
                    gene.WeighBytes[byteToMutate] = !gene.WeighBytes[byteToMutate];
                    break;
                // Byte that encode weigh sign
                case 1:
                    gene.WeighSign = !gene.WeighSign;
                    break;
                // Byte that encode Bias
                case 2:
                    gene.Bias = StaticHelper.GetUniformProbability();
                    break;
                case 3:
                    gene.IsActive = !gene.IsActive;
                    break;
            }
        }

        #endregion
    }
}
