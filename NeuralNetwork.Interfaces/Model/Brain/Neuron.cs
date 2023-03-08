using System;

namespace NeuralNetwork.Interfaces.Model
{
    public abstract class Neuron
    {
        public Neuron(int id, int layerId, float treshold = 0.1f)
        {
            Id = id;
            Layer = layerId;
            Treshold = treshold;
        }

        public int Id { get; private set; }

        public virtual string UniqueId { get; }

        public float Value { get; set; }
        
        public virtual bool CanBeOrigin { get => false; }

        public virtual bool CanBeTarget { get => false; }

        public int Layer { get; set; }

        public float Treshold { get; set; }

        public virtual void ActivationFunction()
        {
            if (Value < Treshold)
                Value = 0;
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

        public override string UniqueId => $"N{Layer}:{Id}";

        public override bool CanBeOrigin { get => true; }

        public override bool CanBeTarget { get => true; }

        //tanh pour les neurones internes
        public override void ActivationFunction()
        {
            var expo = Math.Exp(-2 * Value);
            var result = (float)((1 - expo) / (1 + expo));
            Value = result < Treshold ? 0 : result;
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
        public override void ActivationFunction()
        {
            var expo = Math.Exp(-Value);
            var result = (float)(1 / (1 + expo));
            Value = result < Treshold ? 0 : result;
        }
    }
}
