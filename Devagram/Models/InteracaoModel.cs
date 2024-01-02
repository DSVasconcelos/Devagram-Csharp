using System.ComponentModel.DataAnnotations.Schema;

namespace Devagram.Models
{
    public class Interacao
    {
        public int Id { get; set; }
        public bool visualizado { get; set; }
        public string tipo { get; set; }
        public int? IdPublicacao { get; set; }
        public int? IdUsuario { get; set; }
        

        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; private set; }

        [ForeignKey("IdPublicacao")]
        public virtual Publicacao Publicacao { get; private set; }
    }
}
