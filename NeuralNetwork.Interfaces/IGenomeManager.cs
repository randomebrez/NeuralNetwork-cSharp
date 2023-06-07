using NeuralNetwork.Interfaces.Model;
using System.Collections.Generic;

namespace NeuralNetwork.Interfaces
{
    public interface IGenomeManager
    {
        List<Genome> GetGenomes(int number, BrainCaracteristics caracteristics);

        Brain TranslateGenome(Genome genome, BrainCaracteristics caracteristics);

        Genome GetMixedGenome(Genome genomeA, Genome genomeB, BrainCaracteristics brainCaracteristics, int crossOverNumber, float mutationRate);

        Unit[] GetUnitFromGenomeGraphs(List<GenomeGraph> genomeGraphs);
    }
}
