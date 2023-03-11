using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NeuralNetwork.DataBase.Abstraction;
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
        var spaceDimensions = new int[] { 50, 50 };
        var maxPopulationNumber = 100;
        var maxNumberOfGeneration = 500;
        var unitLifeTime = 150;
        var selectionRadius = 0.3f;
        var networkCaracteristics = new NetworkCaracteristics
        {
            GeneNumber = 70,
            InputNumber = 4,
            OutputNumber = 8,
            NeutralNumbers = new List<int> { 3, 2 },
            WeighBytesNumber = 4
        };

        // PROGRAM

        // File is automatically recreated when instantiating the 'context'
        if (cleanDatabase && File.Exists(sqliteConnectionString))
            File.Delete(sqliteConnectionString);
        
        var sqlGateway = new DatabaseGateway(new Context(sqliteConnectionString));
        var environmentManager = new EnvironmentManager(sqlGateway, networkCaracteristics, maxPopulationNumber);
        
        var fileText = environmentManager.ExecuteLifeAsync(spaceDimensions, maxNumberOfGeneration, unitLifeTime, selectionRadius, 1 + 2 * maxPopulationNumber / 3).GetAwaiter().GetResult();
        
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