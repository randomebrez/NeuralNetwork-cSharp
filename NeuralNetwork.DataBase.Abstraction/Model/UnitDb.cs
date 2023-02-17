using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeuralNetwork.DataBase.Abstraction.Model
{
    [Table("units")]
    public class UnitDb
    {
        [Column("unit_id"), Key, Required]
        public int Id { get; set; }

        [Column("unit_identifier"), Required]
        public string Identifier { get; set; }

        [Column("generation_id"), Required]
        public int GenerationId { get; set; }

        [Column("selection_score")]
        public float? SelectionScore { get; set; }

        [Column("simulation_id"), Required]
        public int SimulationId { get; set; }

        [ForeignKey(nameof(SimulationId))]
        public Simulation Simulation { get; set; }
    }
}
