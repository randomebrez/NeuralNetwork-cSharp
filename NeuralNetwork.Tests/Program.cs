﻿using System.Diagnostics;
using System.Text;
using NeuralNetwork.DataBase.Abstraction;
using NeuralNetwork.Implementations;
using NeuralNetwork.Interfaces.Model;
using NeuralNetwork.Tests;

var start = DateTime.UtcNow;
// config sur fixe
//var logfilepath = @"D:\Codes\Test\neuralTework.txt";
//var sqliteConnectionString = @"D:\Codes\Test\NeuralNetworkDatabase.txt";

//Config sur pc portable
var logfilepath = @"C:\Users\nico-\Documents\Codes\Tests\neuralTework.txt";
var sqliteConnectionString =  @"C:\Users\nico-\Documents\Codes\Tests\NeuralNetworkDatabase.txt";

if (File.Exists(logfilepath))
    File.Delete(logfilepath);

var networkCaracteristics = new NetworkCaracteristics
{
    GeneNumber = 32,
    InputNumber = 4,
    OutputNumber = 8,
    NeutralNumber = 2,
    WeighBytesNumber = 4
};

var totalNumberOfGenes = (networkCaracteristics.InputNumber + networkCaracteristics.NeutralNumber) *
                         (networkCaracteristics.OutputNumber + networkCaracteristics.NeutralNumber);

var spaceDimensions = new int[] { 50, 50 };
var maxPopulationNumber = 150;
var maxNumberOfGeneration = 500;
var unitLifeTime = 150;
var selectionRadius = 0.3f;
int? numberOfBestToSave = null;

var sqlGateway = new DatabaseGateway(new Context(sqliteConnectionString));
var environmentManager = new EnvironmentManager(sqlGateway, networkCaracteristics, maxPopulationNumber);

var fileText = environmentManager.ExecuteLifeAsync(spaceDimensions, maxNumberOfGeneration, unitLifeTime, selectionRadius, numberOfBestToSave).GetAwaiter().GetResult();

var delta = DateTime.UtcNow - start;
Console.WriteLine($"\nSimulation ended in : {delta.Minutes}:{delta.Seconds}:{delta.Milliseconds}");

using (var fileStream = File.OpenWrite(logfilepath))
{
    var bytes = new UTF8Encoding().GetBytes(fileText.Replace(',', '.'), 0, fileText.Length);
    fileStream.Write(bytes);
    fileStream.Flush();
}