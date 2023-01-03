using System.Text;
using NeuralNetwork.Interfaces.Model;
using NeuralNetwork.Managers;

namespace NeuralNetwork.Tests;

public class OtherTests
{
    public void Test(NetworkCaracteristics networkCaracteristics)
    {
        var populationManager = new PopulationManager(networkCaracteristics);
        var populatioNumber = 50;
        var logfilepath = @"C:\Users\nlouviaux\Desktop\Test\neuralTework.txt";
        if (File.Exists(logfilepath))
            File.Delete(logfilepath);
        
        var fileText = new StringBuilder();
        
        var firstGeneration = populationManager.GenerateFirstGeneration(populatioNumber);
        var inputs = new List<float> { 0.9f, 0.1f, 0.2f };
        var brainScores = new Dictionary<int, float>();
        var index = 0;
        var scoreMean = 0f;
        var validBrainNumber = 0;
        Console.WriteLine("Generation 0");
        foreach(var brain in firstGeneration)
        {
            if (brain == null)
                continue;
            validBrainNumber++;
            var brainManager = new BrainManager(brain);
            var (outputId, outputIntensity) = brainManager.ComputeOutput(inputs);
            var brainScore = (3 - outputId) * outputIntensity;
            scoreMean += brainScore;
            //Console.WriteLine(brainScore);
            brainScores.Add(index, brainScore);
            index++;
        }
        fileText.AppendLine(
            $"{scoreMean / validBrainNumber};{brainScores.Values.Max()};{brainScores.Where(t => t.Value <= 1).Count()};{brainScores.Where(t => t.Value > 1 && t.Value <= 2).Count()};{brainScores.Where(t => t.Value > 2).Count()}");
        Console.WriteLine($"Mean : {scoreMean/validBrainNumber}");
        
        var selectedBrain = brainScores.OrderByDescending(t => t.Value);
        var selectedBrains = selectedBrain.Take(10).Select(t => firstGeneration[t.Key]);
        var generationNumber = 10000;
        var plusOn = false;
        for(int i = 1; i < generationNumber; i++)
        {
            index = 0;
            validBrainNumber = 0;
            scoreMean = 0f;
            Console.WriteLine($"Generation {i}");
            var nextGeneration = populationManager.GenerateNewGeneration(populatioNumber, selectedBrains.ToList());
            brainScores.Clear();
        
            if (i % 1000 == 0)
                plusOn = !plusOn;
            
            foreach (var brain in nextGeneration)
            {
                if (brain == null)
                    continue;
                validBrainNumber++;
                var brainManager = new BrainManager(brain);
                var (outputId, outputIntensity) = brainManager.ComputeOutput(inputs);
                var brainScore = plusOn ? outputId * outputIntensity : (3 - outputId) * outputIntensity;
                //var brainScore = (3 - outputId) * outputIntensity;
                //Console.WriteLine(brainScore);
                brainScores.Add(index, brainScore);
                scoreMean += brainScore;
                index++;
            }
        
            fileText.AppendLine(
                $"{scoreMean / validBrainNumber};{brainScores.Values.Max()};{brainScores.Where(t => t.Value <= 1).Count()};{brainScores.Where(t => t.Value > 1 && t.Value <= 2).Count()};{brainScores.Where(t => t.Value > 2).Count()}");
            Console.WriteLine($"ValidBrains : {validBrainNumber} - Mean : {scoreMean/validBrainNumber}");
            selectedBrains = brainScores.OrderByDescending(t => t.Value).Take(10).Select(t => nextGeneration[t.Key]);
        }
        
        using (var fileStream = File.OpenWrite(logfilepath))
        {
            var bytes = new UTF8Encoding().GetBytes(fileText.Replace(',', '.').ToString(), 0, fileText.Length);
            fileStream.Write(bytes);
            fileStream.Flush();
        }
    }
}