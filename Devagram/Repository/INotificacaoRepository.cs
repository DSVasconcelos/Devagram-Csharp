using Devagram.Dtos;
using Devagram.Models;

namespace Devagram.Repository
{
    public interface INotificacaoRepository
    {
        public void Notificar(Interacao interacao); //aciona a funcionalidade para inserir a nova notificacão na tabela

        List<NotificacaoRespostaDto> GetNotificacoes(int idUsuario);
        
        void VerNotificacao(Interacao interacao);
        
        Interacao GetInteracao(int idInteracao);
    }
}