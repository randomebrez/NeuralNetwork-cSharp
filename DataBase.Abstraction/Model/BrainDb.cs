using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBase.Abstraction.Model
{
    [Table("brains")]
    public class BrainDb
    {
        [Column("brain_id"), Key, Required]
        public int Id { get; set; }

        [Column("simulation_id"), Required]
        public int SimulationId { get; set; }

        [Column("generation_id"), Required]
        public int GenerationId { get; set; }

        [Column("brain_unique_id"), Required]
        public string UniqueId { get; set; }

        [Column("parentA_unique_id"), Required]
        public string ParentA { get; set; }

        [Column("parentB_unique_id"), Required]
        public string ParentB { get; set; }

        [Column("genome"), Required]
        public string Genome { get; set; }

        [Column("score")]
        public float Score { get; set; }
    }
}
