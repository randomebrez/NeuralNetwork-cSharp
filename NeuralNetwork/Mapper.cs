using publicDtos = NeuralNetwork.Abstraction.Model;
using internalDtos = BrainEncryption.Abstraction.Model;
using System.Linq;
using System.Collections.Generic;

namespace NeuralNetwork
{
    public static class Mapper
    {
        // Genome & Network caracteristics
        public static internalDtos.GenomeCaracteristics ToGenomeCaracteristic(this publicDtos.GenomeCaracteristics genomeCaracteristics)
        {
            return new internalDtos.GenomeCaracteristics(genomeCaracteristics.GeneNumber, genomeCaracteristics.WeighBytesNumber);
        }
        public static internalDtos.NetworkCaracteristics ToNetworkCaracteristic(this publicDtos.BrainCaracteristics brainCarac)
        {
            return new internalDtos.NetworkCaracteristics
            {
                BrainName = brainCarac.BrainName,
                InputLayer = brainCarac.InputLayer.ToInternal(),
                NeutralLayers = brainCarac.NeutralLayers.Select(t => t.ToInternal()).ToList(),
                Outputlayer = brainCarac.OutputLayer.ToInternal()
            };
        }

        // Genome
        public static publicDtos.Genome ToPublic(this internalDtos.Genome genome)
        {
            return new publicDtos.Genome()
            {
                GeneNumber = genome.GeneNumber,
                Genes = genome.Genes.Select(t => t.ToPublic()).ToArray(),
                GenomeToString = genome.ToString()
            };
        }
        public static internalDtos.Genome ToInternal(this publicDtos.Genome genome)
        {
            return new internalDtos.Genome(genome.GeneNumber)
            {
                Genes = genome.Genes.Select(t => t.ToInternal()).ToArray(),
            };
        }

        // Genes
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
        public static internalDtos.Gene ToInternal(this publicDtos.Gene gene)
        {
            return new internalDtos.Gene(gene.EdgeIdentifier, gene.WeighBits.Length)
            {
                WeighSign = gene.WeighSign,
                WeighBits = gene.WeighBits,
                Bias = gene.Bias,
                IsActive = gene.IsActive
            };
        }


        // Brain
        public static publicDtos.Brain ToPublic(this internalDtos.Brain brain)
        {
            return new publicDtos.Brain
            {
                Name = brain.Name,
                Edges = brain.Edges.Select(t => t.ToPublic()).ToList(),
                Neurons = brain.Neurons.ToPublic(),
                OutputLayerId = brain.OutputLayerId
            };
        }
        public static internalDtos.LayerCaracteristics ToInternal(this publicDtos.LayerCaracteristics layerCaracteristic)
        {
            return new internalDtos.LayerCaracteristics(layerCaracteristic.Type.ToInternal())
            {
                NeuronNumber = layerCaracteristic.NeuronNumber,
                //Todo : a delete
                LayerId = layerCaracteristic.LayerId,
                ActivationFunction = layerCaracteristic.ActivationFunction.ToInternal(),
                ActivationFunction90PercentTreshold = layerCaracteristic.ActivationFunction90PercentTreshold

            };
        }

        // Neurons
        public static publicDtos.BrainNeurons ToPublic(this internalDtos.BrainNeurons brainNeurons)
        {
            // Input layer
            var inputLayer = new publicDtos.NeuronLayer 
            { 
                LayerType = publicDtos.LayerTypeEnum.Input,
                Id = 0,
                Neurons = brainNeurons.Inputs.Select(t => t.ToPublic()).ToList()
            };

            // Neutral layers
            var neutralLayerId = 1;
            var neutralLayers = new List<publicDtos.NeuronLayer>();
            var neutralNeurons = brainNeurons.Neutrals.Where(t => t.LayerId == neutralLayerId);
            while (neutralNeurons.Count() > 0)
            {
                var neutralLayer = new publicDtos.NeuronLayer
                {
                    LayerType = publicDtos.LayerTypeEnum.Neutral,
                    Id = neutralLayerId,
                    Neurons = neutralNeurons.Select(t => t.ToPublic()).ToList()
                };
                neutralLayers.Add(neutralLayer);
                neutralLayerId++;
                neutralNeurons = brainNeurons.Neutrals.Where(t => t.LayerId == neutralLayerId);
            }

            // Output layer
            var outputLayer = new publicDtos.NeuronLayer
            {
                LayerType = publicDtos.LayerTypeEnum.Output,
                Id = neutralLayerId,
                Neurons = brainNeurons.Outputs.Select(t => t.ToPublic()).ToList()
            };

            return new publicDtos.BrainNeurons
            {
                InputLayer = inputLayer,
                NeutralLayers = neutralLayers,
                OutputLayer = outputLayer,
                SinkNeuron = brainNeurons.SinkNeuron.ToPublic()
            };
        }
        public static publicDtos.Neuron ToPublic(this internalDtos.Neuron neuron)
        {
            return new publicDtos.Neuron
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
        
        // Network edges
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

        // Enum
        public static internalDtos.LayerTypeEnum ToInternal(this publicDtos.LayerTypeEnum layerType)
        {
            switch(layerType)
            {
                case publicDtos.LayerTypeEnum.Input:
                    return internalDtos.LayerTypeEnum.Input;
                case publicDtos.LayerTypeEnum.Neutral:
                    return internalDtos.LayerTypeEnum.Neutral;
                case publicDtos.LayerTypeEnum.Output:
                    return internalDtos.LayerTypeEnum.Output;
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
    }
}
