namespace NeuralNetwork.Interfaces.Model;

public static class StaticSpaceDimension
{
    public static Dictionary<int, (int min, int max)> SpaceDimensions { get; set; }

    public static int DimensionNumber => SpaceDimensions.Count;
}