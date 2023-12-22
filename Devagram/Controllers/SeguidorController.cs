using Devagram.Dtos;
using Devagram.Models;
using Devagram.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Devagram.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class SeguidorController : BaseController
    {
        private readonly ILogger<SeguidorController> _logger;
        private readonly ISeguidorRepository _seguidorRepository;

        public SeguidorController(ILogger<SeguidorController> logger,
                                  ISeguidorRepository seguidorRepository, 
                                  IUsuarioRepository usuarioRepository) : base(usuarioRepository)
        {
            _logger = logger;
            _seguidorRepository = seguidorRepository;
        }

        [HttpPut]
        public IActionResult Seguir(int idSeguido) {
            try 
            {
                Usuario usuarioSeguido = _usuarioRepository.GetUsuarioPorId(idSeguido); //busca o usuario seguido pelo id
                Usuario usuarioSeguidor = LerToken(); //obtem o id do usuario logado através do token

                if (usuarioSeguido != null)
                {
                    Seguidor seguidor = _seguidorRepository.GetSeguidor(usuarioSeguidor.Id, usuarioSeguido.Id); //busca na tabela de seguidores se existe relação entre os dois ids
                     if (seguidor != null)
                     {
                         _seguidorRepository.Desseguir(seguidor); //se houver, o usuario logado deixará de seguir
                         return Ok("Você deixou de seguir esse usuario!");
                     }
                     else //se não...
                     {
                         Seguidor seguidorNovo = new Seguidor()
                         {
                             idUsuarioSeguido = usuarioSeguido.Id,
                             idUsuarioSeguidor = usuarioSeguidor.Id
                         };

                         _seguidorRepository.Seguir(seguidorNovo); //os id's (seguidor e seguido) serão relacionados na tabela de seguidores
                        
                         return Ok("Agora você está seguindo esse usuario!");    
                     }
                }
                else {
                    return BadRequest("Não foi possível seguir/desseguir esse usuário...");
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Não foi possível seguir esse usuario");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    descricao = "Ocorreu o seguinte erro: " + e.Message,
                    status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }   
}
