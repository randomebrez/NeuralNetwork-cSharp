using System.Collections.Generic;
using System;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork.Helpers;
using NeuralNetwork.Interfaces.Model;
using NeuralNetwork.Managers;
using System.Linq;
using NeuralNetwork.Tests.Model;
using NeuralNetwork.Interfaces;
using NeuralNetwork.Interfaces.Model.Etc;

namespace NeuralNetwork.Tests
{
    public class EnvironmentManager
    {
        private readonly IDatabaseGateway _sqlGateway;
        private readonly IPopulationManager _populationManager;
        private readonly List<BrainCaracteristics> _networkCaracteristics;
        private readonly ReproductionCaracteristics _reproductionCaracteristics;

        private int _maxPopulationNumber;
        private Dictionary<Guid, UnitManagerTest> _units = new Dictionary<Guid, UnitManagerTest>();
        private List<UnitTest> _selectedBrains = new List<UnitTest>();

        public EnvironmentManager(IDatabaseGateway sqlGateway, List<BrainCaracteristics> networkCaracteristics, int maxPopulationNumber, ReproductionCaracteristics reproductionCaracteristics)
        {
            _sqlGateway = sqlGateway;
            _populationManager = new PopulationManager();
            _maxPopulationNumber = maxPopulationNumber;
            _networkCaracteristics = networkCaracteristics;
            _reproductionCaracteristics = reproductionCaracteristics;
        }

        public async Task<string> ExecuteLifeAsync(int[] spaceDimensions, int maxNumberOfGeneration, int unitLifeTime, float selectionRadius)
        {
            var logs = new StringBuilder();
            var simulationId = await _sqlGateway.AddNewSimulationAsync();

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
                var survivorNumber = SelectBestUnitsCircular(selectionRadius * StaticSpaceDimension.SpaceDimensions[0].max);
                //var survivorNumber = SelectBestUnitsLateral(0.1f);


                // ToDo : Store best in db


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


        private async Task<string> ExecuteGenerationLifeAsync()
        {
            var logs = new StringBuilder();
            var start = DateTime.UtcNow;

            var loopCounter = _units.First().Value.GetUnit.LifeTime;
            for (int i = 0; i < loopCounter; i++)
            {
                foreach (var unit in _units.Values)
                    unit.ExecuteAction();
            }
            var end1 = DateTime.UtcNow;
            var executingTime = end1 - start;
            logs.AppendLine($"Life execution time: {executingTime.Minutes}:{executingTime.Seconds}:{executingTime.Milliseconds}");

            return logs.ToString();
        }

        private async Task<string> GetNextGenerationAsync(int unitLifeTime, int generationId, int simulationId)
        {
            var start = DateTime.UtcNow;
            var logs = new StringBuilder();

            _units.Clear();
            Unit[] brains;
            if (_selectedBrains.Any())
                brains = _populationManager.GenerateNewGeneration(_maxPopulationNumber, _selectedBrains.Select(t => t.Unit).ToList(), _networkCaracteristics, _reproductionCaracteristics);
            else
                brains = _populationManager.GenerateFirstGeneration(_maxPopulationNumber, _networkCaracteristics);
            for (var index = 0; index < brains.Length; index++)
            {
                if (brains[index] == null)
                    continue;
                var newUnit = new UnitManagerTest(brains[index], GetRandomPosition(), unitLifeTime, generationId, simulationId);
                _units.Add(brains[index].Identifier, newUnit);
            }
            var delta = DateTime.UtcNow - start;
            logs.AppendLine($"New generation generated : {delta.Minutes}:{delta.Seconds}:{delta.Milliseconds}");

            //var dbUnits = _units.Values.Select(t => t.GetUnit).ToList();
            //
            //var start = DateTime.UtcNow;
            //await _sqlGateway.StoreBrainsAsync(simulationId, generationId, dbUnits.Select(t => t.Unit.Brains.First().Value.Brain).ToList()).ConfigureAwait(false);
            //var delta2 = DateTime.UtcNow - start;
            //logs.AppendLine($"New brains stored : {delta2.Minutes}:{delta2.Seconds}:{delta2.Milliseconds}");

            return logs.ToString();
        }


        private int SelectBestUnitsLateral(float xPercent, int meanChildNumber)
        {
            _selectedBrains.Clear();
            var minCoordinateToReach = StaticSpaceDimension.SpaceDimensions[0].min + (1 + xPercent) * (StaticSpaceDimension.SpaceDimensions[0].max - StaticSpaceDimension.SpaceDimensions[0].min) / 2;
            var survivorNumber = 0;
            foreach (var unit in _units.Values)
            {
                if (unit.GetUnit.Position.GetCoordinate(0) >= minCoordinateToReach)
                {
                    _selectedBrains.Add(unit.GetUnit);
                    survivorNumber++;
                }
            }

            var index1 = _selectedBrains.Count / 3;
            for (int i = 0; i < _selectedBrains.Count; i++)
            {
                if (i < index1)
                    _selectedBrains[i].Unit.MaxChildNumber = meanChildNumber + meanChildNumber / 2;
                else if (i < _selectedBrains.Count - index1)
                    _selectedBrains[i].Unit.MaxChildNumber = meanChildNumber;
                else
                    _selectedBrains[i].Unit.MaxChildNumber = meanChildNumber - meanChildNumber / 2;
            }

            return survivorNumber;
        }

        private int SelectBestUnitsCircular(float radius)
        {
            _selectedBrains.Clear();
            var radiusforSurvivor = radius * radius;
            var unitCenterDistances = new Dictionary<Guid, float>();
            foreach (var unit in _units.Values)
            {
                var squareSum = 0f;
                for (int j = 0; j < StaticSpaceDimension.DimensionNumber; j++)
                    squareSum += (float)Math.Pow(unit.GetUnit.Position.GetCoordinate(j), 2);
                unitCenterDistances.Add(unit.GetUnit.Unit.Identifier, squareSum);
            }
            var selectedUnits = unitCenterDistances.OrderBy(t => t.Value);
            var survivorNumber = unitCenterDistances.Where(t => t.Value < radiusforSurvivor).Count();
            var maxNumberToTake = 100 * _reproductionCaracteristics.PercentToSelect / _maxPopulationNumber;
            foreach (var unitPair in selectedUnits.Take(maxNumberToTake))
                _selectedBrains.Add(_units[unitPair.Key].GetUnit);

            var index1 = _selectedBrains.Count / 3;
            for (int i = 0; i < _selectedBrains.Count; i++)
            {
                if (i < index1)
                    _selectedBrains[i].Unit.MaxChildNumber = _reproductionCaracteristics.MeanChildNumberByUnit + _reproductionCaracteristics.MeanChildNumberByUnit / 2;
                else if (i < _selectedBrains.Count - index1)
                    _selectedBrains[i].Unit.MaxChildNumber = _reproductionCaracteristics.MeanChildNumberByUnit;
                else
                    _selectedBrains[i].Unit.MaxChildNumber = _reproductionCaracteristics.MeanChildNumberByUnit - _reproductionCaracteristics.MeanChildNumberByUnit / 2;
            }

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
    }
}