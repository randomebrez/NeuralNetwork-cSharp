﻿using System.Collections.Generic;

namespace NeuralNetwork.Interfaces.Model
{
    public class Brain
    {
        public string Name { get; set; }

        public BrainNeurons Neurons { get; set; }

        public List<Edge> Edges { get; set; }

        public int OutputLayerId { get; set; }
    }
}