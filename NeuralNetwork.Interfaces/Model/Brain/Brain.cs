using System.Text;
using System.Collections.Generic;
using System;

namespace NeuralNetwork.Interfaces.Model
{
    public class Brain
    {
        public int Id { get; set; }

        public Guid UniqueIdentifier { get; set; }

        public BrainNeurons Neurons { get; set; }

        public List<Edge> Edges { get; set; }

        public Genome Genome { get; set; }

        public Guid ParentC { get; set; }

        public Guid ParentB { get; set; }

        public float Score { get; set; }

        public int UseForChildCounter { get; set; }

        public int MaxChildNumber { get; set; } = 3;

        private string _edgesString { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(_edgesString))
            {
                var str = new StringBuilder();
                foreach (var edge in Edges)
                    str.Append($";{edge.Origin.Id}{edge.Target.Id}{edge.Weight}");
                str = str.Remove(0, 1);
                _edgesString = str.ToString();
            }
            return _edgesString;
        }
    }
}