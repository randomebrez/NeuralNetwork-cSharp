using System.Text;
using NeuralNetwork.Helpers;
using NeuralNetwork.Implementations;
using NeuralNetwork.Interfaces.Model;
using NeuralNetwork.Managers;

namespace NeuralNetwork.Tests;

public class EnvironmentManager
{
    private readonly DatabaseGateway _sqlGateway;
    private readonly PopulationManager _populationManager;

    private int _maxPopulationNumber;
    private Dictionary<Guid, UnitManager> _units = new Dictionary<Guid, UnitManager>();
    private List<Brain> _selectedBrains = new List<Brain>();
    private readonly NetworkCaracteristics _networkCaracteristics;
    
    public EnvironmentManager(DatabaseGateway sqlGateway, NetworkCaracteristics networkCaracteristics, int maxPopulationNumber)
    {
        _sqlGateway = sqlGateway;
        _populationManager = new PopulationManager(sqlGateway, networkCaracteristics);
        _maxPopulationNumber = maxPopulationNumber;
        _networkCaracteristics = networkCaracteristics;
    }

    public async Task<string> ExecuteLifeAsync(int[] spaceDimensions, int maxNumberOfGeneration, int unitLifeTime, float selectionRadius, int? nubmberOfBestToSave)
    {
        var logs = new StringBuilder();
        var simulationId = await _sqlGateway.AddNewSimulationAsync();

        // Initialyze the life space
        StaticSpaceDimension.SpaceDimensions = new Dictionary<int, (int min, int max)>();
        for(int i = 0; i < spaceDimensions.Length; i ++)
            StaticSpaceDimension.SpaceDimensions.Add(i, (-spaceDimensions[i], spaceDimensions[i]));
        var survivorNumber = 0;
        var generationNumber = 0;
        var successCount = 0;

        //Loop on generations
        while (successCount < 10 && generationNumber < maxNumberOfGeneration)
        {
            var start = DateTime.UtcNow;
            Console.WriteLine($"Starting Generation {generationNumber}");
            // Get the population
            await GetNextGenerationAsync(unitLifeTime, generationNumber + 1, simulationId);

            //Make them live
            await ExecuteGenerationLifeAsync().ConfigureAwait(false);

            //Select The Best
            survivorNumber = SelectBestUnitsCircular(selectionRadius * StaticSpaceDimension.SpaceDimensions[0].max, nubmberOfBestToSave);
            //survivorNumber = SelectBestUnitsLateral(0.1f);

            generationNumber++;
            var survivorPercent = 100 * ((float)survivorNumber / (float)_maxPopulationNumber);
            if (survivorPercent > 80)
                successCount++;
            else
                successCount = 0;

            var deltaT = DateTime.UtcNow - start;
            Console.WriteLine($"Total time for generation {generationNumber} : {deltaT.Minutes}:{deltaT.Seconds}:{deltaT.Milliseconds}");
            Console.WriteLine($"Survivor number : {survivorNumber} - Percent : {survivorPercent} %\n\n");
        }
        return logs.ToString();
    }

    private async Task GetNextGenerationAsync(int unitLifeTime, int generationId, int simulationId)
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
            var newUnit = new UnitManager(t, GetRandomPosition(), unitLifeTime, generationId, simulationId);
            _units.Add(newUnit.GetUnit.Identifier, newUnit);
        }
        var start = DateTime.UtcNow;
        await _sqlGateway.StoreUnitsAsync(_units.Values.Select(t => t.GetUnit).ToList()).ConfigureAwait(false);
        var end1 = DateTime.UtcNow;
        var delta1 = end1 - start;
        Console.WriteLine($"New units stored : {delta1.Minutes}:{delta1.Seconds}:{delta1.Milliseconds}");
        await _sqlGateway.StoreUnitBrainsAsync(_units.Values.Select(t => t.GetUnit).ToList()).ConfigureAwait(false);
        var delta2 = DateTime.UtcNow - end1;
        Console.WriteLine($"New brains stored : {delta2.Minutes}:{delta2.Seconds}:{delta2.Milliseconds}");
    }

    private async Task ExecuteGenerationLifeAsync()
    {
        var start = DateTime.UtcNow;

        var loopCounter = _units.First().Value.GetUnit.LifeTime;
        for (int i = 0; i < loopCounter; i++)
        {
            foreach (var unit in _units.Values)
                unit.ExecuteAction(i + 1);
        }
        var end1 = DateTime.UtcNow;
        var executingTime = end1 - start;
        Console.WriteLine($"Life execution time: {executingTime.Minutes}:{executingTime.Seconds}:{executingTime.Milliseconds}");

        await _sqlGateway.StoreLifeStepAsync(_units.Values.Select(t => t.GetUnitWithPositions).ToList());
        var end2 = DateTime.UtcNow;
        var storingTime = end2 - end1;
        Console.WriteLine($"Unit steps stored in : {storingTime.Minutes}:{storingTime.Seconds}:{storingTime.Milliseconds}");
    }
    
    private int SelectBestUnitsCircular(float radius, int? maxNumberToTake)
    {
        _selectedBrains.Clear();
        var radiusToReach = radius * radius;
        var unitCenterDistances = new Dictionary<Guid, float>();
        foreach (var unit in _units.Values)
        {
            var squareSum = 0f;
            for (int j = 0; j < StaticSpaceDimension.DimensionNumber; j++)
                squareSum += (float)Math.Pow(unit.GetUnit.Position.GetCoordinate(j), 2);
            unitCenterDistances.Add(unit.GetUnit.Identifier, squareSum);
        }
        var selectedUnits = unitCenterDistances.OrderBy(t => t.Value).Where(t => t.Value < radiusToReach);
        var survivorNumber = selectedUnits.Count();
        foreach (var unitPair in selectedUnits.Take(maxNumberToTake??survivorNumber))
            _selectedBrains.Add(_units[unitPair.Key].GetUnit.Brain);

        return survivorNumber;
    }
    
    private float[] GetRandomPosition()
    {
        var result = new float[StaticSpaceDimension.DimensionNumber];
        for (int i = 0; i < StaticSpaceDimension.DimensionNumber; i++)
            result[i] = StaticHelper.GetRandomValue(StaticSpaceDimension.SpaceDimensions[i].min, StaticSpaceDimension.SpaceDimensions[i].max);
        return result;
    }   
    
    
    
    private string SavePosition()
    {
        var logs = new StringBuilder();
        foreach (var unit in _units.Values)
        {
            logs.AppendLine($"{unit.GetUnit.Position.GetCoordinate(0)};{unit.GetUnit.Position.GetCoordinate(1)}");
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
            if (unit.GetUnit.Position.GetCoordinate(0) >= minCoordinateToReach)
            {
                _selectedBrains.Add(unit.GetUnit.Brain);
                survivorNumber++;
            }
        }

        return survivorNumber;
    }
}