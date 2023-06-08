using BrainEncryption;
using BrainEncryption.Abstraction;
using NeuralNetwork.Helpers;
using NeuralNetwork.Interfaces;
using NeuralNetwork.Interfaces.Model;
using System.Collections.Generic;
using Genome = NeuralNetwork.Interfaces.Model.Genome;
using Brain = NeuralNetwork.Interfaces.Model.Brain;

namespace NeuralNetwork.Implementations
{
    public class GenomeManager : IGenomeManager
    {
        private IGenome _genomeEncryption;
        private IBrainBuilder _brainBuilder;

        public GenomeManager()
        {
            _genomeEncryption = new GenomeEncrypter();
            _brainBuilder = new BrainBuilder();
        }

        public List<Genome> GenomesListGet(int number, BrainCaracteristics caracteristics)
        {
            var result = new List<Genome>();

            var genomeCarac = caracteristics.GenomeCaracteristics.ToGenomeCaracteristic();
            var networkCarac = caracteristics.ToNetworkCaracteristic();
            var geneCodes = _genomeEncryption.GetGeneCodes(networkCarac);
            for(int i = 0; i < number; i ++)
            {
                // Generate genome
                result.Add(_genomeEncryption.GenerateGenome(genomeCarac, geneCodes).ToPublic());
            }

            return result;
        }

        public Genome GenomeCroosOverGet(Genome genomeA, Genome genomeB, BrainCaracteristics caracteristics, int crossOverNumber, float mutationRate)
        {
            var mappedBrainCarac = caracteristics.ToNetworkCaracteristic();
            var geneCodes = _genomeEncryption.GetGeneCodes(mappedBrainCarac);
            var genomeCarac = caracteristics.GenomeCaracteristics.ToGenomeCaracteristic();

            // CrossOver genomes
            var mixedGenome = _genomeEncryption.CrossOver(genomeCarac, genomeA.ToInternal(), genomeB.ToInternal(), crossOverNumber);

            // Apply mutation on generated genome        
            return _genomeEncryption.MutateGenome(mixedGenome, geneCodes, mutationRate).ToPublic();
        }
         
        public Brain GenomeToBrainTranslate(Genome genome, BrainCaracteristics caracteristics)
        {
            var brain = _brainBuilder.BuildBrain(caracteristics.ToNetworkCaracteristic());
            _genomeEncryption.TranslateGenome(brain, genome.ToInternal());

            return brain.ToPublic();
        }

        public Unit[] UnitsFromGenomeGraphList(List<GenomeGraph> genomeGraphs)
        {
            var number = genomeGraphs.Count;
            var units = new Unit[number];
            for(int i = 0; i < number; i++)
            {
                var unit = new Unit();
                unit.GenomeGraph = genomeGraphs[i];
                unit.BrainGraph = GetBrainGraphFromGenomeGraph(genomeGraphs[i]);
                units[i] = unit;
            }

            return units;
        }

        private BrainGraph GetBrainGraphFromGenomeGraph(GenomeGraph graph)
        {
            var brainGraph = new BrainGraph();

            foreach(var genomeNode in graph.GenomeNodes)
            {
                var brain = GenomeToBrainTranslate(genomeNode.Genome, genomeNode.Caracteristics);
                brainGraph.BrainNodes.Add(genomeNode.Caracteristics.BrainName, brain);

                if (genomeNode.Caracteristics.IsDecisionBrain)
                    brainGraph.DecisionBrain = brain;
            }

            foreach(var genomeEdge in graph.GenomeEdges)
            {
                brainGraph.BrainEdges.Add(genomeEdge.Key, new List<Brain>());
                foreach (var linkedGenome in genomeEdge.Value)
                    brainGraph.BrainEdges[genomeEdge.Key].Add(brainGraph.BrainNodes[linkedGenome]);
            }
            return brainGraph;
        }
    }
}
