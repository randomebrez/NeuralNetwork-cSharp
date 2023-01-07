using NeuralNetwork.Interfaces;
using NeuralNetwork.Interfaces.Model;

namespace NeuralNetwork.Managers
{
    public class BrainManager : IBrain
    {
        private readonly Brain _brain;

        public BrainManager(Brain brain)
        {
            _brain = brain;
        }

        public (int ouputId, float neuronIntensity) ComputeOutput(List<float> inputs)
        {
            InitialyzeInputNeuronsValue(inputs);
            ComputeLayer(1);
            ComputeLayer(2);

            var bestOutput = GetBestOutput();
            return (bestOutput.Id, bestOutput.Value);
        }

        private void InitialyzeInputNeuronsValue(List<float> inputs)
        {
            for (int i = 0; i < _brain.Neurons.Inputs.Count; i++)
            {
                _brain.Neurons.Inputs[i].Value = inputs[i];
                _brain.Neurons.Inputs[i].ActivationFunction();
            }
        }

        private void ComputeLayer(int layerId)
        {
            var vertices = _brain.Vertices.Where(t => t.Target.Layer == layerId).ToList();
            var groupedByTarget = vertices.GroupBy(t => t.Target.UniqueId);

            foreach (var vertexBatch in groupedByTarget)
            {
                var targetNeuron = vertexBatch.First().Target;
                var computedValue = 0f;
                foreach (var vertex in vertexBatch)
                    computedValue += vertex.Origin.Value * vertex.Weight;
                targetNeuron.Value = computedValue;
                targetNeuron.ActivationFunction();
            }
        }

        private Neuron GetBestOutput()
        {
            var bestOutput = _brain.Neurons.Outputs.OrderBy(t => t.Value).Last();

            return bestOutput.Value == 0f ?
                _brain.Neurons.SinkNeuron :
                bestOutput;
        }
    }
}
