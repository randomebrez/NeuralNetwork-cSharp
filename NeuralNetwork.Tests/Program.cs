using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NeuralNetwork.Helpers;
using NeuralNetwork.Implementations;
using NeuralNetwork.Interfaces.Model;
using NeuralNetwork.Interfaces.Model.Etc;
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
        var reproductionCaracteristics = new ReproductionCaracteristics
        {
            PercentToSelect = 30,
            MeanChildNumberByUnit = 5,
            CrossOverNumber = 2,
            MutationRate = 0.001f
        };

        var inputLayer = new LayerCaracteristics(LayerTypeEnum.Input, 0, 4, ActivationFunctionEnum.Identity, 0);
        var neutralLayer_1 = new LayerCaracteristics(LayerTypeEnum.Neutral, 1, 2, ActivationFunctionEnum.Tanh, 2);
        var neutralLayer_2 = new LayerCaracteristics(LayerTypeEnum.Neutral, 2, 2, ActivationFunctionEnum.Tanh, 3);
        var outputLayer = new LayerCaracteristics(LayerTypeEnum.Output, 2, 8, ActivationFunctionEnum.Sigmoid, 3);

        var networkCaracteristics = new List<BrainCaracteristics>
        {
            new BrainCaracteristics()
            {
                BrainName = "Main",
                InputLayer = inputLayer,
                OutputLayer = outputLayer,
                NeutralLayers = new List<LayerCaracteristics> { neutralLayer_1, neutralLayer_2 },
                GenomeCaracteristics = new GenomeCaracteristics
                {
                    GeneNumber = 30,
                    WeighBytesNumber = 3
                }
            }
        };

        // PROGRAM

        // File is automatically recreated when instantiating the 'context'
        if (cleanDatabase && File.Exists(sqliteConnectionString))
            File.Delete(sqliteConnectionString);
        
        var sqlGateway = new DatabaseGateway(sqliteConnectionString);
        var environmentManager = new EnvironmentManager(sqlGateway, networkCaracteristics, maxPopulationNumber, reproductionCaracteristics);
        
        var fileText = environmentManager.ExecuteLifeAsync(spaceDimensions, maxNumberOfGeneration, unitLifeTime, selectionRadius).GetAwaiter().GetResult();
        
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