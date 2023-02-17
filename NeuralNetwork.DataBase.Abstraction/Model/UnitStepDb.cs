using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeuralNetwork.DataBase.Abstraction.Model
{
    [Table("unit_steps")]
    public class UnitStepDb
    {
        [Column("step_id"), Key, Required]
        public int Id { get; set; }

        [Column("unit_id"), Required]
        public int UnitId { get; set; }

        [Column("life_step_id"), Required]
        public int LifeStep { get; set; }

        [Column("position"), Required]
        public string Position { get; set; }

        [ForeignKey(nameof(UnitId))]
        public UnitDb Unit { get; set; }
    }
}
