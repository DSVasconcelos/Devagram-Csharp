using System.ComponentModel.DataAnnotations.Schema;

namespace Devagram.Models
{
    public class Seguidor
    {

        public int id { get; set; }
        public int? idUsuarioSeguidor { get; set; }
        public int? idUsuarioSeguido { get; set; }
        
        [ForeignKey("idUsuarioSeguidor")]
        public virtual Usuario UsuarioSeguidor { get; private set; }

        [ForeignKey("idUsuarioSeguido")]
        public virtual Usuario UsuarioSeguido { get; private set; }
    }
}
