using System.Collections.Generic;
using System;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork.Helpers;
using NeuralNetwork.Implementations;
using NeuralNetwork.Interfaces.Model;
using NeuralNetwork.Managers;
using System.Linq;

namespace NeuralNetwork.Tests
{
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
            _populationManager = new PopulationManager(networkCaracteristics);
            _maxPopulationNumber = maxPopulationNumber;
            _networkCaracteristics = networkCaracteristics;
        }

        public async Task<string> ExecuteLifeAsync(int[] spaceDimensions, int maxNumberOfGeneration, int unitLifeTime, float selectionRadius, int numberOfBestToTake)
        {
            var logs = new StringBuilder();
            var simulationId = await _sqlGateway.AddNewSimulationAsync();
            await _sqlGateway.CreateVerticesAsync(_populationManager.GeneCodes);

            // Initialyze the life space
            StaticSpaceDimension.SpaceDimensions = new Dictionary<int, (int min, int max)>();
            for (int i = 0; i < spaceDimensions.Length; i++)
                StaticSpaceDimension.SpaceDimensions.Add(i, (-spaceDimensions[i], spaceDimensions[i]));

            var generationNumber = 0;
            var successCount = 0;

            //Loop on generations
            while (successCount < 3 && generationNumber < maxNumberOfGeneration)
            {
                var currentLog = new StringBuilder();
                var start = DateTime.UtcNow;
                currentLog.AppendLine($"Starting Generation {generationNumber}");
                // Get the population
                currentLog.Append(await GetNextGenerationAsync(unitLifeTime, generationNumber, simulationId).ConfigureAwait(false));

                //Make them live
                currentLog.AppendLine(await ExecuteGenerationLifeAsync().ConfigureAwait(false));

                //Select The Best
                var survivorNumber = SelectBestUnitsCircular(selectionRadius * StaticSpaceDimension.SpaceDimensions[0].max, numberOfBestToTake);
                //var survivorNumber = SelectBestUnitsLateral(0.1f);

                var survivorPercent = 100 * ((float)survivorNumber / (float)_maxPopulationNumber);
                if (survivorPercent > 98)
                    successCount++;
                else
                    successCount = 0;

                var deltaT = DateTime.UtcNow - start;
                currentLog.AppendLine($"Total time for generation {generationNumber} : {deltaT.Minutes}:{deltaT.Seconds}:{deltaT.Milliseconds}");
                currentLog.AppendLine($"Survivor number : {survivorNumber} - Percent : {survivorPercent} %\n\n");
                Console.WriteLine(currentLog.ToString());
                logs.AppendLine(currentLog.ToString());

                generationNumber++;
            }
            return logs.ToString();
        }

        private async Task<string> GetNextGenerationAsync(int unitLifeTime, int generationId, int simulationId)
        {
            var logs = new StringBuilder();

            _units.Clear();
            Brain[] brains;
            if (_selectedBrains.Any())
                brains = _populationManager.GenerateNewGeneration(_maxPopulationNumber, _selectedBrains);
            else
                brains = _populationManager.GenerateFirstGeneration(_maxPopulationNumber);
            for (var index = 0; index < brains.Length; index++)
            {
                if (brains[index] == null)
                    continue;
                var newUnit = new UnitManager(brains[index], GetRandomPosition(), unitLifeTime, generationId, simulationId);
                _units.Add(newUnit.GetUnit.Identifier, newUnit);
            }
            var dbUnits = _units.Values.Select(t => t.GetUnit).ToList();
            var start = DateTime.UtcNow;
            await _sqlGateway.StoreUnitsAsync(dbUnits).ConfigureAwait(false);
            var end1 = DateTime.UtcNow;
            var delta1 = end1 - start;
            logs.AppendLine($"New units stored : {delta1.Minutes}:{delta1.Seconds}:{delta1.Milliseconds}");

            await _sqlGateway.StoreUnitBrainsAsync(dbUnits).ConfigureAwait(false);
            var delta2 = DateTime.UtcNow - end1;
            logs.AppendLine($"New brains stored : {delta2.Minutes}:{delta2.Seconds}:{delta2.Milliseconds}");

            return logs.ToString();
        }

        private async Task<string> ExecuteGenerationLifeAsync()
        {
            var logs = new StringBuilder();
            var start = DateTime.UtcNow;

            var loopCounter = _units.First().Value.GetUnit.LifeTime;
            for (int i = 0; i < loopCounter; i++)
            {
                foreach (var unit in _units.Values)
                    unit.ExecuteAction(i + 1);
            }
            var end1 = DateTime.UtcNow;
            var executingTime = end1 - start;
            logs.AppendLine($"Life execution time: {executingTime.Minutes}:{executingTime.Seconds}:{executingTime.Milliseconds}");

            await _sqlGateway.StoreLifeStepAsync(_units.Values.Select(t => t.GetUnitWithPositions).ToList());
            var end2 = DateTime.UtcNow;
            var storingTime = end2 - end1;
            logs.AppendLine($"Unit steps stored in : {storingTime.Minutes}:{storingTime.Seconds}:{storingTime.Milliseconds}");

            return logs.ToString();
        }

        private int SelectBestUnitsCircular(float radius, int? maxNumberToTake)
        {
            _selectedBrains.Clear();
            var radiusToReach = 8000;
            var radiusforSurvivor = radius * radius;
            var unitCenterDistances = new Dictionary<Guid, float>();
            foreach (var unit in _units.Values)
            {
                var squareSum = 0f;
                for (int j = 0; j < StaticSpaceDimension.DimensionNumber; j++)
                    squareSum += (float)Math.Pow(unit.GetUnit.Position.GetCoordinate(j), 2);
                unitCenterDistances.Add(unit.GetUnit.Identifier, squareSum);
            }
            var selectedUnits = unitCenterDistances.OrderBy(t => t.Value).Where(t => t.Value < radiusToReach);
            var survivorNumber = unitCenterDistances.Where(t => t.Value < radiusforSurvivor).Count();
            foreach (var unitPair in selectedUnits.Take(maxNumberToTake ?? survivorNumber))
                _selectedBrains.Add(_units[unitPair.Key].GetUnit.Brain);

            if (selectedUnits.Any())
                Console.WriteLine($"High score = {selectedUnits.First().Value}");
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
}