using BrainEncryption.Abstraction;
using BrainEncryption.Abstraction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrainEncryption
{
    public class GenomeEncrypter : IGenome
    {
        public Genome GenerateGenome(GenomeCaracteristics caracteristics, HashSet<string> geneCodes)
        {
            var genome = new Genome(caracteristics.GeneNumber);
            var geneCodeList = geneCodes.ToList();

            for (int i = 0; i < caracteristics.GeneNumber; i++)
            {
                var geneCodeRandomIndex = StaticHelper.GetRandomValue(0, geneCodeList.Count - 1);
                var geneCode = geneCodeList[geneCodeRandomIndex];
                var gene = new Gene(geneCode, caracteristics.WeightBitArraySize);
                RandomizeGeneBytes(gene);
                genome.Genes[i] = gene;
            }
            return genome;
        }

        public HashSet<string> GetGeneCodes(NetworkCaracteristics caracteristics)
        {
            var geneCodes = new HashSet<string>();
            for (int input = 0; input < caracteristics.InputLayer.NeuronNumber; input++)
            {
                for (int output = 0; output < caracteristics.Outputlayer.NeuronNumber; output++)
                    geneCodes.Add($"I:{input}-O:{output}");
            }

            for (int i = 0; i < caracteristics.NeutralLayers.Count(); i++)
            {
                var currentNeutralLayer = caracteristics.NeutralLayers[i];
                for (int j = 0; j < currentNeutralLayer.NeuronNumber; j++)
                {
                    // Input links
                    for (int input = 0; input < caracteristics.InputLayer.NeuronNumber; input++)
                        geneCodes.Add($"I:{input}-N{currentNeutralLayer.LayerId}:{j}");

                    // Output links
                    for (int output = 0; output < caracteristics.Outputlayer.NeuronNumber; output++)
                        geneCodes.Add($"N{currentNeutralLayer.LayerId}:{j}-O:{output}");

                    // Other neutral layers links (Neutral layerIds start from 1)
                    for (int neutralLayer = currentNeutralLayer.LayerId; neutralLayer < caracteristics.NeutralLayers.Count(); neutralLayer++)
                    {
                        for (int neutral = 0; neutral < caracteristics.NeutralLayers[neutralLayer].NeuronNumber; neutral++)
                            geneCodes.Add($"N{currentNeutralLayer.LayerId}:{j}-N{neutralLayer + 1}:{neutral}");
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
            var crossOverTreshold = (float)(crossOverNumber + 1)/ caracteristics.GeneNumber;

            for (int i = 0; i < caracteristics.GeneNumber; i++)
            {
                if (StaticHelper.GetUniformProbability(100 * caracteristics.GeneNumber) < crossOverTreshold)
                    crossOver = !crossOver;

                if (crossOver)
                    newGenome.Genes[i] = genomeDtoB.Genes[i];
                else
                    newGenome.Genes[i] = genomeDtoA.Genes[i];
            }

            return newGenome.DeepCopy();
        }

        public Genome MutateGenome(Genome baseGenome, HashSet<string> geneCodes, float geneMutationRate)
        {
            // Check mutation actually occured on returned object
            foreach (var gene in baseGenome.Genes)
            {
                var mutationOccur = StaticHelper.GetUniformProbability((int)(10 / geneMutationRate)) < geneMutationRate;
                if (mutationOccur == false)
                    continue;

                MutateGene(gene, geneCodes.ToList());
            }
            return baseGenome;
        }

        public void TranslateGenome(Brain brain, Genome genome)
        {   
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
                        edge = GetEdgeFromGene(gene, brain.Neurons);
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
        }


        private Edge GetEdgeFromGene(Gene gene, BrainNeurons neurons)
        {
            var neuronIdentifiers = gene.EdgeIdentifier.Split('-');

            return new Edge
            {
                Identifier = gene.EdgeIdentifier,
                Origin = neurons.GetNeuronByName(neuronIdentifiers[0]),
                Target = neurons.GetNeuronByName(neuronIdentifiers[1]),
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