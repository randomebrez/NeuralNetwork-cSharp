using Newtonsoft.Json;

namespace NeuralNetwork.Helpers;

public static class StaticHelper
{
    private static Random _randomGenerator = new Random();

    public static int GetRandomValue(int minValue = 0, int maxValue = 1)
    {
        return _randomGenerator.Next(minValue, maxValue + 1);
    }

    public static float GetUniformProbability(int maxValue = 1000)
    {
        return _randomGenerator.Next(0, maxValue + 1) / (float)maxValue;
    }

    public static bool GetBooleanValue()
    {
        return _randomGenerator.Next(0, 2) == 1;
    }
    
    public static T DeepCopy<T>(this T self)
    {
        var serialized = JsonConvert.SerializeObject(self);
        return JsonConvert.DeserializeObject<T>(serialized);
    }
}