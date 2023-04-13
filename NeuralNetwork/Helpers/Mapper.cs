using publicDtos = NeuralNetwork.Interfaces.Model;
using internalDtos = BrainEncryption.Abstraction.Model;
using System.Linq;
using System.Collections.Generic;

namespace NeuralNetwork.Helpers
{
    public static class Mapper
    {
        public static internalDtos.GenomeCaracteristics ToGenomeCaracteristic(this publicDtos.GenomeCaracteristics genomeCaracteristics, HashSet<string> geneCodes)
        {
            return new internalDtos.GenomeCaracteristics(genomeCaracteristics.GeneNumber, genomeCaracteristics.WeighBytesNumber, geneCodes);
        }

        public static internalDtos.NetworkCaracteristics ToNetworkCaracteristic(this publicDtos.BrainCaracteristics brainCarac)
        {
            return new internalDtos.NetworkCaracteristics
            {
                InputLayer = brainCarac.InputLayer.ToInternal(),
                NeutralLayers = brainCarac.NeutralLayers.Select(t => t.ToInternal()).ToList(),
                Outputlayer = brainCarac.OutputLayer.ToInternal()
            };
        }


        public static internalDtos.LayerCaracteristics ToInternal(this publicDtos.LayerCaracteristics layerCaracteristic)
        {
            return new internalDtos.LayerCaracteristics(layerCaracteristic.Type.ToInternal())
            {
                NeuronNumber = layerCaracteristic.NeuronNumber,
                LayerId = layerCaracteristic.LayerId,
                ActivationFunction = layerCaracteristic.ActivationFunction.ToInternal(),
                ActivationFunction90PercentTreshold = layerCaracteristic.ActivationFunction90PercentTreshold

            };
        }

        public static internalDtos.LayerTypeEnum ToInternal(this publicDtos.LayerTypeEnum layerType)
        {
            switch(layerType)
            {
                case publicDtos.LayerTypeEnum.Inputs:
                    return internalDtos.LayerTypeEnum.Inputs;
                case publicDtos.LayerTypeEnum.Neutral:
                    return internalDtos.LayerTypeEnum.Neutral;
                case publicDtos.LayerTypeEnum.Output:
                    return internalDtos.LayerTypeEnum.Output;
                default:
                    throw new KeyNotFoundException();
            }
        }

        public static internalDtos.ActivationFunctionEnum ToInternal(this publicDtos.ActivationFunctionEnum activationFunctionType)
        {
            switch (activationFunctionType)
            {
                case publicDtos.ActivationFunctionEnum.Identity:
                    return internalDtos.ActivationFunctionEnum.Identity;
                case publicDtos.ActivationFunctionEnum.Tanh:
                    return internalDtos.ActivationFunctionEnum.Tanh;
                case publicDtos.ActivationFunctionEnum.Sigmoid:
                    return internalDtos.ActivationFunctionEnum.Sigmoid;
                case publicDtos.ActivationFunctionEnum.ConstantOne:
                    return internalDtos.ActivationFunctionEnum.ConstantOne;
                case publicDtos.ActivationFunctionEnum.ConstantZero:
                    return internalDtos.ActivationFunctionEnum.ConstantZero;
                default:
                    throw new KeyNotFoundException();
            }
        }

        public static publicDtos.ActivationFunctionEnum ToPublic(this internalDtos.ActivationFunctionEnum activationFunctionType)
        {
            switch (activationFunctionType)
            {
                case internalDtos.ActivationFunctionEnum.Identity:
                    return publicDtos.ActivationFunctionEnum.Identity;
                case internalDtos.ActivationFunctionEnum.Tanh:
                    return publicDtos.ActivationFunctionEnum.Tanh;
                case internalDtos.ActivationFunctionEnum.Sigmoid:
                    return publicDtos.ActivationFunctionEnum.Sigmoid;
                case internalDtos.ActivationFunctionEnum.ConstantOne:
                    return publicDtos.ActivationFunctionEnum.ConstantOne;
                case internalDtos.ActivationFunctionEnum.ConstantZero:
                    return publicDtos.ActivationFunctionEnum.ConstantZero;
                default:
                    throw new KeyNotFoundException();
            }
        }


        public static publicDtos.Genome ToPublic(this internalDtos.Genome genome)
        {
            return new publicDtos.Genome()
            {
                GeneNumber = genome.GeneNumber,
                Genes = genome.Genes.Select(t => t.ToPublic()).ToArray(),
                GenomeToString = genome.ToString()
            };
        }

        public static publicDtos.Gene ToPublic(this internalDtos.Gene gene)
        {
            return new publicDtos.Gene()
            {
                EdgeIdentifier = gene.EdgeIdentifier,
                WeighSign = gene.WeighSign,
                WeighBits = gene.WeighBits,
                Bias = gene.Bias,
                GeneToString = gene.ToString(),
                IsActive = gene.IsActive
            };
        }


        public static publicDtos.Brain ToPublic(this internalDtos.Brain brain)
        {
            return new publicDtos.Brain
            {
                UniqueIdentifier = brain.UniqueIdentifier,
                Edges = brain.Edges.Select(t => t.ToPublic()).ToList(),
                Neurons = brain.Neurons.ToPublic(),
                OutputLayerId = brain.OutputLayerId
            };
        }

        public static publicDtos.Neuron ToPublic(this internalDtos.Neuron neuron)
        {
            if (neuron is internalDtos.NeuronInput)
                return ((internalDtos.NeuronInput)neuron).ToPublic();
            else if (neuron is internalDtos.NeuronNeutral)
                return ((internalDtos.NeuronNeutral)neuron).ToPublic();
            else
                return ((internalDtos.NeuronOutput)neuron).ToPublic();
        }

        public static publicDtos.NeuronInput ToPublic(this internalDtos.NeuronInput neuron)
        {
            return new publicDtos.NeuronInput
            {
                Id = neuron.Id,
                UniqueId = neuron.UniqueId,
                Layer = neuron.LayerId,
                CanBeOrigin = neuron.CanBeOrigin,
                CanBeTarget = neuron.CanBeTarget,
                ActivationFunctionType = neuron.ActivationFunction.ToPublic()
            };
        }

        public static publicDtos.NeuronNeutral ToPublic(this internalDtos.NeuronNeutral neuron)
        {
            return new publicDtos.NeuronNeutral
            {
                Id = neuron.Id,
                UniqueId = neuron.UniqueId,
                Layer = neuron.LayerId,
                CanBeOrigin = neuron.CanBeOrigin,
                CanBeTarget = neuron.CanBeTarget,
                CurveModifier = neuron.CurveModifier,
                ActivationFunctionType = neuron.ActivationFunction.ToPublic()
            };
        }

        public static publicDtos.NeuronOutput ToPublic(this internalDtos.NeuronOutput neuron)
        {
            return new publicDtos.NeuronOutput
            {
                Id = neuron.Id,
                UniqueId = neuron.UniqueId,
                Layer = neuron.LayerId,
                CanBeOrigin = neuron.CanBeOrigin,
                CanBeTarget = neuron.CanBeTarget,
                CurveModifier = neuron.CurveModifier,
                ActivationFunctionType = neuron.ActivationFunction.ToPublic()
            };
        }

        public static publicDtos.BrainNeurons ToPublic(this internalDtos.BrainNeurons brainNeurons)
        {
            return new publicDtos.BrainNeurons
            {
                Inputs = brainNeurons.Inputs.Select(t => t.ToPublic()).ToList(),
                Neutrals = brainNeurons.Neutrals.Select(t => t.ToPublic()).ToList(),
                Outputs = brainNeurons.Outputs.Select(t => t.ToPublic()).ToList(),
                SinkNeuron = brainNeurons.SinkNeuron.ToPublic()
            };
        }

        public static publicDtos.Edge ToPublic(this internalDtos.Edge edge)
        {
            return new publicDtos.Edge
            {
                Identifier = edge.Identifier,
                Origin = edge.Origin.ToPublic(),
                Target = edge.Target.ToPublic(),
                Weight = edge.Weight
            };
        }
    }
}
