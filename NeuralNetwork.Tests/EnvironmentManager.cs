using System.Text;
using NeuralNetwork.Helpers;
using NeuralNetwork.Interfaces.Model;
using NeuralNetwork.Managers;
using NeuralNetwork.Tests.Model;

namespace NeuralNetwork.Tests;

public class EnvironmentManager
{
    private readonly PopulationManager _populationManager;

    private int _maxPopulationNumber;
    private Dictionary<Guid, Unit> _units = new Dictionary<Guid, Unit>();
    private List<Brain> _selectedBrains = new List<Brain>();
    private readonly Dictionary<int, string> _generationsOutputs = new Dictionary<int, string>();
    private readonly NetworkCaracteristics _networkCaracteristics;
    
    public EnvironmentManager(NetworkCaracteristics networkCaracteristics, int maxPopulationNumber)
    {
        _populationManager = new PopulationManager(networkCaracteristics);
        _maxPopulationNumber = maxPopulationNumber;
        _networkCaracteristics = networkCaracteristics;
    }

    public string ExecuteLife(int[] spaceDimensions, int numberOfGenerations, int unitLifeTime, float selectionRadius, int? nubmberOfBestToSave)
    {
        var logs = new StringBuilder();
        
        // Initialyze the life space
        StaticSpaceDimension.SpaceDimensions = new Dictionary<int, (int min, int max)>();
        for(int i = 0; i < spaceDimensions.Length; i ++)
            StaticSpaceDimension.SpaceDimensions.Add(i, (-spaceDimensions[i], spaceDimensions[i]));

        //Loop on generations
        for (int i = 0; i < numberOfGenerations; i++)
        {
            Console.WriteLine($"Starting Generation {i}");
            // Get the population
            GetNextGeneration(unitLifeTime);
            
            //save initial position
            //logs.Append(SavePosition());
            
            //Make them live
            ExecuteGenerationLife();

            // Store information to log concerning current generation
            StoreGenerationInformation(i);
            
            //logs.Append(SavePosition());
            
            //Select The Best
            var numberOfSurvivors = SelectBestUnits(selectionRadius * StaticSpaceDimension.SpaceDimensions[0].max, nubmberOfBestToSave);
            //var numberOfSurvivors = SelectBestUnitsLateral(0.8f);
            //Console.WriteLine($"{i};{numberOfSurvivors};{_generationsOutputs[i]}");
            
            //Save logs
            logs.AppendLine($"{i};{numberOfSurvivors};{_generationsOutputs[i]}");
        }
        return logs.ToString();
    }

    private void GetNextGeneration(int unitLifeTime)
    {
        _units.Clear();
        Brain[] brains;
        if (_selectedBrains.Any())
            brains = _populationManager.GenerateNewGeneration(_maxPopulationNumber, _selectedBrains);
        else 
            brains = _populationManager.GenerateFirstGeneration(_maxPopulationNumber);
        for (var index = 0; index < brains.Length; index++)
        {
            var t = brains[index];
            var newUnit = new Unit(t, GetRandomPosition(), unitLifeTime);
            _units.Add(newUnit.Identifier, newUnit);
        }
    }

    private void ExecuteGenerationLife()
    {
        var loopCounter = _units.First().Value.LifeTime;
        for (int i = 0; i < loopCounter; i++)
        {
            foreach (var unit in _units.Values)
                unit.ExecuteAction();
        }
    }
    
    private int SelectBestUnits(float radius, int? maxNumberToTake)
    {
        _selectedBrains.Clear();
        var radiusToReach = radius * radius;
        var unitCenterDistances = new Dictionary<Guid, float>();
        foreach (var unit in _units.Values)
        {
            var squareSum = 0f;
            for (int j = 0; j < StaticSpaceDimension.DimensionNumber; j++)
                squareSum += (float)Math.Pow(unit.Position.GetCoordinate(j), 2);
            unitCenterDistances.Add(unit.Identifier, squareSum);
        }
        var selectedUnits = unitCenterDistances.OrderBy(t => t.Value).Where(t => t.Value < radiusToReach);
        var survivorNumber = selectedUnits.Count();
        foreach (var unitPair in selectedUnits.Take(maxNumberToTake??survivorNumber))
            _selectedBrains.Add(_units[unitPair.Key].Brain);

        return survivorNumber;
    }
    
    private float[] GetRandomPosition()
    {
        var result = new float[StaticSpaceDimension.DimensionNumber];
        for (int i = 0; i < StaticSpaceDimension.DimensionNumber; i++)
            result[i] = StaticHelper.GetRandomValue(StaticSpaceDimension.SpaceDimensions[i].min, StaticSpaceDimension.SpaceDimensions[i].max);
        return result;
    }
    
    private void StoreGenerationInformation(int generationIndex)
    {
        var dict = new Dictionary<int, int>();
        var storingString = new StringBuilder();
        
        //from -1 to count sink outputs
        for(int i = -1 ; i < _networkCaracteristics.OutputNumber; i++)
            dict.Add(i,0);
        foreach (var unit in _units.Values)
        {
            var unitOutputs = unit.GetLifeTimeOutputs;
            foreach (var outputs in unitOutputs.Where(t => t.Value > 0))
                dict[outputs.Key] += outputs.Value;
        }
        foreach (var counter in dict.Values)
            storingString.Append($"{counter};");
        storingString.Remove(storingString.Length - 1, 1);
        _generationsOutputs.Add(generationIndex, storingString.ToString());
    }
    
    
    
    
    private string SavePosition()
    {
        var logs = new StringBuilder();
        foreach (var unit in _units.Values)
        {
            logs.AppendLine($"{unit.Position.GetCoordinate(0)};{unit.Position.GetCoordinate(1)}");
        }

        return logs.ToString();
    }
    
    private int SelectBestUnitsLateral(float xPercent)
    {
        _selectedBrains.Clear();
        var minCoordinateToReach = StaticSpaceDimension.SpaceDimensions[0].min + (1 + xPercent) * (StaticSpaceDimension.SpaceDimensions[0].max - StaticSpaceDimension.SpaceDimensions[0].min) / 2;
        var survivorNumber = 0;
        foreach (var unit in _units.Values)
        {
            if (unit.Position.GetCoordinate(0) >= minCoordinateToReach)
            {
                _selectedBrains.Add(unit.Brain);
                survivorNumber++;
            }
        }

        return survivorNumber;
    }
}