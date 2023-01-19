using NeuralNetwork.Interfaces;
using NeuralNetwork.Interfaces.Model;
using NeuralNetwork.Helpers;
using NeuralNetwork.Implementations;

namespace NeuralNetwork.Managers;

public class PopulationManager : IPopulation
{
    private HashSet<string> _geneCodes;
    private Dictionary<string, Neuron> _neuronsDict;
    private BrainNeurons _baseNeurons;
    private DatabaseGateway _sqlGateway;

    private readonly NetworkCaracteristics _dimension;

    public PopulationManager(DatabaseGateway sqlGateway, NetworkCaracteristics dimension)
    {
        _dimension = dimension;
        _sqlGateway = sqlGateway;
        InitialyzeAsync().GetAwaiter().GetResult();
    }

    public Brain[] GenerateFirstGeneration(int childNumber)
    {
        var newBrains = new Brain[childNumber];
        for (int i = 0; i < childNumber; i++)
        {
            var newBrain = BrainHelper.GenerateRandomBrain(_dimension, _baseNeurons, _geneCodes.ToList(), _neuronsDict);
            if (newBrain != null)
                newBrains[i] = newBrain;
        }
        return newBrains;
    }
    
    public Brain[] GenerateNewGeneration(int childNumber, List<Brain> selectedBrains)
    {
        var newBrains = new Brain[childNumber];
        var maximumTry = 10;
        for (int i = 0; i < childNumber; i++)
        {
            var validBrain = false;
            var currentTry = 0;
            var brain = new Brain();
            while (validBrain == false && currentTry < maximumTry)
            {
                currentTry++;
                brain = GetChild(selectedBrains);
                validBrain = brain != null;
            }
            if (validBrain)
                newBrains[i] = brain;
        }

        return newBrains;
    }

    private Brain GetChild(List<Brain> selectedBrains)
    {
        var genomeA = selectedBrains[StaticHelper.GetRandomValue(0, selectedBrains.Count - 1)].Genome;
        var genomeB = selectedBrains[StaticHelper.GetRandomValue(0, selectedBrains.Count - 1)].Genome;
        var mixedGenome = genomeA.CrossOver(genomeB);
        mixedGenome.DeepCopy().MutateGenome(_geneCodes.ToList());
        return mixedGenome.GenerateBrainFromGenome(_baseNeurons, _neuronsDict);
    }

    private async Task InitialyzeAsync()
    {
        _neuronsDict = new Dictionary<string, Neuron>();
        _geneCodes = new HashSet<string>();

        _baseNeurons = BrainHelper.GetBaseNeurons(_dimension);
        _geneCodes = GetGeneCodes();
        await _sqlGateway.CreateVerticesAsync(_geneCodes);

        foreach (var inputNeuron in _baseNeurons.Inputs)
            _neuronsDict.Add(inputNeuron.UniqueId, inputNeuron);
        foreach (var neutralNeuron in _baseNeurons.Neutrals)
            _neuronsDict.Add(neutralNeuron.UniqueId, neutralNeuron);
        foreach (var outputNeuron in _baseNeurons.Outputs)
            _neuronsDict.Add(outputNeuron.UniqueId, outputNeuron);
    }

    private HashSet<string> GetGeneCodes()
    {
        var geneCodes = new HashSet<string>();
        for (int i = 0; i < _dimension.InputNumber; i++)
        {
            for (int j = 0; j < _dimension.NeutralNumber; j++)
            {
                geneCodes.Add($"I:{i}-N:{j}");
            }
            //for (int k = 0; k < _dimension.OutputNumber; k++)
            //{
            //    geneCodes.Add($"I:{i}-O:{k}");
            //}
        }
        for (int i = 0; i < _dimension.NeutralNumber; i++)
        {
            //for (int j = 0; j < _dimension.NeutralNumber; j++)
            //{
            //    geneCodes.Add($"N:{i}-N:{j}");
            //}
            for (int k = 0; k < _dimension.OutputNumber; k++)
            {
                geneCodes.Add($"N:{i}-O:{k}");
            }
        }

        return geneCodes;
    }
}