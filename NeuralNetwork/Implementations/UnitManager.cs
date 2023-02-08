using NeuralNetwork.Helpers;
using NeuralNetwork.Interfaces.Model;
using NeuralNetwork.Managers;
using System.Text;

namespace NeuralNetwork.Implementations
{
    public class UnitManager
    {
        private readonly BrainManager _brainManager;
        private readonly Dictionary<int, int> _selectedOuput = new Dictionary<int, int>();
        private readonly Unit _unit;
        private string[] _positions;

        public UnitManager(Brain brain, float[] startingPosition, int lifeTime, int generationId, int simulationId)
        {
            _unit = new Unit
            {
                Identifier = Guid.NewGuid(),
                Position = new SpacePosition(startingPosition),
                LifeTime = lifeTime,
                Fertile = true,
                Brain = brain,
                Age = 1,
                GenerationId = generationId,
                SimulationId = simulationId
            };
            _positions = new string[lifeTime + 1];
            _positions[0] = _unit.Position.ToString();
            _brainManager = new BrainManager(brain);
        }

        public Unit GetUnit => _unit;

        public (Unit unit, string[] positions) GetUnitWithPositions => (_unit, _positions);

        public void ExecuteAction(int actionNumber)
        {
            var inputs = GetDistanceToWalls();
            var inputString = new StringBuilder();
            for(int i = 0; i < inputs.Length; i++)
                inputString.Append($"I{i}:{inputs[i]};");
            var result = _brainManager.ComputeOutput(inputs.ToList());
            StoreOutput(result.ouputId);
            ExecuteOutput(result.ouputId);
            _positions[actionNumber] = _unit.Position.ToString();
            _unit.Age++;
        }

        public Dictionary<int, int> GetLifeTimeOutputs => _selectedOuput;

        private void StoreOutput(int outputId)
        {
            if (_selectedOuput.ContainsKey(outputId))
                _selectedOuput[outputId]++;
            else
                _selectedOuput.Add(outputId, 1);
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

        private float[] GetDistanceToWalls()
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
    }
}
