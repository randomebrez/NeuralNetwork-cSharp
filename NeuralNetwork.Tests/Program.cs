using System.Text;
using NeuralNetwork.Interfaces.Model;
using NeuralNetwork.Tests;

var logfilepath = @"C:\Users\nlouviaux\Desktop\Test\neuralTework.txt";
if (File.Exists(logfilepath))
    File.Delete(logfilepath);

var networkCaracteristics = new NetworkCaracteristics
{
    GeneNumber = 10,
    InputNumber = 4,
    OutputNumber = 4,
    NeutralNumber = 2,
    WeighBytesNumber = 4
};

var totalNumberOfGenes = (networkCaracteristics.InputNumber + networkCaracteristics.NeutralNumber) *
                         (networkCaracteristics.OutputNumber + networkCaracteristics.NeutralNumber);

var spaceDimensions = new int[] { 50, 50 };
var maxPopulationNumber = 100;
var numberOfGenerations = 200;
var unitLifeTime = 200;
var selectionRadius = 0.2f;
int? numberOfBestToSave = null;

var environmentManager = new EnvironmentManager(networkCaracteristics, maxPopulationNumber);

var fileText = environmentManager.ExecuteLife(spaceDimensions, numberOfGenerations, unitLifeTime, selectionRadius, numberOfBestToSave);

using (var fileStream = File.OpenWrite(logfilepath))
{
    var bytes = new UTF8Encoding().GetBytes(fileText.Replace(',', '.'), 0, fileText.Length);
    fileStream.Write(bytes);
    fileStream.Flush();
}

