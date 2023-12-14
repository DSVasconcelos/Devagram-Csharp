using Devagram.Dtos;
using Devagram.Models;
using Devagram.Repository;
using Devagram.Services;
using Devagram.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Devagram.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IUsuarioRepository _usuarioRepository;

        public LoginController(ILogger<LoginController> logger, IUsuarioRepository usuarioRepository)
        {
            _logger = logger;
            _usuarioRepository = usuarioRepository;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult EfetuarLogin([FromBody] LoginRequisicaoDto loginRequisicao) 
        {
            try
            {
                if (!String.IsNullOrEmpty(loginRequisicao.Senha) && !String.IsNullOrEmpty(loginRequisicao.Email) &&
                   !String.IsNullOrWhiteSpace(loginRequisicao.Senha) && !String.IsNullOrWhiteSpace(loginRequisicao.Email))

                {
                    Usuario usuario = _usuarioRepository.getUsuarioPorLoginSenha(loginRequisicao.Email.ToLower(), MD5Utils.GerarHashMD5(loginRequisicao.Senha));

                    if (usuario != null)
                    {
                        return Ok(new LoginRespostaDto()
                        {
                            Email = usuario.Email,
                            Nome = usuario.Nome,
                            Token = TokenService.CriarToken(usuario)
                        });
                    }
                    else
                    {
                        return BadRequest(new ErrorRespostaDto()
                        {
                            descricao = "Email ou senha invalido",
                            status = StatusCodes.Status400BadRequest
                        });
                    }
                }
                else
                {
                    return BadRequest(new ErrorRespostaDto()
                    {
                        descricao = "O usuario não preencheu os campos corretamente",
                        status = StatusCodes.Status400BadRequest
                    });
                }
            }

            catch (Exception e)
            {
                _logger.LogError("Ocorreu um erro no Login: " + e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    descricao = "Ocorreu um erro ao fazer Login",
                    status = StatusCodes.Status500InternalServerError
                });
            } 
        }

    }
}
