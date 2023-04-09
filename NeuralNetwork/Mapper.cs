using publicDtos = NeuralNetwork.Interfaces.Model;
using internalDtos = BrainEncryption.Abstraction.Model;
using System.Linq;
using System.Reflection.Emit;

namespace NeuralNetwork
{
    public static class Mapper
    {
        public static internalDtos.GenomeCaracteristics ToGenomeCaracteristic(this publicDtos.BrainCaracteristics brainCarac)
        {
            return new internalDtos.GenomeCaracteristics(brainCarac.GeneNumber, brainCarac.WeighBytesNumber, brainCarac.GeneCodes);
        }

        public static internalDtos.NetworkCaracteristics ToNetworkCaracteristic(this publicDtos.BrainCaracteristics brainCarac)
        {
            return new internalDtos.NetworkCaracteristics
            {
                InputNumber= brainCarac.InputNumber,
                NeutralNumbers= brainCarac.NeutralNumbers,
                OutputNumber= brainCarac.OutputNumber,
                Tanh90Percent = brainCarac.Tanh90Percent,
                Sigmoid90Percent = brainCarac.Sigmoid90Percent
            };
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
                Edges = brain.Edges.Select( t => t.ToPublic()).ToList(),
                Neurons = brain.Neurons.ToPublic()
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
                CanBeTarget = neuron.CanBeTarget
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
                CurveModifier = neuron.CurveModifier
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
