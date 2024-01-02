using Devagram.Dtos;
using Devagram.Models;
using Devagram.Repository;
using Devagram.Repository.Impl;
using Microsoft.AspNetCore.Mvc;


namespace Devagram.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificacaoController : BaseController
    {
        private readonly ILogger<PublicacaoController> _logger;
        private readonly INotificacaoRepository _notificacaoRepository;
        public NotificacaoController(ILogger<PublicacaoController> logger,
                                    IUsuarioRepository usuarioRepository, INotificacaoRepository notificacaoRepository) : base(usuarioRepository)
        {
            _logger = logger;
            _notificacaoRepository = notificacaoRepository;
        }

        [HttpGet]
        public IActionResult TrazerNotificacoes() 
        {
            try {
                List<NotificacaoRespostaDto> notificacoes = _notificacaoRepository.GetNotificacoes(LerToken().Id) ;
                NotificacaoRequisicaoDto notificacoDto;
               
                foreach (NotificacaoRespostaDto notificacoesVisualizadas in notificacoes) {
                    Interacao interacao = _notificacaoRepository.GetInteracao(notificacoesVisualizadas.IdInteracao);
                    interacao.visualizado = true;
                    _notificacaoRepository.VerNotificacao(interacao);
                }
                
                
                return Ok(notificacoes);

                
            }
            catch (Exception e)
            {
                _logger.LogError("Não foi possível carregar as notificacoes");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    descricao = "Ocorreu o seguinte erro: " + e.Message,
                    status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}
