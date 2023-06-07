using NeuralNetwork.Interfaces;
using NeuralNetwork.Interfaces.Model;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork.Implementations
{
    public class BrainCompute : IBrain
    {
        public void ComputeBrain(Brain brain, List<float> inputs)
        {
            InitialyzeInputNeuronsValue(brain, inputs);

            for (int i = 1; i <= brain.OutputLayerId + 1; i++)
                ComputeLayer(brain, i);
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
                var targetNeuron = brain.Neurons.GetNeuronByName(vertexBatch.First().Target.UniqueId);
                var computedValue = 0f;
                foreach (var vertex in vertexBatch)
                {
                    var origin = brain.Neurons.GetNeuronByName(vertex.Origin.UniqueId);
                    computedValue += origin.Value * vertex.Weight;
                }
                targetNeuron.Value = computedValue / vertexBatch.Count();
                targetNeuron.ActivationFunction();
            }
        }

        public List<float> ComputeBrainGraph(BrainGraph graph, Dictionary<string, List<float>> inputs)
        {
            return ComputeRec(graph.DecisionBrain, graph, inputs);
        }

        private List<float> ComputeRec(Brain currentBrain, BrainGraph graph, Dictionary<string, List<float>> inputs)
        {
            var inputToUse = new List<float>();
            if (graph.BrainEdges.TryGetValue(currentBrain.Name, out var originBrains))
            {
                foreach (var brain in originBrains)
                    inputToUse.AddRange(ComputeRec(brain, graph, inputs));
            }
            if (inputs.TryGetValue(currentBrain.Name, out var brainInputs))
                inputToUse.AddRange(brainInputs);


            ComputeBrain(currentBrain, inputToUse);
            return currentBrain.Neurons.Outputs.Select(t => t.Value).ToList();
        }
    }
}
