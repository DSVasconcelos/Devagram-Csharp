using Devagram.Dtos;
using Devagram.Models;
using Devagram.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Devagram.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CurtidaController : BaseController
    {
        private readonly ILogger<CurtidaController> _logger;
        private readonly ICurtidasRepository _curtidaRepository;
        private readonly INotificacaoRepository _notificacaoRepository;

        public CurtidaController(ILogger<CurtidaController> logger,
                                  ICurtidasRepository curtidaRepository,
                                  IUsuarioRepository usuarioRepository,
                                  INotificacaoRepository notificacaoRepository) : base(usuarioRepository)
        {
            _logger = logger;
            _curtidaRepository = curtidaRepository;
            _notificacaoRepository = notificacaoRepository;

        }
        [HttpPut]
        public IActionResult Curtir([FromBody] CurtidaRequisicaoDto curtidadto) { 
            try
            {
                if (curtidadto != null)
                {
                    Curtida curtida = _curtidaRepository.GetCurtida(curtidadto.IdPublicacao, LerToken().Id);
                    if (curtida != null)
                    {

                        _curtidaRepository.Descurtir(curtida);
                        return Ok("Publicacao descurtida com sucesso");
                    }
                    else
                    {
                        Curtida curtidaNova = new Curtida()
                        {
                            IdPublicacao = curtidadto.IdPublicacao,
                            IdUsuario = LerToken().Id
                        };

                        _curtidaRepository.Curtir(curtidaNova);

                        Interacao interacao = new Interacao(); //gera um nova notificação
                        interacao.tipo = "curtida";
                        interacao.visualizado = false;
                        interacao.IdUsuario = LerToken().Id;
                        interacao.IdPublicacao = curtidadto.IdPublicacao;

                        _notificacaoRepository.Notificar(interacao); //envia a notificação para ser salva no banco de dados

                        return Ok("Publicacao curtida com sucesso");
                    }

                }
                else {
                    _logger.LogError("Requisição de curtir está vazia");
                    return BadRequest("Requisição de curtir está vazia");
                }

                
            }
            catch (Exception e)
            {
                _logger.LogError("Não foi possível curtir essa publicaco");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    descricao = "Ocorreu o seguinte erro: " + e.Message,
                    status = StatusCodes.Status500InternalServerError
                });

            }
        }
    }
}
