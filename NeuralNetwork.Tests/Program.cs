using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NeuralNetwork.Helpers;
using NeuralNetwork.Implementations;
using NeuralNetwork.Interfaces.Model;
using NeuralNetwork.Tests;

internal class program
{
    static void Main(string[] args)
    {
        var start = DateTime.UtcNow;

        // config sur fixe
        var logfilepath = @"D:\Codes\Test\neuralTework.txt";
        var sqliteConnectionString = @"D:\Codes\Test\NeuralNetworkDatabase.txt";

        //Config sur pc portable
        //var logfilepath = @"C:\Users\nico-\Documents\Codes\Tests\neuralTework.txt";
        //var sqliteConnectionString =  @"C:\Users\nico-\Documents\Codes\Tests\NeuralNetworkDatabase.txt";

        var cleanDatabase = true;
        var maxNumberOfGeneration = 500;

        var spaceDimensions = new int[] { 50, 50 };
        var maxPopulationNumber = 100;
        var unitLifeTime = 150;

        var selectionRadius = 0.3f;
        var maxNumberToSelect = 30;
        var meanChildNumber = 5;
        var crossOverNumber = 2;
        var mutationRate = 0.001f;

        var networkCaracteristics = new List<BrainCaracteristics>
        {
            new BrainCaracteristics("Main")
            {
                GeneNumber = 70,
                InputNumber = 4,
                OutputNumber = 8,
                NeutralNumbers = new List<int> { 5 },
                WeighBytesNumber = 3,
                Tanh90Percent = 3,
                Sigmoid90Percent = 3
            }
        };

        // PROGRAM

        // File is automatically recreated when instantiating the 'context'
        if (cleanDatabase && File.Exists(sqliteConnectionString))
            File.Delete(sqliteConnectionString);
        
        var sqlGateway = new DatabaseGateway(sqliteConnectionString);
        var environmentManager = new EnvironmentManager(sqlGateway, networkCaracteristics, maxPopulationNumber, crossOverNumber, mutationRate);
        
        var fileText = environmentManager.ExecuteLifeAsync(spaceDimensions, maxNumberOfGeneration, unitLifeTime, selectionRadius, maxNumberToSelect, meanChildNumber).GetAwaiter().GetResult();
        
        var delta = DateTime.UtcNow - start;
        Console.WriteLine($"\nSimulation ended in : {delta.Minutes}:{delta.Seconds}:{delta.Milliseconds}");
        
        if (!File.Exists(logfilepath))
            File.Create(logfilepath);
        
        using (var fileStream = File.OpenWrite(logfilepath))
        {
            var bytes = new UTF8Encoding().GetBytes(fileText.Replace(',', '.'), 0, fileText.Length);
            fileStream.Write(bytes);
            fileStream.Flush();
        }
    }
}