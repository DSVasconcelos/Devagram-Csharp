using Devagram.Models;

namespace Devagram.Repository.Impl
{
    public class CurtidaRepositoryImpl : ICurtidasRepository
    {
        private readonly DevagramContext _context;

        public CurtidaRepositoryImpl(DevagramContext context)
        {
            _context = context;
        }

        public void Curtir(Curtida curtida)
        {
            _context.Add(curtida);
            _context.SaveChanges();
        }

        public void Descurtir(Curtida curtida)
        {
            _context.Remove(curtida);
            _context.SaveChanges();
        }

        public Curtida GetCurtida(int IdPublicacao, int IdUsuario)
        {
            return _context.Curtidas.FirstOrDefault(c => 
                                                    c.IdPublicacao == IdPublicacao && 
                                                    c.IdUsuario == IdUsuario);
        }
    }
}
