using BrainEncryption.Abstraction.Model;
using System.Collections.Generic;

namespace BrainEncryption.Abstraction
{
    public interface IGenomeEncrypter
    {
        HashSet<string> GeneCodesList(NetworkCaracteristics caracteristics);

        Genome GenomeGenerate(GenomeCaracteristics caracteristics, HashSet<string> geneCodes);

        Genome StringToGenomeTranslate(string genomeString);

        Genome GenomeCrossOver(GenomeCaracteristics caracteristics, Genome genomeDtoA, Genome genomeDtoB, int crossOverNumber);

        Genome GenomeMutate(Genome baseGenome, HashSet<string> geneCodes, float geneMutationRate);

        void GenomeTranslate(Brain brain, Genome genome);
    }
}