using System.Collections.Generic;

namespace NeuralNetwork.Abstraction.Model
{
    public class GenericEdge<T> where T : GraphableObject
    {
        public T Target { get; set; }
        public Dictionary<int, T> Origins { get; set; }
        public Dictionary<int, float> Weights { get; set; }
    }
}
