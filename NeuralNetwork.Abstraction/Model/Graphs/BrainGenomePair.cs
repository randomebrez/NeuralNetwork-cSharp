using System;

namespace NeuralNetwork.Abstraction.Model
{
    public class GenomeCaracteristicPair : GraphableObject
    {
        public override string Id { get; set; } = Guid.NewGuid().ToString();

        public Genome Genome { get; set; }

        public BrainCaracteristics Caracteristics { get; set; }
    }

    public class BrainGenomePair : GenomeCaracteristicPair
    {
        public override string Id { get => Brain.Name; set => Id = value; }

        public Brain Brain { get; set; }
    }
}
