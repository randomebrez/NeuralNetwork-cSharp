using System.Text;
using NeuralNetwork.Interfaces.Model;
using NeuralNetwork.Tests;

var logfilepath = @"C:\Users\nlouviaux\Desktop\Test\neuralTework.txt";
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
var maxPopulationNumber = 100;
var numberOfGenerations = 150;
var unitLifeTime = 150;
var selectionRadius = 0.1f;
int? numberOfBestToSave = null;

var environmentManager = new EnvironmentManager(networkCaracteristics, maxPopulationNumber);

var fileText = environmentManager.ExecuteLife(spaceDimensions, numberOfGenerations, unitLifeTime, selectionRadius, numberOfBestToSave);
fileText = $"{maxPopulationNumber * unitLifeTime}\n{fileText}";
using (var fileStream = File.OpenWrite(logfilepath))
{
    var bytes = new UTF8Encoding().GetBytes(fileText.Replace(',', '.'), 0, fileText.Length);
    fileStream.Write(bytes);
    fileStream.Flush();
}

