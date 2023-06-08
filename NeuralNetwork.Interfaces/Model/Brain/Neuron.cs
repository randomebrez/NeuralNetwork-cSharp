using System;

namespace NeuralNetwork.Interfaces.Model
{
    public class Neuron
    {
        public int Id { get; set; }

        public string UniqueId { get; set; }

        public float Value { get; set; }

        public virtual bool CanBeOrigin { get; set; }

        public virtual bool CanBeTarget { get; set; }

        public int Layer { get; set; }

        public float Treshold { get; set; }

        public float CurveModifier { get; set; }

        public ActivationFunctionEnum ActivationFunctionType { get; set; }

        public virtual void ActivationFunction()
        {
            switch (ActivationFunctionType)
            {
                case ActivationFunctionEnum.ConstantOne:
                    Value = 1;
                    return;
                case ActivationFunctionEnum.ConstantZero:
                    Value = 0;
                    return;
                case ActivationFunctionEnum.Tanh:
                    var expoTanh = Math.Exp(-2 * Value * CurveModifier);
                    var resultTanh = (float)((1 - expoTanh) / (1 + expoTanh));
                    Value = resultTanh;
                    break;
                case ActivationFunctionEnum.Sigmoid:
                    var expoSigmoid = Math.Exp(-Value * CurveModifier);
                    var resultSigmoid = (float)(1 / (1 + expoSigmoid));
                    Value = resultSigmoid < Treshold ? 0 : resultSigmoid;
                    break;
                case ActivationFunctionEnum.Identity:
                    break;
            }
        }
    }
}
