namespace BrainEncryption.Abstraction.Model
{
    public abstract class Neuron
    {
        public Neuron(int id, int layerId, ActivationFunctionEnum activationFunction, float curveModifier)
        {
            Id = id;
            LayerId = layerId;
            ActivationFunction = activationFunction;
            CurveModifier = curveModifier;
        }

        public int Id { get; }

        public abstract string UniqueId { get; }
        
        public virtual bool CanBeOrigin { get => false; }

        public virtual bool CanBeTarget { get => false; }

        public int LayerId { get; }

        public ActivationFunctionEnum ActivationFunction { get; }

        public float CurveModifier { get; }
    }

    public class NeuronInput : Neuron
    {
        public NeuronInput(int id, int layerId, ActivationFunctionEnum activationFunction, float curveModifier) : base(id, layerId, activationFunction, curveModifier)
        {
        }

        public override string UniqueId => $"I:{Id}";

        public override bool CanBeOrigin { get => true; }
    }

    public class NeuronNeutral : Neuron
    {
        public NeuronNeutral(int id, int layerId, ActivationFunctionEnum activationFunction, float curveModifier) : base(id, layerId, activationFunction, curveModifier)
        {
        }

        public override string UniqueId => $"N{LayerId}:{Id}";

        public override bool CanBeOrigin { get => true; }

        public override bool CanBeTarget { get => true; }

    }

    public class NeuronOutput : Neuron
    {
        public NeuronOutput(int id, int layerId, ActivationFunctionEnum activationFunction, float curveModifier) : base(id, layerId, activationFunction, curveModifier)
        {
        }

        public override string UniqueId => $"O:{Id}";

        public override bool CanBeTarget { get => true; }
    }
}
