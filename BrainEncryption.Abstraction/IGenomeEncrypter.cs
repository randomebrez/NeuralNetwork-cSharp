using BrainEncryption.Abstraction.Model;
using System.Collections.Generic;

namespace BrainEncryption.Abstraction
{
    public interface IGenomeEncrypter
    {
        HashSet<string> GetGeneCodes(NetworkCaracteristics caracteristics);

        Genome GenerateGenome(GenomeCaracteristics caracteristics, HashSet<string> geneCodes);

        Genome GetGenomeFromString(string genomeString);

        Genome CrossOver(GenomeCaracteristics caracteristics, Genome genomeDtoA, Genome genomeDtoB, int crossOverNumber);

        Genome MutateGenome(Genome baseGenome, HashSet<string> geneCodes, float geneMutationRate);

        void TranslateGenome(Brain brain, Genome genome);
    }
}