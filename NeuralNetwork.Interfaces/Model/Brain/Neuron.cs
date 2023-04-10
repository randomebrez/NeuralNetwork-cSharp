using System;

namespace NeuralNetwork.Interfaces.Model
{
    public abstract class Neuron
    {
        public int Id { get; set; }

        public string UniqueId { get; set; }

        public float Value { get; set; }

        public virtual bool CanBeOrigin { get; set; }

        public virtual bool CanBeTarget { get; set; }

        public int Layer { get; set; }

        public float Treshold { get; set; }

        public abstract void ActivationFunction();
    }

    public class NeuronInput : Neuron
    {
        public override void ActivationFunction()
        {
            // Don't do anything for input neurons
        }
    }

    public class NeuronNeutral : Neuron
    {
        public float CurveModifier { get; set; }

        public override void ActivationFunction()
        {
            // tanh for neutral neurons
            var expo = Math.Exp(- 2 * Value * CurveModifier);
            var result = (float)((1 - expo) / (1 + expo));
            Value = result;
        }
    }

    public class NeuronOutput : Neuron
    {
        public float CurveModifier { get; set; }

        public override void ActivationFunction()
        {
            //sigmoid for output neurons
            var expo = Math.Exp(- Value * CurveModifier);
            var result = (float)(1 / (1 + expo));
            Value = result < Treshold ? 0 : result;
        }
    }
}
