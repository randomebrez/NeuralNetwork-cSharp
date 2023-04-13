using BrainEncryption.Abstraction.Model;
using System.Collections.Generic;

namespace BrainEncryption.Abstraction
{
    public interface IGenome
    {
        HashSet<string> GetGeneCodes(NetworkCaracteristics caracteristics);

        Genome GenerateGenome(GenomeCaracteristics caracteristics);

        Genome GetGenomeFromString(string genomeString);

        Genome CrossOver(GenomeCaracteristics caracteristics, Genome genomeDtoA, Genome genomeDtoB, int crossOverNumber);

        Genome MutateGenome(Genome baseGenome, GenomeCaracteristics caracteristics, float geneMutationRate);

        void TranslateGenome(Brain brain, Genome genome);
    }
}