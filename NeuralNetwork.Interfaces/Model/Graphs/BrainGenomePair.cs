﻿using NeuralNetwork.Interfaces.Model.Caracteristics;

namespace NeuralNetwork.Interfaces.Model
{
    public class GenomeCaracteristicPair
    {
        public Genome Genome { get; set; }

        public BrainCaracteristics Caracteristics { get; set; }
    }

    public class BrainGenomePair : GenomeCaracteristicPair
    {
        public string Key { get; set; }

        public Brain Brain { get; set; }
    }
}
