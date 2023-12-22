using Devagram.Dtos;
using Devagram.Models;
using Devagram.Repository;
using Devagram.Services;
using Microsoft.AspNetCore.Mvc;

namespace Devagram.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublicacaoController :BaseController //herda basecontroller
    {
        private readonly ILogger<PublicacaoController> _logger;
        private readonly IPublicacaoRepository _publicacaoRepository;

        public PublicacaoController(ILogger<PublicacaoController> logger,
                                  IPublicacaoRepository publicacaoRepository,
                                  IUsuarioRepository usuarioRepository) : base(usuarioRepository) //para obter os dados de quem vai publicar
        {
            _logger = logger;
            _publicacaoRepository = publicacaoRepository;
        }
        [HttpPost]
        public IActionResult Publicar([FromForm] PublicacaoRequisicaoDto publicacaodto)
        {
            try
            {
                Usuario usuario = LerToken();
                CosmicService cosmicService = new CosmicService();
                if(publicacaodto != null) 
                {
                    if (String.IsNullOrEmpty(publicacaodto.Descricao) && String.IsNullOrWhiteSpace(publicacaodto.Descricao))
                    {
                        _logger.LogError("Descrição invalida!");
                        return BadRequest("É obrigatorio uma descrição em toda as publicações!");
                    }
                    if (publicacaodto.Foto == null) {
                        _logger.LogError("A foto está invalida!");
                        return BadRequest("É obrigatorio uma foto em toda as publicações!");
                    }

                    Publicacao publicacao = new Publicacao()
                    {
                        Descricao = publicacaodto.Descricao,
                        IdUsuario = usuario.Id,
                        Foto = cosmicService.EnviarImagem(new ImagemDto { Imagem = publicacaodto.Foto, Nome = "publicacao" })
                    };

                    _publicacaoRepository.Publicar(publicacao); 
                }
                return Ok("Publicação criada com sucesso!");
            }
            catch (Exception e)
            {
                _logger.LogError("Não foi possível criar a publicação!");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    descricao = "Ocorreu o seguinte erro: " + e.Message,
                    status = StatusCodes.Status500InternalServerError
                });
            }
        }

    }
}
