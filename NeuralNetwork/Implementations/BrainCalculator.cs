using NeuralNetwork.Abstraction;
using NeuralNetwork.Abstraction.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork.Implementations
{
    public class BrainCalculator : IBrainCalculator
    {
        // ToDo : transform a brain in a graph to compute recursively
        public void BrainCompute(Brain brain, List<float> inputs)
        {
            InitialyzeInputNeuronsValue(brain, inputs);
             
            for (int i = 1; i <= brain.OutputLayerId; i++)
                ComputeLayer(brain, i);
        }

        private void InitialyzeInputNeuronsValue(Brain brain, List<float> inputs)
        {
            try
            {
                for (int i = 0; i < brain.Neurons.InputLayer.NeuronNumber; i++)
                {
                    brain.Neurons.InputLayer.Neurons[i].Value = inputs[i];
                    brain.Neurons.InputLayer.Neurons[i].ActivationFunction();
                }
            }
            catch
            {
                throw new Exception($"InputMethod : {brain.Name} : BrainNeuronN {brain.Neurons.InputLayer.NeuronNumber} : InputLayerNCount {brain.Neurons.InputLayer.Neurons.Count} : InputsN : {inputs.Count}");
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


        public List<float> BrainGraphCompute(BrainGraph graph, Dictionary<string, List<float>> inputs)
        {
            try
            {
                return ComputeRec(graph.DecisionBrain, graph, inputs);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private List<float> ComputeRec(Brain currentBrain, BrainGraph brainGraph, Dictionary<string, List<float>> inputs)
        {
            var inputToUse = new List<float>();
            if (brainGraph.EdgeDic.TryGetValue(currentBrain.Name, out var edges))
            {
                foreach (var brain in edges.Origins.Values)
                    inputToUse.AddRange(ComputeRec(brain.Brain, brainGraph, inputs));
            }
            if (inputs.TryGetValue(currentBrain.Name, out var brainInputs))
                inputToUse.AddRange(brainInputs);


            BrainCompute(currentBrain, inputToUse);
            return currentBrain.Neurons.OutputLayer.Neurons.Select(t => t.Value).ToList();
        }
    }
}
