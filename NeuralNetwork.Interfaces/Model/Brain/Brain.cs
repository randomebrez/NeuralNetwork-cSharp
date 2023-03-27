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

        public List<Vertex> Vertices { get; set; }

        public Genome Genome { get; set; }

        public Guid FirstParent { get; set; }

        public Guid SecondParent { get; set; }

        public int UseForChildCounter { get; set; }

        public int MaxChildNumber { get; set; } = 3;

        private string _vertexString { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(_vertexString))
            {
                var str = new StringBuilder();
                foreach (var vertex in Vertices)
                    str.Append($";{vertex.Origin.Id}{vertex.Target.Id}{vertex.Weight}");
                str = str.Remove(0, 1);
                _vertexString = str.ToString();
            }
            return _vertexString;
        }
    }
}