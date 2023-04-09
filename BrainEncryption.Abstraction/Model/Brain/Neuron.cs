using System;

namespace BrainEncryption.Abstraction.Model
{
    public abstract class Neuron
    {
        public Neuron(int id, int layerId)
        {
            Id = id;
            LayerId = layerId;
        }

        public int Id { get; private set; }

        public abstract string UniqueId { get; }
        
        public virtual bool CanBeOrigin { get => false; }

        public virtual bool CanBeTarget { get => false; }

        public int LayerId { get; set; }
    }

    public class NeuronInput : Neuron
    {
        public NeuronInput(int id, int layerId) : base(id, layerId)
        {
            
        }

        public override string UniqueId => $"I:{Id}";

        public override bool CanBeOrigin { get => true; }
    }

    public class NeuronNeutral : Neuron
    {
        public NeuronNeutral(int id, int layerId, float tanh90percent) : base(id, layerId)
        {
            CurveModifier = (float)((0.5f / tanh90percent) * (Math.Log(1.9) - Math.Log(0.1)));
        }

        public override string UniqueId => $"N{LayerId}:{Id}";

        public override bool CanBeOrigin { get => true; }

        public override bool CanBeTarget { get => true; }

        public float CurveModifier;
    }

    public class NeuronOutput : Neuron
    {
        public NeuronOutput(int id, int layerId, float sigmoid90percent) : base(id, layerId)
        {
            CurveModifier = (float)Math.Log(9) / sigmoid90percent;
        }

        public override string UniqueId => $"O:{Id}";

        public override bool CanBeTarget { get => true; }

        public float CurveModifier { get; set; }
    }
}
