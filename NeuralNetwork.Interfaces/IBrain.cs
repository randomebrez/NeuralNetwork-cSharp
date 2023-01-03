namespace NeuralNetwork.Interfaces
{
    public interface IBrain
    {
        public (int ouputId, float neuronIntensity) ComputeOutput(List<float> inputs);
    }
}