using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork.Abstraction.Model
{
    public class GenericGraph<T> where T : GraphableObject
    {
        public List<T> Nodes { get; internal set; }
        public List<GenericEdge<T>> Edges { get; internal set; }

        internal Dictionary<string, T> NodesToDictionary => Nodes.ToDictionary(t => t.Id, u => u);
        internal Dictionary<string, GenericEdge<T>> EdgesToDictionary => Edges.ToDictionary(t => t.Target.Id, u => u);

        public void InitGraph(List<T> nodes, List<GenericEdge<T>> edges)
        {
            Nodes = nodes;
            Edges = edges;
        }
    }
}
