using NeuralNetwork.Interfaces.Model;
using System.Collections.Generic;

namespace NeuralNetwork.Interfaces
{
    public interface IGenomeManager
    {
        List<Genome> GenomesListGet(int number, BrainCaracteristics caracteristics);

        BrainGenomePair GenomeToBrainTranslate(Genome genome, BrainCaracteristics caracteristics);

        Genome GenomeCrossOverGet(Genome genomeA, Genome genomeB, BrainCaracteristics brainCaracteristics, int crossOverNumber, float mutationRate);

        Unit[] UnitsFromGenomeGraphList(List<GenomeGraph> genomeGraphs);
    }
}
