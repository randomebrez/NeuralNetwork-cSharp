﻿using System.Collections.Generic;
using System.Linq;

namespace BrainEncryption.Abstraction.Model
{
    public class NetworkCaracteristics
    {
        public NetworkCaracteristics()
        {
            NeutralLayers = new List<LayerCaracteristics>();
        }

        public string CaracteristicTemplateName { get; set; }

        public string BrainName { get; set; }

        public LayerCaracteristics InputLayer { get; set; }

        public List<LayerCaracteristics> NeutralLayers { get; set; }

        public LayerCaracteristics Outputlayer { get; set; }

        public int OutputLayerId => 1 + NeutralLayers.Count();
    }
}
