using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeuralNetwork.DataBase.Abstraction.Model
{
    [Table("input_values")]
    public class InputDb
    {
        [Column("id"), Key, Required]
        public int Id { get; set; }

        [Column("step_id"), Required]
        public int StepId { get; set; }

        [Column("values"), Required]
        public string Values { get; set; }

        [ForeignKey(nameof(StepId))]
        public UnitStepDb UnitStep { get; set; }
    }
}
