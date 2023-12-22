using Devagram.Models;

namespace Devagram.Repository.Impl
{
    public class PublicacaoRepositoryImpl : IPublicacaoRepository //herda a classe interface
    {
        private readonly DevagramContext _context;

        public PublicacaoRepositoryImpl(DevagramContext context)
        {
            _context = context;
        }

        public void Publicar(Publicacao publicacao)
        {
            _context.Add(publicacao);
            _context.SaveChanges();
        }
    }
}
