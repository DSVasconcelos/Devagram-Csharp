using Devagram.Dtos;
using Devagram.Models;

namespace Devagram.Repository
{
    public interface IPublicacaoRepository
    {
        List<PublicacaoFeedRespostaDto> GetPublicacoesFeed(int idUsuario);
        List<PublicacaoFeedRespostaDto> GetPublicacoesFeedUsuario(int idUsuario);
        public void Publicar(Publicacao publicacao);
        
    }
}
