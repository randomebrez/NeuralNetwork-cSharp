﻿using System;
using System.Collections.Generic;

namespace NeuralNetwork.Interfaces.Model
{
    public class Unit
    {
        public Unit()
        {
            Identifier = Guid.NewGuid();
            Brains = new Dictionary<string, BrainGenomePair>();
        }

        public Guid Identifier { get; set; }

        public Guid ParentA { get; set; }

        public Guid ParentB { get; set; }

        public Dictionary<string, BrainGenomePair> Brains { get; set; }

        public GenomeGraph GenomeGraph { get; set; }

        public BrainGraph BrainGraph { get; set; }

        public int UseForChildCounter { get; set; }

        public int MaxChildNumber { get; set; } = 3;
    }
}