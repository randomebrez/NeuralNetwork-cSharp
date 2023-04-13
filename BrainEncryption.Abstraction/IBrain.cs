using BrainEncryption.Abstraction.Model;

namespace BrainEncryption.Abstraction
{
    public interface IBrainBuilder
    {
        Brain BuildBrain(NetworkCaracteristics caracteristics);
    }
}
