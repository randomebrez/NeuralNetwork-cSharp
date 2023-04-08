namespace BrainEncryption.Abstraction.Model
{
    public abstract class Neuron
    {
        public Neuron(int id, int layerId)
        {
            Id = id;
            Layer = layerId;
        }

        public int Id { get; private set; }

        public virtual string UniqueId { get; }

        public float Value { get; set; }
        
        public virtual bool CanBeOrigin { get => false; }

        public virtual bool CanBeTarget { get => false; }

        public int Layer { get; set; }

        public float Treshold { get; set; }

        public abstract void ActivationFunction();
    }

    public class NeuronInput : Neuron
    {
        public NeuronInput(int id, int layerId) : base(id, layerId)
        {
            
        }

        public override string UniqueId => $"I:{Id}";

        public override bool CanBeOrigin { get => true; }

        public override void ActivationFunction()
        {
            // Don't do anything for input neurons
        }
    }

    public class NeuronNeutral : Neuron
    {
        public NeuronNeutral(int id, int layerId, float tanh90percent) : base(id, layerId)
        {
            _curveModifier = (float)((0.5f / tanh90percent) * (Math.Log(1.9) - Math.Log(0.1)));
        }

        public override string UniqueId => $"N{Layer}:{Id}";

        public override bool CanBeOrigin { get => true; }

        public override bool CanBeTarget { get => true; }

        private float _curveModifier;

        public override void ActivationFunction()
        {
            // tanh for neutral neurons
            var expo = Math.Exp(Value * _curveModifier);
            var result = (float)((1 - expo) / (1 + expo));
            Value = result;
        }
    }

    public class NeuronOutput : Neuron
    {
        public NeuronOutput(int id, int layerId, float sigmoid90percent) : base(id, layerId)
        {
            _curveModifier = (float)Math.Log(9) / sigmoid90percent;
        }

        public override string UniqueId => $"O:{Id}";

        public override bool CanBeTarget { get => true; }

        public float _curveModifier { get; set; }

        public override void ActivationFunction()
        {
            //sigmoid for output neurons
            var expo = Math.Exp(- Value * _curveModifier);
            var result = (float)(1 / (1 + expo));
            Value = result < Treshold ? 0 : result;
        }
    }
}
