using NeuralNetwork.Interfaces;
using NeuralNetwork.Interfaces.Model;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork.Managers
{
    public class UnitManager : IUnitBrains
    {
        private readonly Unit _unit;


        public UnitManager(Unit unit)
        {
            _unit = unit;
        }


        public void ComputeBrain(string brainKey, List<float> inputs)
        {
            var brain = _unit.Brains[brainKey].Brain;
            InitialyzeInputNeuronsValue(brain, inputs);

            for (int i = 1; i <= brain.OutputLayerId + 1; i++)
                ComputeLayer(brain, i);
        }

        public (int ouputId, float neuronIntensity) GetBestOutput(string brainKey)
        {
            var brain = _unit.Brains[brainKey].Brain;
            var bestOutputNeuron = GetBestOutput(brain);
            return (bestOutputNeuron.Id, bestOutputNeuron.Value);
        }

        public List<float> GetOutputs(string brainKey)
        {
            var brain = _unit.Brains[brainKey].Brain;
            return brain.Neurons.Outputs.Select(t => t.Value).ToList();
        }



        private void InitialyzeInputNeuronsValue(Brain brain, List<float> inputs)
        {
            for (int i = 0; i < brain.Neurons.Inputs.Count; i++)
            {
                brain.Neurons.Inputs[i].Value = inputs[i];
                brain.Neurons.Inputs[i].ActivationFunction();
            }
        }

        private void ComputeLayer(Brain brain, int layerId)
        {
            var vertices = brain.Edges.Where(t => t.Target.Layer == layerId).ToList();
            var groupedByTarget = vertices.GroupBy(t => t.Target.UniqueId);

            foreach (var vertexBatch in groupedByTarget)
            {
                var targetNeuron = vertexBatch.First().Target;
                var computedValue = 0f;
                foreach (var vertex in vertexBatch)
                    computedValue += vertex.Origin.Value * vertex.Weight;
                targetNeuron.Value = computedValue / vertexBatch.Count();
                targetNeuron.ActivationFunction();
            }
        }

        private Neuron GetBestOutput(Brain brain)
        {
            var bestOutput = brain.Neurons.Outputs.OrderBy(t => t.Value).Last();

            return bestOutput.Value == 0f ?
                brain.Neurons.SinkNeuron :
                bestOutput;
        }
    }
}
