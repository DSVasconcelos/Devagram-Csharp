using Devagram.Dtos;
using Devagram.Models;

namespace Devagram.Repository.Impl
{
    public class NotificacaoRepositoryImpl : INotificacaoRepository
    {
        private readonly DevagramContext _context;

        public NotificacaoRepositoryImpl(DevagramContext context)
        {
            _context = context;
        }

        public Interacao GetInteracao(int idInteracao)
        {
            return _context.Interacoes.FirstOrDefault(u => u.Id == idInteracao);
        }

        public List<NotificacaoRespostaDto> GetNotificacoes(int idUsuario)
        {
            var notificacoes =
            from interacoes in _context.Interacoes
            join publicacoes in _context.Publicacoes on interacoes.IdPublicacao equals publicacoes.Id
            join usuarios in _context.Usuarios on publicacoes.IdUsuario equals usuarios.Id
            where publicacoes.IdUsuario == idUsuario && interacoes.visualizado == false
            select new NotificacaoRespostaDto
            {
                IdInteracao = interacoes.Id,
                Tipo = interacoes.tipo,
                Nome = usuarios.Nome,
                idUsuario = usuarios.Id,
                idPublicacao = publicacoes.Id
            };
            
            return notificacoes.ToList();
        }

        public void Notificar(Interacao interacao) //adiciona a nova interação na tabela
        {
            _context.Add(interacao);
            _context.SaveChanges();
        }


        public void VerNotificacao(Interacao notificacao)
        {
            _context.Update(notificacao);
            _context.SaveChanges();
        }
    }
}

