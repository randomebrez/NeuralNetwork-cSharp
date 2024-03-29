﻿using System.Text;

namespace BrainEncryption.Abstraction.Model
{
    public class Genome
    {
        public Genome(int geneNumber)
        {
            GeneNumber = geneNumber;
            Genes = new Gene[geneNumber];
        }


        public int GeneNumber { get; set; }
        public Gene[] Genes { get; set; }

        public override string ToString()
        {
            var result = new StringBuilder();
            for (int i = 0; i < GeneNumber; i++)
                result.Append($"{Genes[i]}!");

            return result.ToString();
        }
    }
}
