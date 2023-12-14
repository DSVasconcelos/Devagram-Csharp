using Devagram.Models;
using Devagram.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Devagram.Controllers
{
    [Authorize]
    public class BaseController : ControllerBase
    {
        protected readonly IUsuarioRepository _usuarioRepository;

        public BaseController(IUsuarioRepository usuarioRepository)
        {
           
            _usuarioRepository = usuarioRepository;
        }

        protected Usuario LerToken() //esse metodo só vai ser visivel pelas classes que herdem a BaseController
        {
            var idUsuario = User.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).FirstOrDefault(); //faz a leitura do id no token referente ao usuario logado

            if (string.IsNullOrEmpty(idUsuario))
            {
                return null;
            }
            else {
                return _usuarioRepository.GetUsuarioPorId(int.Parse(idUsuario));
            }
        }
    }
}
