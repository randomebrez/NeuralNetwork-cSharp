using System.Collections.Generic;

namespace NeuralNetwork.Tests.Model
{
    public static class StaticSpaceDimension
    {
        public static Dictionary<int, (int min, int max)> SpaceDimensions { get; set; }

        public static int DimensionNumber => SpaceDimensions.Count;
    }
}