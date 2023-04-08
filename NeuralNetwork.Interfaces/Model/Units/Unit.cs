using System;
using System.Collections.Generic;

namespace NeuralNetwork.Interfaces.Model
{
    public class Unit
    {
        public int Id { get; set; }

        public Guid Identifier { get; set; }

        public Guid ParentA { get; set; }

        public Guid ParentB { get; set; }

        public Dictionary<string> Brain { get; set; }

        public Genome Genome { get; set; }
    }
}