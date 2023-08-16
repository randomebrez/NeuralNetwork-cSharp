using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork.Abstraction.Model
{
    public class GenericGraph<T> where T : GraphableObject
    {
        public List<T> Nodes { get; internal set; }
        public Dictionary<string, List<T>> Edges { get; internal set; }

        internal Dictionary<string, T> NodesToDictionary => Nodes.ToDictionary(t => t.Id, u => u);

        public void InitGraph(List<T> nodes, Dictionary<string, List<T>> edges)
        {
            Nodes = nodes;
            Edges = edges;
        }
    }
}
