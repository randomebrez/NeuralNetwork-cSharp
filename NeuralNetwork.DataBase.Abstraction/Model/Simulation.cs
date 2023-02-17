using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeuralNetwork.DataBase.Abstraction.Model
{
    [Table("simulations")]
    public class Simulation
    {
        [Column("simulation_id"), Key, Required]
        public int Id { get; set; }
    }
}
