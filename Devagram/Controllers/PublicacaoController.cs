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
        private readonly IComentarioRepository _comentarioRepository;
        private readonly ICurtidasRepository _curtidaRepository;

        public PublicacaoController(ILogger<PublicacaoController> logger,
                                  IPublicacaoRepository publicacaoRepository,
                                  IUsuarioRepository usuarioRepository,
                                  IComentarioRepository comentarioRepository,
                                  ICurtidasRepository curtidaRepository) : base(usuarioRepository) //para obter os dados de quem vai publicar
        {
            _logger = logger;
            _publicacaoRepository = publicacaoRepository;
            _comentarioRepository = comentarioRepository;
            _curtidaRepository = curtidaRepository;
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

        [HttpGet]
        [Route("feed")]
        public IActionResult FeedHome() { 
            try
            {
                //cria uma lista chamada feed, atribuindo à ela todas as publicacoes criadas por USUARIO_SEGUIDOS pelo USUARIO_LOGADO
                List<PublicacaoFeedRespostaDto> feed = _publicacaoRepository.GetPublicacoesFeed(LerToken().Id);
                foreach (PublicacaoFeedRespostaDto feedResposta in feed) //percorre por cada uma das publicacoes encontradas, no feed coletando os dados à serem exibidos
                {
                    Usuario usuario = _usuarioRepository.GetUsuarioPorId(feedResposta.IdUsuario); //busca e cria um objeto com os dados de foto, nome e id do usuario que fez a postagem
                    UsuarioRespostaDto usuarioRespostaDto = new UsuarioRespostaDto()
                    {
                        Nome = usuario.Nome,
                        Avatar = usuario.FotoPerfil,
                        IdUsuario = usuario.Id
                    };
                    feedResposta.Usuario = usuarioRespostaDto;//atribui ao feed os dados do usuario que fez a publicacao que será exibida

                    List<Comentario> comentarios = _comentarioRepository.GetComentariosPorPublicacao(feedResposta.IdPublicacao);//busca os comentarios da publicacao que será exibida
                    feedResposta.Comentarios = comentarios; //atribui ao feed os comentarios da publicacao que será exibida

                    List<Curtida> curtidas = _curtidaRepository.GetCurtidaPorPublicacao(feedResposta.IdPublicacao);//busca as curtidas da publicacao que será exibida
                    feedResposta.Curtidas = curtidas;//atribui ao feed as curtidas da publicacao que será exibida

                }
                return Ok(feed);
            }
            catch (Exception e) {
                _logger.LogError("Não foi possível carregar o feed");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    descricao = "Ocorreu o seguinte erro: " + e.Message,
                    status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpGet]
        [Route("feedusuario")]
        public IActionResult FeedUsuario(int idUsuario)
        {
            try
            {
                //cria uma lista chamada feed, atribuindo à ela todas as publicacoes criadas por USUARIO_SEGUIDOS pelo USUARIO_LOGADO
                List<PublicacaoFeedRespostaDto> feed = _publicacaoRepository.GetPublicacoesFeedUsuario(idUsuario);
                foreach (PublicacaoFeedRespostaDto feedResposta in feed) //percorre por cada uma das publicacoes encontradas, no feed coletando os dados à serem exibidos
                {
                    Usuario usuario = _usuarioRepository.GetUsuarioPorId(feedResposta.IdUsuario); //busca e cria um objeto com os dados de foto, nome e id do usuario que fez a postagem
                    UsuarioRespostaDto usuarioRespostaDto = new UsuarioRespostaDto()
                    {
                        Nome = usuario.Nome,
                        Avatar = usuario.FotoPerfil,
                        IdUsuario = usuario.Id
                    };
                    feedResposta.Usuario = usuarioRespostaDto;//atribui ao feed os dados do usuario que fez a publicacao que será exibida

                    List<Comentario> comentarios = _comentarioRepository.GetComentariosPorPublicacao(feedResposta.IdPublicacao);//busca os comentarios da publicacao que será exibida
                    feedResposta.Comentarios = comentarios; //atribui ao feed os comentarios da publicacao que será exibida

                    List<Curtida> curtidas = _curtidaRepository.GetCurtidaPorPublicacao(feedResposta.IdPublicacao);//busca as curtidas da publicacao que será exibida
                    feedResposta.Curtidas = curtidas;//atribui ao feed as curtidas da publicacao que será exibida

                }
                return Ok(feed);
            }
            catch (Exception e)
            {
                _logger.LogError("Não foi possível carregar o feed desse usuario");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    descricao = "Ocorreu o seguinte erro: " + e.Message,
                    status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}
