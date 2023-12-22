using Devagram.Models;

namespace Devagram.Repository
{
    public interface ICurtidasRepository
    {
        public void Curtir(Curtida curtida);

        public void Descurtir(Curtida curtida);

        public Curtida GetCurtida(int IdPublicacao, int IdUsuario);
    }
}
