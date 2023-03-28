using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeuralNetwork.DataBase.Abstraction.Model
{
    [Table("brains")]
    public class BrainDb
    {
        [Column("brain_id"), Key, Required]
        public int Id { get; set; }

        [Column("simulation_id"), Required]
        public int SimulationId { get; set; }

        [Column("brain_unique_id"), Required]
        public string UniqueId { get; set; }

        [Column("genome"), Required]
        public string Genome { get; set; }

        [Column("score")]
        public float Score { get; set; }
    }
}
