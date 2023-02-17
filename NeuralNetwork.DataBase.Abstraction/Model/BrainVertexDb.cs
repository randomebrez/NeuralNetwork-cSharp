using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeuralNetwork.DataBase.Abstraction.Model
{
    [Table("brain_vertices")]
    public class BrainVertexDb
    {
        [Column("vertex_id"), Key, Required]
        public int Id { get; set; }

        [Column("vertex_identifier"), Required]
        public string Identifier { get; set; }
    }
}
