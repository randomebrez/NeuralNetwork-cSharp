using System.Text;
using System.Collections.Generic;

namespace NeuralNetwork.Interfaces.Model
{
    public class Brain
    {
        public string Name { get; set; }

        public BrainNeurons Neurons { get; set; }

        public List<Edge> Edges { get; set; }

        public int OutputLayerId { get; set; }

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