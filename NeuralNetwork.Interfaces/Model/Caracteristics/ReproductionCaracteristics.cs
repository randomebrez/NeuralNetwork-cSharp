namespace NeuralNetwork.Abstraction.Model
{
    public class ReproductionCaracteristics
    {
        public int PercentToSelect { get; set; }
        public int MeanChildNumberByUnit { get; set; }
        public int CrossOverNumber { get; set; }
        public float MutationRate { get; set; }
    }
}
