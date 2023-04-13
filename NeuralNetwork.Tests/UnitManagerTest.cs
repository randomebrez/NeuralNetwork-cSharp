using NeuralNetwork.Helpers;
using NeuralNetwork.Interfaces;
using NeuralNetwork.Interfaces.Model;
using NeuralNetwork.Managers;
using NeuralNetwork.Tests.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuralNetwork.Tests
{
    public class UnitManagerTest
    {
        private readonly IUnitBrains _unitManager;
        private readonly Dictionary<int, int> _selectedOuput = new Dictionary<int, int>();
        private readonly UnitTest _unit;
        private string[] _positions;

        public UnitManagerTest(Unit unit, float[] startingPosition, int lifeTime, int generationId, int simulationId)
        {
            _unit = new UnitTest
            {
                Unit= unit,
                LifeTime = lifeTime,
                GenerationId = generationId,
                SimulationId = simulationId,
                Position = new SpacePosition(startingPosition)
            };
            _positions = new string[lifeTime + 1];
            _positions[0] = _unit.Position.ToString();
            _unitManager = new UnitManager(unit);
        }

        public UnitTest GetUnit => _unit;

        public (UnitTest unit, string[] positions) GetUnitWithPositions => (_unit, _positions);

        public Dictionary<int, int> GetLifeTimeOutputs => _selectedOuput;


        public void ExecuteAction()
        {
            var inputs = GetInputs();
            _unitManager.ComputeBrain("Main", inputs.ToList());

            var result = _unitManager.GetBestOutput("Main");
            ExecuteOutput(result.ouputId);

            _unit.Age++;
            _positions[_unit.Age] = _unit.Position.ToString();
        }

        private float[] GetInputs()
        {
            var distances = new float[2 * StaticSpaceDimension.DimensionNumber];
            for (int i = 0; i < StaticSpaceDimension.DimensionNumber; i++)
            {
                var maxDistance = Math.Abs(StaticSpaceDimension.SpaceDimensions[i].max - StaticSpaceDimension.SpaceDimensions[i].min);
                distances[2 * i] = 1 - Math.Abs(StaticSpaceDimension.SpaceDimensions[i].max - _unit.Position.GetCoordinate(i)) / maxDistance;
                distances[2 * i + 1] = 1 - Math.Abs(StaticSpaceDimension.SpaceDimensions[i].min - _unit.Position.GetCoordinate(i)) / maxDistance;
            }
            return distances;
        }

        private void ExecuteOutput(int outputId)
        {
            var jumpSize = 1f;
            switch (outputId)
            {
                //Sink Output
                case -1:
                    var randomDimension = StaticHelper.GetRandomValue(0, StaticSpaceDimension.DimensionNumber - 1);
                    var randomValue = StaticHelper.GetBooleanValue() ? 1 : -1;
                    Move(randomDimension, randomValue);
                    break;
                case 0:
                    Move(0, jumpSize);
                    break;
                case 1:
                    Move(0, -jumpSize);
                    break;
                case 2:
                    Move(1, jumpSize);
                    break;
                case 3:
                    Move(1, -jumpSize);
                    break;
                case 4:
                    Move(0, jumpSize / 2);
                    Move(1, jumpSize / 2);
                    break;
                case 5:
                    Move(0, -jumpSize / 2);
                    Move(1, jumpSize / 2);
                    break;
                case 6:
                    Move(0, -jumpSize / 2);
                    Move(1, -jumpSize / 2);
                    break;
                case 7:
                    Move(0, jumpSize / 2);
                    Move(1, -jumpSize / 2);
                    break;
            }

        }

        private void Move(int dimensionIndex, float value)
        {
            if (dimensionIndex >= StaticSpaceDimension.DimensionNumber)
                throw new Exception(
                    $"Cannot move to that direction. Max dimension {StaticSpaceDimension.DimensionNumber}. Requested: {dimensionIndex}");
            var isMoveLegit = IsMoveLegit(dimensionIndex, value);
            if (isMoveLegit.legit)
                _unit.Position.SetCoordinate(dimensionIndex, isMoveLegit.finalPosition);
        }

        private (bool legit, float finalPosition) IsMoveLegit(int dimensionIndex, float value)
        {
            var result = _unit.Position.GetCoordinate(dimensionIndex) + value;
            return (result <= StaticSpaceDimension.SpaceDimensions[dimensionIndex].max && result >= StaticSpaceDimension.SpaceDimensions[dimensionIndex].min, result);
        }

        private void StoreOutput(int outputId)
        {
            if (_selectedOuput.ContainsKey(outputId))
                _selectedOuput[outputId]++;
            else
                _selectedOuput.Add(outputId, 1);
        }
    }
}
