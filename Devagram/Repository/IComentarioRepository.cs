using Devagram.Models;

namespace Devagram.Repository
{
    public interface IComentarioRepository
    {
        public void Comentar(Comentario comentario);
        List<Comentario> GetComentariosPorPublicacao(int idPublicacao);
    }
}
