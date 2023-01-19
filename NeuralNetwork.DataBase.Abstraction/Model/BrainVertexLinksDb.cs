using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeuralNetwork.DataBase.Abstraction.Model
{
    [Table("brain_vertex_links")]
    public class BrainVertexLinksDb
    {
        [Column("link_id"), Key, Required]
        public int Id { get; set; }

        [Column("brain_id"), Required]
        public int BrainId { get; set; }

        [Column("vertex_id"), Required]
        public int VertexId { get; set; }

        [Column("vertex_weight"), Required]
        public float VertexWeight { get; set; }

        [ForeignKey(nameof(BrainId))]
        public BrainDb Brain { get; set; }

        [ForeignKey(nameof(VertexId))]
        public BrainVertexDb Vertex { get; set; }
    }
}
