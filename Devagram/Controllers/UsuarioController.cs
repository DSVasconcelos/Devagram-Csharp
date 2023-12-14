using Devagram.Dtos;
using Devagram.Models;
using Devagram.Repository;
using Devagram.Repository.Impl;
using Devagram.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Devagram.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UsuarioController : BaseController
    {
        public readonly ILogger<UsuarioController> _logger;

        public UsuarioController(ILogger<UsuarioController> logger,
            IUsuarioRepository usuarioRepository) : base(usuarioRepository) {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult ObterUsuario()
        {
            try
            {
                Usuario usuario = LerToken();
                return Ok(new UsuarioRespostaDto
                {
                    Nome = usuario.Nome,
                    Email = usuario.Email
                });
            }
            catch (Exception e)
            {
                _logger.LogError("Ocorreu um erro ao obter o usuario");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    descricao = "Ocorreu o seguinte erro: " + e.Message,
                    status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpPut]
        public IActionResult AtualizarUsuario([FromForm] UsuarioRequisicaoDto usuarioDto) 
        {
            try
            {
                Usuario usuario = LerToken();
                var erros = new List<string>();
                if (usuarioDto != null)
                {
                    if (string.IsNullOrEmpty(usuarioDto.Nome) || string.IsNullOrWhiteSpace(usuarioDto.Nome))
                    {
                        erros.Add("Nome invalido");
                    }
                    if (erros.Count > 0)
                    {
                        return BadRequest(new ErrorRespostaDto()
                        {
                            status = StatusCodes.Status400BadRequest,
                            Erros = erros
                        });
                    }
                    else
                    {
                        CosmicService cosmicservice = new CosmicService();
                        usuario.FotoPerfil = cosmicservice.EnviarImagem(new ImagemDto
                        {
                            Imagem = usuarioDto.FotoPerfil,
                            Nome = usuarioDto.Nome.Replace(" ", "")
                        });
                        usuario.Nome = usuarioDto.Nome;

                        _usuarioRepository.AtualizarUsuario(usuario); 
                    }
                }
                return Ok("Dados atualizados com sucesso!");
            }
            catch (Exception e)
            {
                _logger.LogError("Ocorreu um erro ao atualizar os dados do usuario");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    descricao = "Ocorreu o seguinte erro: " + e.Message,
                    status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult SalvarUsuario([FromForm] UsuarioRequisicaoDto usuarioDto)
        {
            try
            {
                var erros = new List<string>();
                if (usuarioDto != null)
                
                {
                    if (string.IsNullOrEmpty(usuarioDto.Nome) || string.IsNullOrWhiteSpace(usuarioDto.Nome))
                    {
                        erros.Add("Nome invalido");
                    }
                    if (string.IsNullOrEmpty(usuarioDto.Email) || string.IsNullOrWhiteSpace(usuarioDto.Email) || !usuarioDto.Email.Contains("@"))
                    {
                        erros.Add("Email invalido");
                    }
                    if (string.IsNullOrEmpty(usuarioDto.Senha) || string.IsNullOrWhiteSpace(usuarioDto.Senha))
                    {
                        erros.Add("Senha invalido");
                    }

                    if (erros.Count > 0)
                    {
                        return BadRequest(new ErrorRespostaDto()
                        {
                            status = StatusCodes.Status400BadRequest,
                            Erros = erros
                        });
                    }

                    CosmicService cosmicService = new CosmicService();
                    Usuario usuario = new Usuario() { 
                        Email=usuarioDto.Email,
                        Senha=usuarioDto.Senha,
                        Nome=usuarioDto.Nome,
                        FotoPerfil = cosmicService.EnviarImagem(new ImagemDto {
                            Imagem = usuarioDto.FotoPerfil, Nome = usuarioDto.Nome.Replace(" ", "") 
                        }),
                    };

                    usuario.Senha = Utils.MD5Utils.GerarHashMD5(usuario.Senha);
                    usuario.Email = usuario.Email.ToLower();

                    if (!_usuarioRepository.VerificarEmail(usuario.Email))
                    {
                        _usuarioRepository.Salvar(usuario);
                    }
                    else {
                        return BadRequest(new ErrorRespostaDto()
                        {
                            status = StatusCodes.Status400BadRequest,
                            descricao = "Usuario já cadastrado"
                        });
                    }
                }

                return Ok("Usuario foi salvo com sucesso");
            }
            catch (Exception e)
            {
                _logger.LogError("Ocorreu um erro ao salvar o usuario");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    descricao = "Ocorreu o seguinte erro: " + e.Message,
                    status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}
