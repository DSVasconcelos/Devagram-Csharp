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

        public ComentarioController(ILogger<ComentarioController> logger, 
                                    IComentarioRepository comentarioRepository, 
                                    IUsuarioRepository usuarioRepository) : base (usuarioRepository)
        {
            _logger = logger;
            _comentarioRepository = comentarioRepository;
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
                   
                    Comentario comentario = new Comentario();
                    comentario.Descricao = comentarioDto.Descricao;
                    comentario.IdPublicacao = comentarioDto.IdPublicacao;
                    comentario.IdUsuario = LerToken().Id;

                    _comentarioRepository.Comentar(comentario);
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
