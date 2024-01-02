using Devagram.Dtos;
using Devagram.Models;
using Devagram.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Devagram.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ComentarioController : BaseController
    {
        private readonly ILogger<ComentarioController> _logger;
        private readonly IComentarioRepository _comentarioRepository;
        private readonly INotificacaoRepository _notificacaoRepository;

        public ComentarioController(ILogger<ComentarioController> logger, 
                                    IComentarioRepository comentarioRepository, 
                                    IUsuarioRepository usuarioRepository, INotificacaoRepository notificacaoRepository) : base (usuarioRepository)
        {
            _logger = logger;
            _comentarioRepository = comentarioRepository;
            _notificacaoRepository = notificacaoRepository;
        }

        [HttpPut]
        public IActionResult Comentar([FromBody] ComentarioRequisicaoDto comentarioDto)
        {
            try
            {
                if (comentarioDto != null)
                {
                    if (String.IsNullOrEmpty(comentarioDto.Descricao) || String.IsNullOrWhiteSpace(comentarioDto.Descricao)) 
                    {
                        _logger.LogError("comentario recebido estava vazio");
                        return BadRequest("Por favor digite seu comentario");
                    }
                   
                    Comentario comentario = new Comentario();//criar o comentario com os dados necessários
                    comentario.Descricao = comentarioDto.Descricao;
                    comentario.IdPublicacao = comentarioDto.IdPublicacao;
                    comentario.IdUsuario = LerToken().Id;

                    _comentarioRepository.Comentar(comentario); //envia o comentario criado para ser salvo no banco de dados

                    Interacao interacao = new Interacao(); //gera um nova notificação
                    interacao.tipo = "comentario";
                    interacao.visualizado = false;
                    interacao.IdUsuario = LerToken().Id;
                    interacao.IdPublicacao = comentarioDto.IdPublicacao;

                    _notificacaoRepository.Notificar(interacao); //envia a notificação para ser salva no banco de dados

                }

                return Ok("Comentario salvo com sucesso!");
            }
            catch (Exception e) 
            {
                _logger.LogError("Não foi comentar nessa publicação");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    descricao = "Ocorreu o seguinte erro: " + e.Message,
                    status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}
