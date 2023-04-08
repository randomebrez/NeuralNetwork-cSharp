using BrainEncryption.Abstraction.Model;

namespace BrainEncryption.Abstraction
{
    public interface IGenome
    {
        HashSet<string> GetGeneCodes(NetworkCaracteristics caracteristics);

        Genome GenerateGenome(GenomeCaracteristics caracteristics);

        string GetStringFromGenome(Genome genome);

        Genome GetGenomeFromString(string genomeString);

        Genome CrossOver(GenomeCaracteristics caracteristics, Genome genomeDtoA, Genome genomeDtoB, int crossOverNumber);

        Genome MutateGenome(Genome baseGenome, GenomeCaracteristics caracteristics, float geneMutationRate);

        Brain TranslateGenome(NetworkCaracteristics networkCarac, Genome genome);
    }
}