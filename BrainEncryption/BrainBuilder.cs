using BrainEncryption.Abstraction;
using BrainEncryption.Abstraction.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BrainEncryption
{
    public class BrainBuilder : IBrainBuilder
    {
        public Brain BuildBrain(NetworkCaracteristics caracteristics)
        {
            var result = new Brain(caracteristics.OutputLayerId);

            // Build Inputs
            result.Neurons.Inputs = BuildInputs(caracteristics.InputLayer);

            // Build Neutral
            result.Neurons.Neutrals = BuildNeutrals(caracteristics.NeutralLayers);

            // Build Outputs
            result.Neurons.Outputs = BuildOutputs(caracteristics.Outputlayer);

            // Build sink neuron
            result.Neurons.SinkNeuron = BuildSinkNeuron(caracteristics.Outputlayer);

            return result;
        }

        private List<NeuronInput> BuildInputs(LayerCaracteristics inputLayer)
        {
            var result = new List<NeuronInput>();
            for(int i = 0; i < inputLayer.NeuronNumber; i++)
                result.Add(new NeuronInput(i, 0, inputLayer.ActivationFunction));

            return result;
        }

        private List<NeuronNeutral> BuildNeutrals(List<LayerCaracteristics> neutralLayersCaracteristics)
        { 
            var result = new List<NeuronNeutral>();

            for(int i = 0; i < neutralLayersCaracteristics.Count; i ++)
            {
                var currentLayer = neutralLayersCaracteristics[i];
                var curvemodifier = GetCurveModifier(currentLayer.ActivationFunction, currentLayer.ActivationFunction90PercentTreshold);
                for(int j = 0; j < currentLayer.NeuronNumber; j++)
                    result.Add(new NeuronNeutral(j, currentLayer.LayerId, currentLayer.ActivationFunction, curvemodifier));
            }

            return result;
        }

        private List<NeuronOutput> BuildOutputs(LayerCaracteristics outputLayerCaracteristics)
        {
            var result = new List<NeuronOutput>();
            var curveModifier = GetCurveModifier(outputLayerCaracteristics.ActivationFunction, outputLayerCaracteristics.ActivationFunction90PercentTreshold);

            for (int i = 0; i < outputLayerCaracteristics.NeuronNumber; i++)
                result.Add(new NeuronOutput(i, outputLayerCaracteristics.LayerId, outputLayerCaracteristics.ActivationFunction, curveModifier));

            return result;
        }

        private NeuronOutput BuildSinkNeuron(LayerCaracteristics outputLayerCaracteristics)
        {
            return new NeuronOutput(-1, outputLayerCaracteristics.LayerId, ActivationFunctionEnum.ConstantOne, 0);
        }

        private float GetCurveModifier(ActivationFunctionEnum function, float treshold)
        {
            switch(function)
            {
                case ActivationFunctionEnum.Tanh:
                    return (float)((0.5f / treshold) * (Math.Log(1.9) - Math.Log(0.1)));
                case ActivationFunctionEnum.Sigmoid:
                    return (float)Math.Log(9) / treshold;
                default:
                    return 1;
            }
        }

        //(float)((0.5f / activationFunctionTreshold) * (Math.Log(1.9) - Math.Log(0.1)));
        // (float)Math.Log(9) / sigmoid90percent;
    }
}
