using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeuralNetwork.DataBase.Abstraction.Model
{
    [Table("brains")]
    public class BrainDb
    {
        [Column("brain_id"), Key, Required]
        public int Id { get; set; }

        [Column("unit_id"), Required]
        public int UnitId { get; set; }

        [ForeignKey(nameof(UnitId))]
        public UnitDb Unit { get; set; }
    }
}
