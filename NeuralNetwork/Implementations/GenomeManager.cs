using BrainEncryption;
using BrainEncryption.Abstraction;
using NeuralNetwork.Abstraction;
using NeuralNetwork.Abstraction.Model;
using System.Collections.Generic;
using Genome = NeuralNetwork.Abstraction.Model.Genome;

namespace NeuralNetwork.Implementations
{
    public class GenomeManager : IGenomeManager
    {
        private IGenomeEncrypter _genomeEncryption;
        private IBrainBuilder _brainBuilder;

        public GenomeManager()
        {
            _genomeEncryption = new GenomeEncrypter();
            _brainBuilder = new BrainBuilder();
        }

        // Get genomes from caracteristics
        // Generate geneCodes
        public List<Genome> GenomesListGet(int number, BrainCaracteristics caracteristics)
        {
            var result = new List<Genome>();

            var genomeCarac = caracteristics.GenomeCaracteristics.ToGenomeCaracteristic();
            var networkCarac = caracteristics.ToNetworkCaracteristic();
            var geneCodes = _genomeEncryption.GeneCodesList(networkCarac);
            for(int i = 0; i < number; i ++)
            {
                // Generate genome
                result.Add(_genomeEncryption.GenomeGenerate(genomeCarac, geneCodes).ToPublic());
            }

            return result;
        }

        // Croos over 2 genomes
        public Genome GenomeCrossOverGet(Genome genomeA, Genome genomeB, BrainCaracteristics caracteristics, int crossOverNumber, float mutationRate)
        {
            var mappedBrainCarac = caracteristics.ToNetworkCaracteristic();
            var geneCodes = _genomeEncryption.GeneCodesList(mappedBrainCarac);
            var genomeCarac = caracteristics.GenomeCaracteristics.ToGenomeCaracteristic();

            // CrossOver genomes
            var mixedGenome = _genomeEncryption.GenomeCrossOver(genomeCarac, genomeA.ToInternal(), genomeB.ToInternal(), crossOverNumber);

            // Apply mutation on generated genome        
            return _genomeEncryption.GenomeMutate(mixedGenome, geneCodes, mutationRate).ToPublic();
        }

        // Generate units to client use
        // Translate the GenomeGraph in a BrainGraph that is then used by client for calculation
        public Unit[] UnitsFromGenomeGraphList(List<GenomeGraph> genomeGraphs)
        {
            var number = genomeGraphs.Count;
            var units = new Unit[number];
            for(int i = 0; i < number; i++)
            {
                var unit = new Unit();
                unit.GenomeGraph = genomeGraphs[i];
                unit.BrainGraph = GenomeGraphToBrainGraphTranslate(genomeGraphs[i]);
                units[i] = unit;
            }

            return units;
        }

        // Translate a genome into a brain
        public BrainGenomePair GenomeToBrainTranslate(Genome genome, BrainCaracteristics caracteristics)
        {
            var brain = _brainBuilder.BrainBuild(caracteristics.ToNetworkCaracteristic());
            _genomeEncryption.GenomeTranslate(brain, genome.ToInternal());

            return new BrainGenomePair
            {
                Genome = genome,
                Brain = brain.ToPublic(),
                Caracteristics = caracteristics
            };
        }

        // Translate a GenomeGraph into a BrainGraph
        // Meaning it translates every genome of the GenomeGraph into a brain, then create the edges
        private BrainGraph GenomeGraphToBrainGraphTranslate(GenomeGraph graph)
        {
            var brainGraph = new BrainGraph();

            // Translate genomes into brains
            foreach(var genomeNode in graph.GenomeNodes)
            {
                var brainGenomePair = GenomeToBrainTranslate(genomeNode.Genome, genomeNode.Caracteristics);
                brainGraph.BrainNodes.Add(genomeNode.Caracteristics.BrainName, brainGenomePair);

                if (genomeNode.Caracteristics.IsDecisionBrain)
                    brainGraph.DecisionBrain = brainGenomePair.Brain;
            }

            // Build edges
            foreach(var genomeEdge in graph.GenomeEdges)
            {
                brainGraph.BrainEdges.Add(genomeEdge.Key, new List<BrainGenomePair>());
                foreach (var linkedGenome in genomeEdge.Value)
                    brainGraph.BrainEdges[genomeEdge.Key].Add(brainGraph.BrainNodes[linkedGenome]);
            }
            return brainGraph;
        }
    }
}
