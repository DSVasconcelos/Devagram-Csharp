using Devagram.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Devagram.Services
{
    public class TokenService
    {
        public static string CriarToken(Usuario usuario)
        {
            var TokenHandler = new JwtSecurityTokenHandler();
            var ChaveCriptografia = Encoding.ASCII.GetBytes(ChaveJWT.ChaveSecreta);
            var TokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Sid, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuario.Nome)
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(ChaveCriptografia), SecurityAlgorithms.HmacSha256Signature)
            };
            var Token = TokenHandler.CreateToken(TokenDescriptor);

            return TokenHandler.WriteToken(Token);
        }
    }
}
