﻿using Newtonsoft.Json;
using System;

namespace BrainEncryption
{
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

        public static float Truncate(this float value, int numberOfDigits)
        {
            var multiplicator = numberOfDigits * 10;
            var result = Math.Truncate(multiplicator * value);
            return (float)result / multiplicator;
        }
    }
}