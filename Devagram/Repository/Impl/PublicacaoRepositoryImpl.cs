using Devagram.Dtos;
using Devagram.Migrations;
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

        public List<PublicacaoFeedRespostaDto> GetPublicacoesFeed(int idUsuario)
        {
            var feed =
                from publicacoes in _context.Publicacoes
                join seguidores in _context.Seguidores on publicacoes.IdUsuario equals seguidores.idUsuarioSeguido
                where seguidores.idUsuarioSeguidor == idUsuario
                select new PublicacaoFeedRespostaDto
                {
                    IdPublicacao = publicacoes.Id,
                    Descricao = publicacoes.Descricao,
                    Foto = publicacoes.Foto,
                    IdUsuario = (int)publicacoes.IdUsuario
                };
            return feed.ToList();
        }

        public List<PublicacaoFeedRespostaDto> GetPublicacoesFeedUsuario(int idUsuario)
        {
            var feedUsuario =
                from publicacoes in _context.Publicacoes
                where publicacoes.IdUsuario == idUsuario
                select new PublicacaoFeedRespostaDto
                {
                    IdPublicacao = publicacoes.Id,
                    Descricao = publicacoes.Descricao,
                    Foto = publicacoes.Foto,
                    IdUsuario = (int)publicacoes.IdUsuario
                };
            return feedUsuario.ToList();
        }

        public void Publicar(Publicacao publicacao)
        {
            _context.Add(publicacao);
            _context.SaveChanges();
        }
    }
}
