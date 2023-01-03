namespace NeuralNetwork.Interfaces.Model;

public class Brain
{
    public int Id { get; set; }

    public BrainNeurons Neurons { get; set; }

    public List<Vertex> Vertices { get; set; }

    public Genome Genome { get; set; }
}