namespace NeuralNetwork.Interfaces.Model
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

        public virtual float ActivationFunction(float threshold = 0f)
        {
            if (Value < threshold)
                Value = 0;

            return Value;
        }
    }

    public class NeuronInput : Neuron
    {
        public NeuronInput(int id, int layerId) : base(id, layerId)
        {
        }

        public override string UniqueId => $"I:{Id}";

        public override bool CanBeOrigin { get => true; }

        public string SensorType { get; set; }
    }

    public class NeuronNeutral : Neuron
    {
        public NeuronNeutral(int id, int layerId) : base(id, layerId)
        {
        }

        public override string UniqueId => $"N:{Id}";

        public override bool CanBeOrigin { get => true; }

        public override bool CanBeTarget { get => true; }

        //tanh pour les neurones internes
        public override float ActivationFunction(float threshold = 0f)
        {
            var expo = Math.Exp(-2 * Value);
            return (float)((1 - expo) / (1 + expo));
        }
    }

    public class NeuronOutput : Neuron
    {
        public NeuronOutput(int id, int layerId) : base(id, layerId)
        {
        }

        public override string UniqueId => $"O:{Id}";

        public override bool CanBeTarget { get => true; }

        public string ActionType { get; set; }
        
        //sigmoid for output neurons
        public override float ActivationFunction(float threshold = 0f)
        {
            var expo = Math.Exp(-Value);
            return (float)(1 / (1 + expo));
        }
    }
}
